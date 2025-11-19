using BibliotecaApi.Application.Dtos;

namespace BibliotecaApi.Application.Servicios;

public interface ILibroServicio
{
    Task<IEnumerable<LibroLeerDto>> ListarAsync();
    Task<LibroLeerDto?> ObtenerPorIdAsync(int id);
    Task<LibroLeerDto> CrearAsync(LibroCrearDto dto);
    Task<bool> ActualizarAsync(int id, LibroActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}