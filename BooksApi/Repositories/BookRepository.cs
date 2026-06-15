using BooksApi.Repositories;
using BooksApi.Models;
using Microsoft.EntityFrameworkCore;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _db;
    public BookRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Book>> GetAllAsync()
        =>  await _db.Books.Include(b => b.Genre).ToListAsync();

    public async Task<Book?> GetByIdAsync(int id)
        => await _db.Books.Include(b => b.Genre).FirstOrDefaultAsync(b => b.Id == id);

    public async Task<IEnumerable<Book>> GetByGenreAsync(int genreId) 
        => await _db.Books.Where(b => b.GenreId == genreId).Include(b => b.Genre).ToListAsync();        

    public async Task<IEnumerable<Book>> SearchAsync(string query)
        => await _db.Books.Where(b => b.Title.Contains(query) || b.Author.Contains(query)).Include(b=> b.Genre).ToListAsync();

    public async Task AddAsync(Book entity) => await _db.Books.AddAsync(entity);

    public void Delete(Book entity) => _db.Books.Remove(entity);

    public void Update(Book entity) => _db.Books.Update(entity);

}