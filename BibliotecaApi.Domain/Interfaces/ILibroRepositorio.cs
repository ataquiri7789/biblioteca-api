using BibliotecaApi.Domain.Entidades;

public interface ILibroRepositorio
{
    Task<IEnumerable<Libro>> ListarActivosAsync();
    Task<Libro?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Libro libro);
    Task<int> ActualizarAsync(Libro libro);
    Task<int> EliminarAsync(int id);
}