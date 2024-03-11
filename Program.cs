using ContosoWorker;

var builder = Host.CreateApplicationBuilder(args);
var hostContext = builder.Services;
hostContext.AddSingleton<MonitorLoop>();
hostContext.AddHostedService<QueuedHostedService>();
hostContext.AddSingleton<IBackgroundTaskQueue>(ctx =>
{
    if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
        queueCapacity = 100;
    return new BackgroundTaskQueue(queueCapacity);
});

var host = builder.Build();

var monitorLoop = host.Services.GetRequiredService<MonitorLoop>();
monitorLoop.StartMonitorLoop();

host.Run();
