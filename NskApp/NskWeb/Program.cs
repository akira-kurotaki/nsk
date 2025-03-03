using NskWeb.Common.Extensions;
using NskWeb.Core.Config;
using NskWeb.Core.Cookie;
using NskWeb.Core.Filter;
using NskWeb.Core.Middleware;
using Community.Microsoft.Extensions.Caching.PostgreSql;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Filters;
using CoreLibrary.Core.ModelBinder;
using CoreLibrary.Core.Service;
using CoreLibrary.Core.Session;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.WebEncoders;
using NLog;
using NLog.Web;
using ReportService.Core;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

// SJIS(Shift_JIS)を使用可能にする
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// ロガー
var logger = LogManager.GetCurrentClassLogger();
logger.Info("--- Application Start ---");

var builder = WebApplication.CreateBuilder(args);

// ログ出力をNLog機能に使うように設定する
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Viewを使ったController追加
// AddRazorRuntimeCompilation Razor（cshtml）のランタイムコンパイル機能有効化
// AddSessionStateTempDataProvider TempDataの格納先をセッションにする
if (ConfigUtil.Get(CoreConst.APP_ENV) == "debug")
{
    builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddSessionStateTempDataProvider()
    .AddJsonOptions(options =>
    {
        // 既定のキャメルケース書式設定の代わりに、パスカルケース書式設定を構成
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
}
else
{
    builder.Services.AddControllersWithViews(options =>
    {
        // 全てのコントローラーに[Authorize]を付与する
        options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
    })
    .AddRazorRuntimeCompilation()
    .AddSessionStateTempDataProvider()
    .AddJsonOptions(options =>
    {
        // 既定のキャメルケース書式設定の代わりに、パスカルケース書式設定を構成
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
}

// 認証エラー時にログインURLにリダイレクトする
builder.Services.AddScoped<CustomCookieAuthenticationEvents>();

// 認証ミドルウェアサービスを追加
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(ConfigUtil.GetInt(CoreConst.AUTHENTICATION_COOKIE_TIMEOUT));
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.Path = "/";
    options.Cookie.Domain = ConfigUtil.Get("HttpCookies_Domain");
    options.Cookie.HttpOnly = true;

    if (ConfigUtil.GetBool("HttpCookies_Use_Secure"))
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    }

    // 未認証（401 Unauthorized）時のリダイレクト先
    options.EventsType = typeof(CustomCookieAuthenticationEvents);
    // 権限なし（403 Forbidden）時のリダイレクト先
    options.AccessDeniedPath = new PathString("/F900/D900004/Init");
});

// HttpContextAccessorを各クラスへDI可能とする
builder.Services.AddHttpContextAccessor();

//IsNewSession代替機能の対応
builder.Services.AddTransient<ISessionStore, CustomDistributedSessionStore>();

// セッション管理
switch (ConfigUtil.Get("SessionState"))
{
    case "redis":
        // セッション管理でRedisを使用する場合
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            // 接続文字列
            options.Configuration = ConfigUtil.Get("SessionState_Redis_Configuration");
            options.InstanceName = ConfigUtil.Get("SessionState_Redis_InstanceName");
        });
        break;
    case "postgresql":
        // セッション管理でPostgreSqlを使用する場合(Community.Microsoft.Extensions.Caching.PostgreSql)
        builder.Services.AddDistributedPostgreSqlCache(setup =>
        {
            // DB接続文字列
            setup.ConnectionString = builder.Configuration["PgCache:ConnectionString"];
            // スキーマ名
            setup.SchemaName = builder.Configuration["PgCache:SchemaName"];
            // テーブル名
            setup.TableName = builder.Configuration["PgCache:TableName"];
            // （省略可）期限切れアイテムの自動削除（true：削除しない、false：削除する（デフォルト））
            setup.DisableRemoveExpired = bool.Parse(builder.Configuration["PgCache:DisableRemoveExpired"]);
            // （省略可）必要に応じてアプリ起動ごとにテーブルとデータベース関数を作成（true：作成する（デフォルト）、false：作成しない）
            setup.CreateInfrastructure = bool.Parse(builder.Configuration["PgCache:CreateInfrastructure"]);
            // （省略可）期限切れアイテムの自動削除間隔。デフォルトは30分。最小許容時間は、5分。
            setup.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(int.Parse(builder.Configuration["PgCache:ExpiredItemsDeletionInterval"]));
            // （省略可）読み取り専用データベース、または、ユーザーにwrite権限がない場合にfalseを設定。デフォルト：true
            setup.UpdateOnGetCacheItem = bool.Parse(builder.Configuration["PgCache:UpdateOnGetCacheItem"]);
        });
        break;
    case "memory":
    default:
        // セッション管理でオンメモリを使用する場合
        builder.Services.AddDistributedMemoryCache();
        break;
}

