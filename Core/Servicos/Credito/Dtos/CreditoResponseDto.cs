namespace Core.Servicos.Credito.Dtos;

public sealed record CreditoResponseDto(
    string NumeroCredito,
    string NumeroNfse,
    DateOnly DataConstituicao,
    decimal ValorIssqn,
    string TipoCredito,
    bool SimplesNacional,
    decimal Aliquota,
    decimal ValorFaturado,
    decimal ValorDeducao,
    decimal BaseCalculo);