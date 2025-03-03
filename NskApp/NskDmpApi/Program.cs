using BaseDmpApi.Core.Config;
using BaseDmpApi.Middleware;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Service;
using CoreLibrary.Core.Utility;
using NLog.Web;
using System.Text;

// SJIS(Shift_JIS)を使用可能にする
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// ログ出力をNLog機能に使うように設定する
builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // 既定のキャメルケース書式設定の代わりに、パスカルケース書式設定を構成
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// キャッシュ機能の定義を追加
CacheConfig.RegisterMasterTable();

// 監視機能の定義を追加
if (ConfigUtil.GetBool(CoreConst.SYS_DATE_TIME_FLAG))
{
    // システム時間ファイル監視機能
    builder.Services.AddHostedService<SysDateTimeFileMonitorService>();
}

// Webアプリ起動・終了時のイベント処理
builder.Services.AddHostedService<LifetimeEventsHostedService>();

var app = builder.Build();

app.UseErrorHandlerMiddleware();

// ベースパス設定
if (!string.IsNullOrEmpty(ConfigUtil.Get("PathBase")))
{
    app.UsePathBase(ConfigUtil.Get("PathBase"));
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseLoggingMiddleware();

app.MapControllers();

app.Run();
