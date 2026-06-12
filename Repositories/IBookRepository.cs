using BooksApi.Models;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> GetByGenreAsync(int genreId);
    Task<IEnumerable<Book>> SearchAsync(string query);
}