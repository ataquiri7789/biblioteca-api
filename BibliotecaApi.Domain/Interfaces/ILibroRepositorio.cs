using BibliotecaApi.Domain.Entidades;

public interface ILibroRepositorio
{
    Task<IEnumerable<Libro>> ListarActivosAsync();
    Task<Libro?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Libro libro);
    Task<bool> ActualizarAsync(Libro libro);
    Task<bool> EliminarAsync(int id);
}