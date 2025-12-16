using System;
using FluentAssertions;
using Shared.ServiceBus.Dtos;
using Xunit;

namespace Shared.Test.ServiceBus.Dto;

public class CreditoRequestTest
{
    private readonly CreditoRequestDtoValidator _validator = new();

    private readonly CreditoRequestDto dtoValido = new(
        "123456",
        "7891011",
        DateOnly.FromDayNumber(1),
        1500.75m,
        "ISSQN",
        true,
        5,
        30000,
        5000,
        25000);

    [Fact]
    public void Deve_Retornar_Valido()
    {
        var resultado = _validator.Validate(dtoValido);

        resultado.Should().NotBeNull();
        resultado.Errors.Should().BeEmpty();
        resultado.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Deve_Retornar_Invalido_TipoCredito()
    {
        var dto = dtoValido with { TipoCredito = string.Empty };

        var resultado = _validator.Validate(dto);

        resultado.Should().NotBeNull();
        resultado.Errors.Should().NotBeEmpty();
        resultado.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("123d456")]
    [InlineData("")]
    public void Deve_Retonar_Invalido_Numero_Credito(string numeroCredito)
    {
        var dto = dtoValido with { NumeroCredito = numeroCredito };

        var resultado = _validator.Validate(dto);

        resultado.Should().NotBeNull();
        resultado.Errors.Should().NotBeEmpty();
        resultado.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("789s1011")]
    [InlineData("")]
    public void Deve_Retonar_Invalido_Numero_Nfse(string numeroNfse)
    {
        var dto = dtoValido with { NumeroNfse = numeroNfse };

        var resultado = _validator.Validate(dto);

        resultado.Should().NotBeNull();
        resultado.Errors.Should().NotBeEmpty();
        resultado.IsValid.Should().BeFalse();
    }
    
    [Fact]
    public void Deve_Retornar_Invalido_ValorIssqn()
    {
        var dto = dtoValido with { ValorIssqn = 0};

        var resultado = _validator.Validate(dto);

        resultado.Should().NotBeNull();
        resultado.Errors.Should().NotBeEmpty();
        resultado.IsValid.Should().BeFalse();
    }
    
    [Fact]
    public void Deve_Retornar_Invalido_ValorDeducao()
    {
        var dto = dtoValido with { ValorDeducao = 0};

        var resultado = _validator.Validate(dto);

        resultado.Should().NotBeNull();
        resultado.Errors.Should().NotBeEmpty();
        resultado.IsValid.Should().BeFalse();
    }
    [Fact]
    public void Deve_Retornar_Invalido_ValorFaturado()
    {
        var dto = dtoValido with { ValorFaturado = 0};

        var resultado = _validator.Validate(dto);

        resultado.Should().NotBeNull();
        resultado.Errors.Should().NotBeEmpty();
        resultado.IsValid.Should().BeFalse();
    }
    [Fact]
    public void Deve_Retornar_Invalido_BaseCalculo()
    {
        var dto = dtoValido with { BaseCalculo = 0};

        var resultado = _validator.Validate(dto);

        resultado.Should().NotBeNull();
        resultado.Errors.Should().NotBeEmpty();
        resultado.IsValid.Should().BeFalse();
    }
    [Fact]
    public void Deve_Retornar_Invalido_Aliquota()
    {
        var dto = dtoValido with { Aliquota = 0};

        var resultado = _validator.Validate(dto);

        resultado.Should().NotBeNull();
        resultado.Errors.Should().NotBeEmpty();
        resultado.IsValid.Should().BeFalse();
    }
    
    
}