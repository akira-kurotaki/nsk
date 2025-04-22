using NskReport.Core.Config;
using NskReport.Middleware;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Service;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NLog.Web;
using System.Text;
using System.Text.Json;

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

// ヘルスチェックサービス登録
builder.Services.AddHealthChecks();

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

// ヘルスチェックミドルウエア登録
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json; charset=utf-8";
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions()))
        {
            writer.WriteStartObject();
            writer.WriteString("Status", report.Status.ToString());
            writer.WriteEndObject();
        }
        var json = Encoding.UTF8.GetString(stream.ToArray());
        await context.Response.WriteAsync(json);
    }
});

app.Run();
