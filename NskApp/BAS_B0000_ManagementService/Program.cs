using BAS_B0000_ManagementService;

var builder = Host.CreateApplicationBuilder(args);

// アプリをWindowsサービスとして動作させる
builder.Services.AddWindowsService(options =>
{
    // サービス名
    options.ServiceName = "BAS_Batch_ManagementService";
});

// サービス
builder.Services.AddHostedService<B0000ManagementService>();

// Windows EventLog設定
#pragma warning disable CA1416 // プラットフォームの互換性を検証
builder.Logging.AddEventLog(eventLogSettings =>
{
    eventLogSettings.SourceName = "BAS_Batch_ManagementService";
});
#pragma warning restore CA1416 // プラットフォームの互換性を検証

var host = builder.Build();
host.Run();