// セッション管理設定
builder.Services.AddSession(options =>
{
    // セッションタイムアウト
    options.IdleTimeout = TimeSpan.FromMinutes(ConfigUtil.GetInt(CoreConst.SESSION_STATE_TIMEOUT));
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ConfigUtil.Get("HttpCookies_Session_Id");

    if (!string.IsNullOrEmpty(ConfigUtil.Get("PathBase")))
    {
        options.Cookie.Path = ConfigUtil.Get("PathBase");
    }

    if (!string.IsNullOrEmpty(ConfigUtil.Get("HttpCookies_Domain")))
    {
        options.Cookie.Domain = ConfigUtil.Get("HttpCookies_Domain");
    }
});

// クッキー認証ミドルウェアの設定
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    if (ConfigUtil.GetBool("HttpCookies_Use_Secure"))
    {
        options.Secure = CookieSecurePolicy.Always; // secure
    }
    options.HttpOnly = HttpOnlyPolicy.Always;   // httponly
});

// MVC機能
builder.Services.AddMvc(options =>
{
    // 例外フィルター
    options.Filters.Add<CoreExceptionFilter>();

    // ログインユーザをセッションに設定
    options.Filters.Add<LoginUserSettingNskWebAttribute>();

    // ロギング
    options.Filters.Add<LoggingFilter>();

    // リクエストの有効性チェック
    options.Filters.Add<ValidRequestNskWebAttribute>();

    /// システムロックチェック
    options.Filters.Add<ValidSystemLockNskWebAttribute>();

    // カスタムモデルバインド
    options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());

    // 禁止文字モデルバインド
    options.ModelBinderProviders.Insert(0, new ProhibitWordModelBinderProvider());
});

// キャッシュ機能の定義を追加
CacheConfig.RegisterMasterTable();

// 監視機能の定義を追加
if (ConfigUtil.GetBool(CoreConst.SYS_DATE_TIME_FLAG))
{
    // システム時間ファイル監視機能
    builder.Services.AddHostedService<SysDateTimeFileMonitorService>();
}

// 必須エラーメッセージをカスタマイズするためのアダプタークラス
builder.Services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();

// Webアプリ起動・終了時のイベント処理
builder.Services.AddHostedService<LifetimeEventsHostedService>();

// データ保護の構成
if (ConfigUtil.GetBool("DataProtection_Use_Certificate"))
{
    builder.Services.AddDataProtection()    // データ保護を有効に設定
        .SetApplicationName(ConfigUtil.Get("DataProtection_SharedApplicationName"))       // 共通アプリ名を設定
        .PersistKeysToFileSystem(new DirectoryInfo(ConfigUtil.Get("DataProtection_PersistKeysPath")))  // キーの保存場所（分散環境では共有フォルダ）
        .ProtectKeysWithCertificate(new X509Certificate2(ConfigUtil.Get("DataProtection_CertificateFilePath"),
                                                            ConfigUtil.Get("DataProtection_CertificatePassword")));  // X.509証明書
}
else
{
    builder.Services.AddDataProtection()    // データ保護を有効に設定
        .SetApplicationName(ConfigUtil.Get("DataProtection_SharedApplicationName"))       // 共通アプリ名を設定
        .PersistKeysToFileSystem(new DirectoryInfo(ConfigUtil.Get("DataProtection_PersistKeysPath")));  // キーの保存場所（分散環境では共有フォルダ）
}

