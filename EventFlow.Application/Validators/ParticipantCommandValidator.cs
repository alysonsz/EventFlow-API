﻿namespace EventFlow.Application.Validators;

public class ParticipantCommandValidator : AbstractValidator<ParticipantCommand>
{
    public ParticipantCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do participante é obrigatório.")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail do participante é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado não é válido.")
            .MaximumLength(150).WithMessage("O e-mail deve ter no máximo 150 caracteres.");
    }
}
