namespace EventFlow.Application.Validators;

public class SpeakerCommandValidator : AbstractValidator<SpeakerCommand>
{
    public SpeakerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do palestrante é obrigatório.")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail do palestrante é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado não é válido.")
            .MaximumLength(150).WithMessage("O e-mail deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Biography)
            .NotEmpty().WithMessage("A biografia do palestrante é obrigatória.")
            .MaximumLength(2000).WithMessage("A biografia deve ter no máximo 2000 caracteres.");
    }
}
