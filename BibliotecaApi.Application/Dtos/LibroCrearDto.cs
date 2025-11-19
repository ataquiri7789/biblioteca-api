using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Application.Dtos;

public class LibroCrearDto
{
    [Required, MaxLength(200)]
    public string Titulo { get; set; } = null!;

    [Required, MaxLength(150)]
    public string Autor { get; set; } = null!;

    [Range(1000, 9999)]
    public int AnioPublicacion { get; set; }

    [MaxLength(150)]
    public string? Editorial { get; set; }

    [Range(1, 100000)]
    public int Paginas { get; set; }

    [MaxLength(100)]
    public string? Categoria { get; set; }

    [MaxLength(20)]
    public string? Isbn { get; set; }
}