using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data;

public class LibraryContext : DbContext
{
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=library.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ========== Конфигурация Author ==========
        modelBuilder.Entity<Author>(entity =>
        {
            // Первичный ключ
            entity.HasKey(a => a.Id);

            // Обязательные поля с ограничениями длины
            entity.Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(100);

            // Игнорируем вычисляемое свойство
            entity.Ignore(a => a.FullName);
        });

        // ========== Конфигурация Genre ==========
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(g => g.Id);

            entity.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(g => g.Description)
                .HasMaxLength(500);
        });

        // ========== Конфигурация Book ==========
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);

            entity.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(b => b.ISBN)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(b => b.PublishYear)
                .IsRequired();

            entity.Property(b => b.QuantityInStock)
                .IsRequired()
                .HasDefaultValue(0);

            // Связь один-ко-многим: Author -> Books (каскадное удаление)
            entity.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один-ко-многим: Genre -> Books (каскадное удаление)
            entity.HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
