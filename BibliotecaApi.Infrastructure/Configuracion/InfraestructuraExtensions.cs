using BibliotecaApi.Infrastructure.Datos;
using BibliotecaApi.Infrastructure.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BibliotecaApi.Infrastructure.Configuracion;

public static class InfraestructuraExtensions
{
    public static IServiceCollection AgregarInfraestructura(
        this IServiceCollection servicios,
        string cadenaConexion)
    {
        servicios.AddDbContext<AppDbContext>(o => o.UseNpgsql(cadenaConexion));
        servicios.AddScoped<ILibroRepositorio, LibroRepositorio>();
        return servicios;
    }
}