using BooksApi.Models;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book> GetByIdAsync(long id);
    Task<Book> CreateAsync(Book book);
    Task<bool> UpdateAsync(long id, Book book);
    Task<bool> DeleteAsync(long id);
}
