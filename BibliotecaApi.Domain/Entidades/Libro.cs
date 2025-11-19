namespace BibliotecaApi.Domain.Entidades;

public class Libro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string Autor { get; set; } = null!;
    public int AnioPublicacion { get; set; }
    public string? Editorial { get; set; }
    public int Paginas { get; set; }
    public string? Categoria { get; set; }
    public string? Isbn { get; set; }
    public bool Activo { get; set; } = true;
}