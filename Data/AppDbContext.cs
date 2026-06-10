
using BooksApi.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Genre> Genres => Set<Genre>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Required
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Author).IsRequired();
            entity.Property(e => e.Price).HasPrecision(10,2);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
        });
        #endregion
    }
}