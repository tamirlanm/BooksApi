
using System.ComponentModel.DataAnnotations;
using BooksApi.Models;
using FluentValidation;

public class BookService : IBooKService
{
    private readonly List<Book> _books = new();
    private long _nextId = 1;
    private readonly object _lock = new();
    //private readonly CreateBookValidator _validator = new CreateBookValidator();
    private readonly IValidator<Book> _validator;
    public BookService(IValidator<Book> validator)
    {
        _validator = validator;
    }

    public IEnumerable<Book> GetAll()
    {
        lock (_lock)
        {   
            return _books.ToList();
        }
    }

    public Book GetById(long id)
    {
        lock(_lock)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if(book is null) throw new NotFoundException($"Book with id={id} is not found.");
            return book;
        }
    }

    public Book Create(Book book)
    {
        var validationResult = _validator.Validate(book);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new BadRequestException($"Error validation: {errors}");
        }
        lock (_lock)
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
    }

    public bool Update(long id, Book book)
    {
        lock (_lock)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == id);
            if(existingBook == null)
            {
                throw new NotFoundException($"Current {existingBook} book is not found");
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Year = book.Year;
            existingBook.Price = book.Price;

            return true;
        }
    }
    public bool Delete(long id)
    {
        lock (_lock)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == id);
            if(existingBook == null)
            {
                throw new NotFoundException($"Current {existingBook} book is not found.");
            }
            _books.Remove(existingBook);
            return true;
        }
    }
}