using System.ComponentModel.DataAnnotations;
using BooksApi.DTOs;
using BooksApi.Models;
using FluentValidation;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

public class GenreService : IGenreService
{
    //private readonly AppDbContext _db;
    //public GenreService(AppDbContext db) => _db = db;
    private readonly IUnitOfWork _uow;
    private readonly IValidator<CreateGenreRequest> _validator;

    public GenreService(IUnitOfWork uow, IValidator<CreateGenreRequest> validator){
        _uow = uow;
        _validator = validator;
    }


    public async Task<IEnumerable<GenreResponse>> GetAllGenresAsync()
    {
        var genres = await _uow.Genres.GetAllAsync();
        return genres.Select(genres => new GenreResponse
        {
            Id = genres.Id,
            Name = genres.Name
        });
    }

    public async Task<GenreResponse> CreateGenreAsync(CreateGenreRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new BadRequestException($"Error validation: {errors}");
        }
        var newGenre = new Genre
        {
            Name = request.Name
        };
        await _uow.Genres.AddAsync(newGenre);
        await _uow.SaveChangesAsync();
        return new GenreResponse
        {
            Id = newGenre.Id,
            Name = newGenre.Name
        };
    }
}
