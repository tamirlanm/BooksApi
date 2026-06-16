using BooksApi.DTOs;
using BooksApi.Models;

public interface IBookService
{
    Task<IEnumerable<BookResponse>> GetAllBooksAsync();
    Task<IEnumerable<BookResponse>> GetBookByGenreAsync(int genreId);
    Task<IEnumerable<BookResponse>> SearchBookAsync(string query);
    Task<BookResponse> GetBookByIdAsync(int id);
    Task<BookResponse> CreateBookAsync(CreateBookRequest request);
    Task<BookResponse> UpdateBookAsync(int id, CreateBookRequest request);
    Task<BookResponse> DeleteBookAsync(int id);
}
