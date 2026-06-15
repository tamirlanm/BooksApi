
using BooksApi.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class CreateBookValidator : AbstractValidator<CreateBookRequest>
{
    private readonly IUnitOfWork _uow;
    public CreateBookValidator(IUnitOfWork uow)
    {
        _uow = uow;
        RuleFor(x => x.Title).NotEmpty().WithMessage("Название обязательно").MaximumLength(150);
        RuleFor(x => x.Author).NotEmpty().MinimumLength(4);
        RuleFor(x => x.Year).InclusiveBetween(800, DateTime.Now.Year).WithMessage($"Год должен быть в диапазоне от 800 до {DateTime.Now.Year}");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Цена должна быть не менее и больше чем 0");
        RuleFor(x => x.GenreId).GreaterThan(0).WithMessage("Некорректный ID жанра").
        MustAsync(GenreMustExist).WithMessage("Указанного жанра не существует в базе данных.");
    }
    private async Task<bool> GenreMustExist(int genreId, CancellationToken cancellationToken)
    {
        var genre =await _uow.Genres.GetByIdAsync(genreId);
        return genre != null;
    }
}