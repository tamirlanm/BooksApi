using BooksApi.Models;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<IEnumerable<Book>> GetByGenreAsync(int genreId);
    Task<IEnumerable<Book>> SearchAsync(string query);
    Task<Book> GetByIdAsync(long id);
    Task<Book> CreateAsync(Book book);
    Task<bool> UpdateAsync(long id, Book book);
    Task<bool> DeleteAsync(long id);
}
