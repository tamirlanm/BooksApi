using BooksApi.DTOs;
using BooksApi.Models;
public interface IGenreService
{
    Task<IEnumerable<GenreResponse>> GetAllGenresAsync();
    Task<GenreResponse> CreateGenreAsync(CreateGenreRequest request);
}