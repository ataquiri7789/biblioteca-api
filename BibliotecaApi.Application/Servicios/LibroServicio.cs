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

    public async Task<LibroLeerDto> CrearAsync(LibroCrearDto dto)
    {
        var libro = new Libro
        {
            Titulo = dto.Titulo,
            Autor = dto.Autor,
            AnioPublicacion = dto.AnioPublicacion,
            Editorial = dto.Editorial,
            Paginas = dto.Paginas,
            Categoria = dto.Categoria,
            Isbn = dto.Isbn
        };

        var id = await _repositorio.CrearAsync(libro);
        var creado = await _repositorio.ObtenerPorIdAsync(id)
                     ?? throw new InvalidOperationException("No se pudo recuperar el libro recién creado.");

        return MapearALeerDto(creado);
    }

    public async Task<bool> ActualizarAsync(int id, LibroActualizarDto dto)
    {
        var existente = await _repositorio.ObtenerPorIdAsync(id);
        if (existente is null) return false;

        existente.Titulo = dto.Titulo;
        existente.Autor = dto.Autor;
        existente.AnioPublicacion = dto.AnioPublicacion;
        existente.Editorial = dto.Editorial;
        existente.Paginas = dto.Paginas;
        existente.Categoria = dto.Categoria;
        existente.Isbn = dto.Isbn;

        return await _repositorio.ActualizarAsync(existente);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        return await _repositorio.EliminarAsync(id);
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