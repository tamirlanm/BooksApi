using BooksApi.Models;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

public class GenreService : IGenreService
{
    private readonly AppDbContext _db;
    public GenreService(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
        return await _db.Genres.ToListAsync();
    }

    public async Task<Genre> CreateAsync(Genre genre)
    {
        var newGenre = new Genre
        {
            Name = genre.Name
        };
        await _db.Genres.AddAsync(genre);
        await _db.SaveChangesAsync();
        return genre;
    }
}
