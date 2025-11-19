using System.Data;
using BibliotecaApi.Domain.Entidades;
using BibliotecaApi.Infrastructure.Datos;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BibliotecaApi.Infrastructure.Repositorios;

public class LibroRepositorio : ILibroRepositorio
{
    private readonly AppDbContext _contexto;

    public LibroRepositorio(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IEnumerable<Libro>> ListarActivosAsync()
    {
        return await _contexto.Libros
            .FromSqlRaw("SELECT * FROM vw_libros_activos")
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Libro?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Libros
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id && l.Activo);
    }

    public async Task<int> CrearAsync(Libro libro)
    {
        _contexto.Libros.Add(libro);
        await _contexto.SaveChangesAsync();
        return libro.Id;
    }




    public async Task<int> ActualizarAsync(Libro libro)
    {
        var existente = await _contexto.Libros
            .FirstOrDefaultAsync(l => l.Id == libro.Id && l.Activo);

        if (existente is null)
            return 0;

        existente.Titulo = libro.Titulo;
        existente.Autor = libro.Autor;
        existente.AnioPublicacion = libro.AnioPublicacion;
        existente.Editorial = libro.Editorial;
        existente.Paginas = libro.Paginas;
        existente.Categoria = libro.Categoria;
        existente.Isbn = libro.Isbn;

        await _contexto.SaveChangesAsync();

        return 1;
    }


    public async Task<int> EliminarAsync(int id)
    {
        var libro = await _contexto.Libros
            .FirstOrDefaultAsync(l => l.Id == id && l.Activo);

        if (libro is null)
            return 0;

        libro.Activo = false;
        await _contexto.SaveChangesAsync();

        return 1;
    }


}