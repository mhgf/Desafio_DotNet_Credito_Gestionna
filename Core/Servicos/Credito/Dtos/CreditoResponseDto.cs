using System.Text.Json.Serialization;
using Shared.Json.Converters;

namespace Core.Servicos.Credito.Dtos;

public sealed record CreditoResponseDto(
    string NumeroCredito,
    string NumeroNfse,
    DateOnly DataConstituicao,
    decimal ValorIssqn,
    string TipoCredito,
    [property: JsonConverter(typeof(BoolJsonConverter))]  bool SimplesNacional,
    decimal Aliquota,
    decimal ValorFaturado,
    decimal ValorDeducao,
    decimal BaseCalculo);