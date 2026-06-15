
using System.ComponentModel.DataAnnotations;
using BooksApi.Migrations;
using BooksApi.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using BooksApi.DTOs;
namespace BooksApi.Services;
public class BookService : IBookService
{
    private readonly IUnitOfWork _uow;
    private readonly IValidator<CreateBookRequest> _validator;

    //private readonly AppDbContext _db; 
    //private static readonly List<Book> _books = new();
    //private long _nextId = 1;
    //private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1,1);
    //private readonly object _lock = new();
    //private readonly CreateBookValidator _validator = new CreateBookValidator();
    
    public BookService(IUnitOfWork uow, IValidator<CreateBookRequest> validator) {
        _uow = uow;
        _validator = validator;
    }

    public async Task<IEnumerable<BookResponse>> GetAllBooksAsync()
    {
        //return _books.ToList();
        var books = await _uow.Books.GetAllAsync();
        return books.Select(book => book.ToResponse());
        /*
        return books.Select(book => new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Year = book.Year,
            Price = book.Price,
            IsAvailable = book.IsAvailable,
            GenreName = book.Genre?.Name ?? "Without Genre"
        });*/
    }

    public async Task<IEnumerable<BookResponse>> GetBookByGenreAsync(int genreId)
    {
        var books = await _uow.Books.GetByGenreAsync(genreId);
        return books.Select(book => book.ToResponse());
    }
    public async Task<IEnumerable<BookResponse>> SearchBookAsync(string query)
    {
        var books = await _uow.Books.SearchAsync(query);
        return books.Select(book => book.ToResponse());
    }

    public async Task<BookResponse> GetBookByIdAsync(int id)
    {
        var book = await _uow.Books.GetByIdAsync(id);
        if(book is null) throw new NotFoundException($"Book with id={id} is not found.");
        return new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Year = book.Year,
            Price = book.Price,
            IsAvailable = book.IsAvailable,
            GenreName = book.Genre?.Name ?? "Without Genre"
        };
    }

    public async Task<BookResponse> CreateBookAsync(CreateBookRequest request)
    {        
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new BadRequestException($"Error validation: {errors}");
        }

        var genre = await _uow.Genres.GetByIdAsync(request.GenreId);
        if(genre is null) throw new NotFoundException($"Genre with Id={request.GenreId} not found");
        
        var newBook = new Book
        {
            Title = request.Title,
            Author = request.Author,
            Year = request.Year,
            Price = request.Price,
            IsAvailable = request.IsAvailable,
            GenreId = request.GenreId
        };
        await _uow.Books.AddAsync(newBook);
        await _uow.SaveChangesAsync();
        return new BookResponse
        {
            Id = newBook.Id,
            Title = newBook.Title,
            Author = newBook.Author,
            Year = newBook.Year,
            Price = newBook.Price,
            IsAvailable = newBook.IsAvailable,
            //GenreId = newBook.GenreId,
            GenreName = genre!.Name
        };   
    }

    public async Task<bool> UpdateBookAsync(int id, CreateBookRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new BadRequestException($"Error validation: {errors}");
        }
        
        var existingBook = await _uow.Books.GetByIdAsync(id);
        if(existingBook == null)
        {
            throw new NotFoundException($"Current book with Id={id} is not found.");
        }
        
        existingBook.Title = request.Title;
        existingBook.Author = request.Author;
        existingBook.Year = request.Year;
        existingBook.Price = request.Price;
        existingBook.IsAvailable = request.IsAvailable;
        existingBook.GenreId = request.GenreId;
        _uow.Books.Update(existingBook);
        await _uow.SaveChangesAsync();
        return true;
        
    }
    public async Task<bool> DeleteBookAsync(int id)
    {
        
        var existingBook = await _uow.Books.GetByIdAsync(id);
        if(existingBook == null)
        {
            throw new NotFoundException($"Current book with Id={id} is not found.");
        }
        _uow.Books.Delete(existingBook);
        await _uow.SaveChangesAsync();
        return true;
    }
}

public static class BookMappingExtensions
{
    public static BookResponse ToResponse(this Book book) => new BookResponse
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        Year = book.Year,
        Price = book.Price,
        IsAvailable = book.IsAvailable,
        GenreName = book.Genre?.Name ?? "Without Genre"
    };
}