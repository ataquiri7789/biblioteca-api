using BibliotecaApi.Application.Dtos;
using BibliotecaApi.Domain.Entidades;

namespace BibliotecaApi.Application.Servicios;

public class LibroServicio : ILibroServicio
{
    private readonly ILibroRepositorio _repositorio;

    public LibroServicio(ILibroRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<LibroLeerDto>> ListarAsync()
    {
        var libros = await _repositorio.ListarActivosAsync();
        return libros.Select(MapearALeerDto);
    }

    public async Task<LibroLeerDto?> ObtenerPorIdAsync(int id)
    {
        var libro = await _repositorio.ObtenerPorIdAsync(id);
        return libro is null ? null : MapearALeerDto(libro);
    }

    public async Task<RespuestaDto<LibroLeerDto>> CrearAsync(LibroCrearDto dto)
    {
        var libro = new Libro
        {
            Titulo = dto.Titulo,
            Autor = dto.Autor,
            AnioPublicacion = dto.AnioPublicacion,
            Editorial = dto.Editorial,
            Paginas = dto.Paginas,
            Categoria = dto.Categoria,
            Isbn = dto.Isbn,
            Activo = true
        };

        var id = await _repositorio.CrearAsync(libro);
        var creado = await _repositorio.ObtenerPorIdAsync(id)
                     ?? throw new InvalidOperationException("No se pudo obtener el libro creado.");

        return new RespuestaDto<LibroLeerDto>
        {
            Mensaje = "El libro fue registrado correctamente",
            Datos = new LibroLeerDto
            {
                Id = creado.Id,
                Titulo = creado.Titulo,
                Autor = creado.Autor,
                AnioPublicacion = creado.AnioPublicacion,
                Editorial = creado.Editorial,
                Paginas = creado.Paginas,
                Categoria = creado.Categoria,
                Isbn = creado.Isbn
            }
        };
    }


    public async Task<RespuestaDto<LibroLeerDto>?> ActualizarAsync(int id, LibroActualizarDto dto)
    {
        var libro = new Libro
        {
            Id = id,
            Titulo = dto.Titulo,
            Autor = dto.Autor,
            AnioPublicacion = dto.AnioPublicacion,
            Editorial = dto.Editorial,
            Paginas = dto.Paginas,
            Categoria = dto.Categoria,
            Isbn = dto.Isbn
        };

        var filas = await _repositorio.ActualizarAsync(libro);

        if (filas == 0)
            return null;

        var actualizado = await _repositorio.ObtenerPorIdAsync(id);

        return new RespuestaDto<LibroLeerDto>
        {
            Mensaje = "El libro se actualizó correctamente",
            Datos = new LibroLeerDto
            {
                Id = actualizado!.Id,
                Titulo = actualizado.Titulo,
                Autor = actualizado.Autor,
                AnioPublicacion = actualizado.AnioPublicacion,
                Editorial = actualizado.Editorial,
                Paginas = actualizado.Paginas,
                Categoria = actualizado.Categoria,
                Isbn = actualizado.Isbn
            }
        };
    }



    public async Task<RespuestaDto<object>?> EliminarAsync(int id)
    {
        var filas = await _repositorio.EliminarAsync(id);

        if (filas == 0)
            return null;

        return new RespuestaDto<object>
        {
            Mensaje = "El libro fue eliminado correctamente",
            Datos = null
        };
    }


    private static LibroLeerDto MapearALeerDto(Libro l) => new()
    {
        Id = l.Id,
        Titulo = l.Titulo,
        Autor = l.Autor,
        AnioPublicacion = l.AnioPublicacion,
        Editorial = l.Editorial,
        Paginas = l.Paginas,
        Categoria = l.Categoria,
        Isbn = l.Isbn
    };
}