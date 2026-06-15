using BooksApi.Models;
using BooksApi.Repositories;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> GetByGenreAsync(int genreId);
    Task<IEnumerable<Book>> SearchAsync(string query);
}