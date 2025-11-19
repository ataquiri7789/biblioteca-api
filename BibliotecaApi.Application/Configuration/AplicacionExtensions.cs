using Microsoft.Extensions.DependencyInjection;

namespace BibliotecaApi.Application.Configuracion;

public static class AplicacionExtensions
{
    public static IServiceCollection AgregarAplicacion(this IServiceCollection servicios)
    {
        servicios.AddScoped<Servicios.ILibroServicio, Servicios.LibroServicio>();
        return servicios;
    }
}