// アプリのCookie設定
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
    if (!string.IsNullOrEmpty(ConfigUtil.Get("PathBase")))
    {
        options.Cookie.Path = ConfigUtil.Get("PathBase");
    }

    if (!string.IsNullOrEmpty(ConfigUtil.Get("HttpCookies_Domain")))
    {
        options.Cookie.Domain = ConfigUtil.Get("HttpCookies_Domain");
    }

    options.Cookie.HttpOnly = true;

    if (ConfigUtil.GetBool("HttpCookies_Use_Secure"))
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // secure
    }
});

// ASP.NET MVCの動作と同様にチェックボックスの下にhiddenを表示する。
builder.Services.Configure<MvcViewOptions>(options =>
{
    options.HtmlHelperOptions.CheckBoxHiddenInputRenderMode = CheckBoxHiddenInputRenderMode.Inline;
});

// ビューのレンダリング時にマルチバイト文字をHTMLエンコードさせない
builder.Services.Configure<WebEncoderOptions>(options =>
{
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});

// リアル帳票出力サービス呼び出しクライアント登録
if (ConfigUtil.GetBool("ReportServiceCheckServerTrusted"))
{
    builder.Services.AddHttpClient<ReportServiceClient>();
}
else
{
    // サーバ証明書を検証しない
    builder.Services.AddHttpClient<ReportServiceClient>().ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = delegate { return true; }
    });
}

// リクエストボディをHTTPフォームとして読み込む際のオプション
//builder.Services.Configure<FormOptions>(options =>
//{
//    // マルチパート本文のサイズ（デフォルトは128MB（134,217,728バイト））
//    //options.MultipartBodyLengthLimit = long.MaxValue;
//    // 許可するフォームエントリの数の制限（デフォルトは1024）
//    options.ValueCountLimit = 30000;
//});

// デフォルトメッセージを日本語に変換する処理
builder.Services.ConfigureModelBindingMessages();

var app = builder.Build();

// エラーハンドリングミドルウエア
app.UseErrorHandlerMiddleware();

// リクエストの入口・出口の処理
app.UseCustomMiddlewareRequest();

// ベースパス設定
if (!string.IsNullOrEmpty(ConfigUtil.Get("PathBase")))
{
    app.UsePathBase(ConfigUtil.Get("PathBase"));
}

//app.UseStatusCodePagesWithRedirects("~/F90/D9004");

// 静的ファイル ミドルウェア
app.UseStaticFiles();

// Cookieポリシー ミドルウェア
app.UseCookiePolicy();

// ルーティング ミドルウェア
app.UseRouting();

// 認証ミドルウェア
// UseAuthenticationとUseAuthorizationはMapRazorPagesやMapDefaultControllerRouteなどのMapメソッドの前に呼び出す
app.UseAuthentication();
// 承認ミドルウェア
app.UseAuthorization();

// セッション有効化
// UseSessionは、UseRoutingの後、かつ、MapRazorPagesとMapDefaultControllerRouteの前に呼び出す
app.UseSession();

// 認証要求の発生時
app.UseCustomMiddlewareAuthenticateRequest();

// エンドポイント ルーティング ミドルウェア
app.UseEndpoints(endpoints =>
{
    // Areaを使った設定
    _ = endpoints.MapControllerRoute(
        name: "Default",
        pattern: "{area:exists}/{controller}/{action}/{id?}",
        defaults: new { area = "F000", controller = "D000000", action = "Init" },
        dataTokens: new { area = "F000" }
    );
});

app.Run();
