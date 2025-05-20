using EventFlow_API.Commands;

namespace EventFlow_API.Validators;
public class EventCommandValidator : AbstractValidator<EventCommand>
{
    public EventCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O título é obrigatório.")
            .MaximumLength(200).WithMessage("O título deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(8000).WithMessage("A descrição deve ter no máximo 8000 caracteres.");

        RuleFor(x => x.Date)
            .GreaterThan(DateTime.Now).WithMessage("A data deve ser no futuro.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("O local é obrigatório.");

        RuleFor(x => x.OrganizerId)
            .GreaterThan(0).WithMessage("OrganizerId inválido.");
    }
}
