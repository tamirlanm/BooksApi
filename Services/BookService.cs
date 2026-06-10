
using System.ComponentModel.DataAnnotations;
using BooksApi.Migrations;
using BooksApi.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class BookService : IBookService
{
    private readonly AppDbContext _db; 

    //private static readonly List<Book> _books = new();
    //private long _nextId = 1;
    public BookService(AppDbContext db) => _db = db;
    //private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1,1);
    //private readonly object _lock = new();
    //private readonly CreateBookValidator _validator = new CreateBookValidator();
    

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        //return _books.ToList();
        return await _db.Books.Include(b => b.Genre).ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByGenreAsync(int genreId)
    {
        return await _db.Books.Where(b => b.GenreId == genreId).Include(b => b.Genre).ToListAsync();
    }
    public async Task<IEnumerable<Book>> SearchAsync(string query)
    {
        var lowerSearch = query.ToLower();
        return await _db.Books.Where(b => b.Title.ToLower().Contains(lowerSearch) || b.Author.ToLower().Contains(lowerSearch)).
        Include(b => b.Genre).ToListAsync();
    }

    public async Task<Book> GetByIdAsync(long id)
    {
        var book = await _db.Books.Include(b => b.Genre).FirstOrDefaultAsync(b => b.Id == id);
        if(book is null) throw new NotFoundException($"Book with id={id} is not found.");
        return book;
    }

    public async Task<Book> CreateAsync(Book book)
    {
        
        var newBook = new Book
        {
            Title = book.Title,
            Author = book.Author,
            Year = book.Year,
            Price = book.Price,
            IsAvailable = book.IsAvailable,
            GenreId = book.GenreId
        };
        await _db.Books.AddAsync(newBook);
        await _db.SaveChangesAsync();
        return newBook;
        
    }

    public async Task<bool> UpdateAsync(long id, Book book)
    {
        
        var existingBook = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
        if(existingBook == null)
        {
            throw new NotFoundException($"Current book with Id={id} is not found.");
        }
        
        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.Year = book.Year;
        existingBook.Price = book.Price;
        existingBook.IsAvailable = book.IsAvailable;
        existingBook.GenreId = book.GenreId;
        await _db.SaveChangesAsync();
        return true;
        
    }
    public async Task<bool> DeleteAsync(long id)
    {
        
        var existingBook = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
        if(existingBook == null)
        {
            throw new NotFoundException($"Current book with Id={id} is not found.");
        }
        _db.Books.Remove(existingBook);
        await _db.SaveChangesAsync();
        return true;
        
    }

}