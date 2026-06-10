
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class CreateBookValidator : AbstractValidator<CreateBookRequest>
{
    private readonly AppDbContext _db;
    public CreateBookValidator(AppDbContext db)
    {
        _db = db;
        RuleFor(x => x.Title).NotEmpty().WithMessage("Название обязательно").MaximumLength(150);
        RuleFor(x => x.Author).NotEmpty().MinimumLength(4);
        RuleFor(x => x.Year).InclusiveBetween(800, DateTime.Now.Year);
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Цена должна быть не менее и больше чем 0");
        RuleFor(x => x.GenreId).GreaterThan(0).WithMessage("Некорректный ID жанра").MustAsync(GenreMustExist).WithMessage("Указанного жанра не существует в базе данных.");
    }
    private async Task<bool> GenreMustExist(int genreId, CancellationToken cancellationToken)
    {
        return await _db.Genres.AnyAsync(g => g.Id == genreId, cancellationToken);
    }
}