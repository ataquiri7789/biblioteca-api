using BibliotecaApi.Application.Dtos;

namespace BibliotecaApi.Application.Servicios;

public interface ILibroServicio
{
    Task<IEnumerable<LibroLeerDto>> ListarAsync();
    Task<LibroLeerDto?> ObtenerPorIdAsync(int id);
    Task<RespuestaDto<LibroLeerDto>> CrearAsync(LibroCrearDto dto);
    Task<RespuestaDto<LibroLeerDto>?> ActualizarAsync(int id, LibroActualizarDto dto);
    Task<RespuestaDto<object>?> EliminarAsync(int id);
}