using Infrastructure.Extensions;
using Microsoft.OpenApi.Extensions;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Loggers/logs.txt", rollingInterval: RollingInterval.Month)
    .Filter.ByExcluding(logEvent =>
    {
        if (!logEvent.Properties.TryGetValue("SourceContext", out var sourceContext)) return false;
        var sourceContextValue = sourceContext.ToString();
        return sourceContextValue.Contains("Microsoft.EntityFrameworkCore.Database.Command");
    })
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

    using var scope = app.Services.CreateScope();
    var swaggerProvider = scope.ServiceProvider.GetRequiredService<ISwaggerProvider>();
    var swagger = swaggerProvider.GetSwagger("v1");

    // Сохранение в JSON
    var swaggerJson = swagger.SerializeAsJson(Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0);
    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "swagger.json"), swaggerJson);
}

app.UseRouting();

app.UseCors("AllowReactApp");

// app.UseHttpsRedirection();  // <- отключено для Render.com

app.UseAuthorization();

app.MapControllers();

// Добавил простой маршрут для проверки доступности
app.MapGet("/", () => "Hello from KnowledgeBase!");

app.Run();