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
        using var conexion = (NpgsqlConnection)_contexto.Database.GetDbConnection();
        if (conexion.State != ConnectionState.Open)
            await conexion.OpenAsync();

        using var comando = new NpgsqlCommand(
            "CALL sp_insertar_libro(@p_titulo, @p_autor, @p_anio_publicacion, @p_editorial, @p_paginas, @p_categoria, @p_isbn, @p_id);",
            conexion);

        comando.Parameters.AddWithValue("p_titulo", libro.Titulo);
        comando.Parameters.AddWithValue("p_autor", libro.Autor);
        comando.Parameters.AddWithValue("p_anio_publicacion", libro.AnioPublicacion);
        comando.Parameters.AddWithValue("p_editorial", (object?)libro.Editorial ?? DBNull.Value);
        comando.Parameters.AddWithValue("p_paginas", libro.Paginas);
        comando.Parameters.AddWithValue("p_categoria", (object?)libro.Categoria ?? DBNull.Value);
        comando.Parameters.AddWithValue("p_isbn", (object?)libro.Isbn ?? DBNull.Value);

        var pId = new NpgsqlParameter("p_id", NpgsqlTypes.NpgsqlDbType.Integer)
        {
            Direction = ParameterDirection.Output
        };
        comando.Parameters.Add(pId);

        await comando.ExecuteNonQueryAsync();
        return (int)(pId.Value ?? 0);
    }

    public async Task<bool> ActualizarAsync(Libro libro)
    {
        using var conexion = (NpgsqlConnection)_contexto.Database.GetDbConnection();
        if (conexion.State != ConnectionState.Open)
            await conexion.OpenAsync();

        using var comando = new NpgsqlCommand(
            "CALL sp_actualizar_libro(@p_id, @p_titulo, @p_autor, @p_anio_publicacion, @p_editorial, @p_paginas, @p_categoria, @p_isbn, @p_filas_afectadas);",
            conexion);

        comando.Parameters.AddWithValue("p_id", libro.Id);
        comando.Parameters.AddWithValue("p_titulo", libro.Titulo);
        comando.Parameters.AddWithValue("p_autor", libro.Autor);
        comando.Parameters.AddWithValue("p_anio_publicacion", libro.AnioPublicacion);
        comando.Parameters.AddWithValue("p_editorial", (object?)libro.Editorial ?? DBNull.Value);
        comando.Parameters.AddWithValue("p_paginas", libro.Paginas);
        comando.Parameters.AddWithValue("p_categoria", (object?)libro.Categoria ?? DBNull.Value);
        comando.Parameters.AddWithValue("p_isbn", (object?)libro.Isbn ?? DBNull.Value);

        var pFilas = new NpgsqlParameter("p_filas_afectadas", NpgsqlTypes.NpgsqlDbType.Integer)
        {
            Direction = ParameterDirection.Output
        };
        comando.Parameters.Add(pFilas);

        await comando.ExecuteNonQueryAsync();
        var filas = (int)(pFilas.Value ?? 0);
        return filas > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        using var conexion = (NpgsqlConnection)_contexto.Database.GetDbConnection();
        if (conexion.State != ConnectionState.Open)
            await conexion.OpenAsync();

        using var comando = new NpgsqlCommand(
            "CALL sp_eliminar_libro(@p_id, @p_filas_afectadas);",
            conexion);

        comando.Parameters.AddWithValue("p_id", id);

        var pFilas = new NpgsqlParameter("p_filas_afectadas", NpgsqlTypes.NpgsqlDbType.Integer)
        {
            Direction = ParameterDirection.Output
        };
        comando.Parameters.Add(pFilas);

        await comando.ExecuteNonQueryAsync();
        var filas = (int)(pFilas.Value ?? 0);
        return filas > 0;
    }
}