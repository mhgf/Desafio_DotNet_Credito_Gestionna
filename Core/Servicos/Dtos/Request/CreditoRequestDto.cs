using System.Data;
using FluentValidation;

namespace Core.Servicos.Dtos;

public record CreditoRequestDto(
    string NumeroCredito,
    string NumeroNfse,
    DateOnly DataConstituicao,
    decimal ValorIssqn,
    string TipoCredito,
    bool SimplesNacional,
    decimal Aliquata,
    decimal ValorFaturado,
    decimal ValorDecucao,
    decimal ValorCalculo);

public class CreditoRequestDtoValidator : AbstractValidator<CreditoRequestDto>
{
    public CreditoRequestDtoValidator()
    {
        RuleFor(x => x.NumeroCredito)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
            .Matches(@"^\d+$")
            .WithMessage("'{PropertyName}' precisa ser somente numeros.");
        RuleFor(x => x.NumeroNfse)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
            .Matches(@"^\d+$")
            .WithMessage("'{PropertyName}' precisa ser somente numeros.");

        RuleFor(x => x.ValorIssqn)
            .GreaterThan(0).WithMessage("O campo {PropertyName} não pode ser negativo ou zero");
        RuleFor(x => x.ValorFaturado)
            .GreaterThan(0).WithMessage("O campo {PropertyName} não pode ser negativo ou zero");
        RuleFor(x => x.ValorDecucao)
            .GreaterThan(0).WithMessage("O campo {PropertyName} não pode ser negativo ou zero");
        RuleFor(x => x.ValorCalculo)
            .GreaterThan(0).WithMessage("O campo {PropertyName} não pode ser negativo ou zero");
        RuleFor(x => x.Aliquata)
            .GreaterThan(0).WithMessage("O campo {PropertyName} não pode ser negativo ou zero");

        RuleFor(x => x.TipoCredito)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
    }
}