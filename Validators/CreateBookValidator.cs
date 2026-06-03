
using System.Data;
using FluentValidation;

public class CreateBookValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Название обязательно").MaximumLength(150);
        RuleFor(x => x.Author).NotEmpty().MinimumLength(4);
        RuleFor(x => x.Year).InclusiveBetween(800, DateTime.Now.Year);
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Цена должна быть не менее и больше чем 0");
    }
}