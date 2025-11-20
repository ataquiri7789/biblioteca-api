using System.Text.Json;
using Amazon.Lambda.AspNetCoreServer;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using BibliotecaApi.Application.Servicios;

using BibliotecaApi.Infrastructure.Datos;
using BibliotecaApi.Infrastructure.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


var secretName = builder.Configuration["DB_SECRET_NAME"];

if (!string.IsNullOrEmpty(secretName))
{
    try
    {
        var secretsClient = new AmazonSecretsManagerClient();

        var response = secretsClient.GetSecretValueAsync(
            new GetSecretValueRequest
            {
                SecretId = secretName
            }).GetAwaiter().GetResult();

        if (!string.IsNullOrEmpty(response.SecretString))
        {
            using var doc = JsonDocument.Parse(response.SecretString);
            if (doc.RootElement.TryGetProperty("ConnectionString", out var connProp))
            {
                var connFromSecret = connProp.GetString();
                if (!string.IsNullOrWhiteSpace(connFromSecret))
                {
                    connectionString = connFromSecret;
                }
            }
        }
    }
    catch (Exception ex)
    {
 
        Console.WriteLine($"No se pudo obtener el secret '{secretName}'. Usando appsettings. Detalle: {ex.Message}");
    }
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "No se encontró una cadena de conexión válida. " +
        "Verifica 'DefaultConnection' en appsettings.json o el secret en AWS.");
}


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});


builder.Services.AddScoped<ILibroRepositorio, LibroRepositorio>();
builder.Services.AddScoped<ILibroServicio, LibroServicio>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
