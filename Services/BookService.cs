
using System.ComponentModel.DataAnnotations;
using BooksApi.Models;
using FluentValidation;

public class BookService : IBookService
{
    private static readonly List<Book> _books = new();
    private long _nextId = 1;
    //private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1,1);
    //private readonly object _lock = new();
    //private readonly CreateBookValidator _validator = new CreateBookValidator();
    

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return _books.ToList();
    }

    public async Task<Book> GetByIdAsync(long id)
    {
        
        var book = _books.FirstOrDefault(b => b.Id == id);
        if(book is null) throw new NotFoundException($"Book with id={id} is not found.");
        return book;
    }

    public async Task<Book> CreateAsync(Book book)
    {
        
        var newBook = new Book
        {
            Id = _nextId++,
            Title = book.Title,
            Author = book.Author,
            Year = book.Year,
            Price = book.Price
        };
        _books.Add(newBook);
        return newBook;
        
    }

    public async Task<bool> UpdateAsync(long id, Book book)
    {
        
        var existingBook = _books.FirstOrDefault(b => b.Id == id);
        if(existingBook == null)
        {
            throw new NotFoundException($"Current book with Id={id} is not found.");
        }
        
        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.Year = book.Year;
        existingBook.Price = book.Price;

        return true;
        
    }
    public async Task<bool> DeleteAsync(long id)
    {
        
        var existingBook = _books.FirstOrDefault(b => b.Id == id);
        if(existingBook == null)
        {
            throw new NotFoundException($"Current book with Id={id} is not found.");
        }
        _books.Remove(existingBook);
        return true;
        
    }
}