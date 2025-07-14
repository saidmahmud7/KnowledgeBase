using Infrastructure.Extensions;
using Microsoft.Extensions.FileProviders;
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
app.UseStaticFiles();
var uploadsPath = Path.Combine("/tmp", "uploads");
if (!Directory.Exists(uploadsPath)) Directory.CreateDirectory(uploadsPath); 

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});
app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var swaggerProvider = scope.ServiceProvider.GetRequiredService<ISwaggerProvider>();
    var swagger = swaggerProvider.GetSwagger("v1");

    var swaggerJson = swagger.SerializeAsJson(Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0);
    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "swagger.json"), swaggerJson);
}

app.UseRouting();

app.UseCors("AllowReactApp");

// app.UseHttpsRedirection(); // отключено для Render

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello from KnowledgeBase!");

app.Run();
