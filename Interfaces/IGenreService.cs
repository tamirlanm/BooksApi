using BooksApi.Models;
public interface IGenreService
{
    Task<IEnumerable<Genre>> GetAllAsync();
    Task<Genre> CreateAsync(Genre genre);
}