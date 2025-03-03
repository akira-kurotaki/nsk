using NSK_B000000_ManagementService;

var builder = Host.CreateApplicationBuilder(args);

// アプリをWindowsサービスとして動作させる
builder.Services.AddWindowsService(options =>
{
    // サービス名
    options.ServiceName = "NSK_Batch_ManagementService";
});

// サービス
builder.Services.AddHostedService<B000000ManagementService>();

// Windows EventLog設定
#pragma warning disable CA1416 // プラットフォームの互換性を検証
builder.Logging.AddEventLog(eventLogSettings =>
{
    eventLogSettings.SourceName = "NSK_Batch_ManagementService";
});
#pragma warning restore CA1416 // プラットフォームの互換性を検証

var host = builder.Build();
host.Run();
