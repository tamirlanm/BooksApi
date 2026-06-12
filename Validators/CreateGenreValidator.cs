using System.Data;
using BooksApi.DTOs;
using FluentValidation;

public class CreateGenreValidator : AbstractValidator<CreateGenreRequest>
{
    private readonly AppDbContext _db;
    public CreateGenreValidator(AppDbContext db)
    {
        _db = db;
        RuleFor(x => x.Name).NotEmpty().WithMessage("Название жанра обязательно").MinimumLength(2).WithMessage("Название жанра должно быть обязательно не менее 2 символа.");
    }
}