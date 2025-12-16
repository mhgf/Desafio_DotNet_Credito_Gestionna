using System.Text.Json.Serialization;
using FluentValidation;
using Shared.Json.Converters;

namespace Shared.ServiceBus.Dtos;

public record CreditoRequestDto(
    string NumeroCredito,
    string NumeroNfse,
    DateOnly DataConstituicao,
    decimal ValorIssqn,
    string TipoCredito,
    [property: JsonConverter(typeof(BoolJsonConverter))] bool SimplesNacional,
    decimal Aliquota,
    decimal ValorFaturado,
    decimal ValorDeducao,
    decimal BaseCalculo);

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
        RuleFor(x => x.ValorDeducao)
            .GreaterThan(0).WithMessage("O campo {PropertyName} não pode ser negativo ou zero");
        RuleFor(x => x.BaseCalculo)
            .GreaterThan(0).WithMessage("O campo {PropertyName} não pode ser negativo ou zero");
        RuleFor(x => x.Aliquota)
            .GreaterThan(0).WithMessage("O campo {PropertyName} não pode ser negativo ou zero");

        RuleFor(x => x.TipoCredito)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
    }
}