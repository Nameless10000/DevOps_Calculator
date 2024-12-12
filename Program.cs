using OpenTelemetry.Metrics;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddOpenTelemetry()
    .WithMetrics(meterProviderBuilder =>
    {
        meterProviderBuilder.AddPrometheusExporter();

        // Добавляем метрики ASP.NET Core
        meterProviderBuilder.AddMeter("Microsoft.AspNetCore.Hosting",
                                      "Microsoft.AspNetCore.Server.Kestrel");

        // Добавляем метрику для HTTP-соединений
        meterProviderBuilder.AddMeter("Microsoft.AspNetCore.Http.Connections");

        // Настраиваем сбор метрик длительности запросов
        meterProviderBuilder.AddView("http.server.request.duration",
            new ExplicitBucketHistogramConfiguration
            {
                Boundaries = [
                    0, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10
                ]
            });
    });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();


app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics(); // Создает конечную точку для метрик на /metrics
});

app.MapPrometheusScrapingEndpoint();

app.UseAuthorization();

app.MapControllers();

app.Run();
