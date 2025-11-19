using BibliotecaApi.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Infrastructure.Datos;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opciones) : base(opciones) { }

    public DbSet<Libro> Libros => Set<Libro>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Libro>(entidad =>
        {
            entidad.ToTable("libros");

            entidad.HasKey(l => l.Id);

       
            entidad.Property(l => l.Id)
                .HasColumnName("id");

            entidad.Property(l => l.Titulo)
                .HasColumnName("titulo");

            entidad.Property(l => l.Autor)
                .HasColumnName("autor");

            entidad.Property(l => l.AnioPublicacion)
                .HasColumnName("anio_publicacion");

            entidad.Property(l => l.Editorial)
                .HasColumnName("editorial");

            entidad.Property(l => l.Paginas)
                .HasColumnName("paginas");

            entidad.Property(l => l.Categoria)
                .HasColumnName("categoria");

            entidad.Property(l => l.Isbn)
                .HasColumnName("isbn");

            entidad.Property(l => l.Activo)
                .HasColumnName("activo");
        });
    }

}