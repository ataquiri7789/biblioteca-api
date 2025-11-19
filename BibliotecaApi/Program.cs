using Amazon.Lambda.AspNetCoreServer.Hosting;
using BibliotecaApi.Api.Middlewares;
using BibliotecaApi.Application.Configuracion;
using BibliotecaApi.Infrastructure.Configuracion;

var builder = WebApplication.CreateBuilder(args);

var cadenaConexion = builder.Configuration.GetConnectionString("DefaultConnection")!;


builder.Services
    .AgregarAplicacion()
    .AgregarInfraestructura(cadenaConexion);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

app.UseMiddleware<ManejadorErroresMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();