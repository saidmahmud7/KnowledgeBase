using Infrastructure.Data;
using Infrastructure.Extensions;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Extensions;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;


var builder = WebApplication.CreateBuilder(args);
builder.Host.AddSerilogLogger();
builder.Host.UseSerilog();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddServices(builder.Configuration);
builder.Services.SwaggerConfigurationServices(); 
builder.Services.AuthConfigureServices(builder.Configuration);
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
try
{
    using var scope = app.Services.CreateScope();
    var serviceProvider = scope.ServiceProvider;

    // 1️⃣ Применяем миграции
    var dataContext = serviceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();

    // 2️⃣ Заполняем БД начальными данными (сидер)
    var seeder = serviceProvider.GetRequiredService<Seeder>();
    await seeder.SeedRole();
    await seeder.SeedUser();
    Console.WriteLine("Приложение успешно запущено!");
}
catch (Exception e)
{
    Console.WriteLine($"Ошибка при запуске: {e.Message}");
    throw; 
}

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
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "Hello from KnowledgeBase!");
app.Run();
