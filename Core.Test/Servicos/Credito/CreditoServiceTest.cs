using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Core.Repositorios;
using Core.Servicos.Credito;
using Core.Servicos.Credito.Dtos;
using FluentAssertions;
using Moq;
using Shared.ServiceBus;
using Shared.ServiceBus.Dtos;
using Xunit;

namespace Core.Test.Servicos.Credito;

public class CreditoServiceTest
{
    private Mock<ICreditoRepository> _repositoryMock = null!;
    private Mock<ICreditoBusService> _creditoBusMock = null!;

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

    private readonly CreditoResponseDto responseValido = new(
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

    #region IntegraCreditoConstituidoAsync

    [Fact]
    [Trait("IntegraCreditoConstituidoAsync", "Sucesso")]
    public async Task IntegraCreditoConstituidoAsync_Deve_Retornar_Sucesso()
    {
        InitMocks();

        _creditoBusMock.Setup(x => x.EnviarCreditoConstituido(It.IsAny<List<CreditoRequestDto>>()))
            .ReturnsAsync(Result.Success());

        var servico = new CreditoService(_creditoBusMock.Object, _repositoryMock.Object);
        var resultado = await servico.IntegraCreditoConstituidoAsync(new List<CreditoRequestDto> { dtoValido });

        resultado.Should().NotBeNull();
        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Success.Should().BeTrue();
    }

    [Fact]
    [Trait("IntegraCreditoConstituidoAsync", "Falha_Bus")]
    public async Task IntegraCreditoConstituidoAsync_Deve_Falhar_No_Envio_Do_Bus()
    {
        InitMocks();
        var mensagem = "Teste";

        _creditoBusMock.Setup(x => x.EnviarCreditoConstituido(It.IsAny<List<CreditoRequestDto>>()))
            .ReturnsAsync(Result.Error(mensagem));

        var servico = new CreditoService(_creditoBusMock.Object, _repositoryMock.Object);
        var resultado = await servico.IntegraCreditoConstituidoAsync(new List<CreditoRequestDto> { dtoValido });

        resultado.Should().NotBeNull();
        resultado.IsSuccess.Should().BeFalse();
        resultado.Errors.Should().NotBeEmpty();
        resultado.Errors.Should().Satisfy(x => x == mensagem);
    }

    [Fact]
    [Trait("IntegraCreditoConstituidoAsync", "Falha_Validacao")]
    public async Task IntegraCreditoConstituidoAsync_Deve_Falhar_Na_Validacao()
    {
        InitMocks();
        var dto = dtoValido with { TipoCredito = "", ValorFaturado = 0 };
        var servico = new CreditoService(_creditoBusMock.Object, _repositoryMock.Object);
        var resultado = await servico.IntegraCreditoConstituidoAsync(new List<CreditoRequestDto> { dtoValido, dto });

        resultado.Should().NotBeNull();
        resultado.IsSuccess.Should().BeFalse();
        resultado.ValidationErrors.Should().NotBeEmpty();
        resultado.ValidationErrors.Should().HaveCountGreaterThan(1);
    }

    #endregion

    #region InsertCreditoAsync

    [Fact]
    [Trait("InsertCreditoAsync", "Sucesso")]
    public async Task InsertCreditoAsync_Deve_Retornar_Sucesso()
    {
        InitMocks();

        _repositoryMock.Setup(x => x.AdicionarAsync(It.IsAny<Entidades.Credito>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _repositoryMock.Setup(x => x.SalvarAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var servico = new CreditoService(_creditoBusMock.Object, _repositoryMock.Object);
        var resultado = await servico.InsertCreditoAsync(dtoValido);

        resultado.Should().NotBeNull();
        resultado.IsSuccess.Should().BeTrue();
    }

    [Fact]
    [Trait("InsertCreditoAsync", "Falha_Repositorio_Adicionar")]
    public async Task InsertCreditoAsync_Deve_Falhar_Ao_Adicionar()
    {
        InitMocks();

        var mensagem = "Erro ao inserir";

        _repositoryMock.Setup(x => x.AdicionarAsync(It.IsAny<Entidades.Credito>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(mensagem));
        _repositoryMock.Setup(x => x.SalvarAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var servico = new CreditoService(_creditoBusMock.Object, _repositoryMock.Object);
        var resultado = await servico.InsertCreditoAsync(dtoValido);

        resultado.Should().NotBeNull();
        resultado.IsSuccess.Should().BeFalse();
        resultado.Errors.Should().NotBeEmpty();
        resultado.Errors.Should().Satisfy(x => x == mensagem);
    }

    [Fact]
    [Trait("InsertCreditoAsync", "Falha_Repositorio_Salvar")]
    public async Task InsertCreditoAsync_Deve_Falhar_Ao_Salvar()
    {
        InitMocks();

        var mensagem = "Erro ao salvar";

        _repositoryMock.Setup(x => x.AdicionarAsync(It.IsAny<Entidades.Credito>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _repositoryMock.Setup(x => x.SalvarAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(mensagem));

        var servico = new CreditoService(_creditoBusMock.Object, _repositoryMock.Object);
        var resultado = await servico.InsertCreditoAsync(dtoValido);

        resultado.Should().NotBeNull();
        resultado.IsSuccess.Should().BeFalse();
        resultado.Errors.Should().NotBeEmpty();
        resultado.Errors.Should().Satisfy(x => x == mensagem);
    }

    [Fact]
    [Trait("InsertCreditoAsync", "Falha_Validacao")]
    public async Task InsertCreditoAsync_Deve_Falhar_Na_Validacao()
    {
        InitMocks();
        var dtoInvalido = dtoValido with { TipoCredito = "", ValorFaturado = 0 };
        var servico = new CreditoService(_creditoBusMock.Object, _repositoryMock.Object);
        var resultado = await servico.InsertCreditoAsync(dtoInvalido);


        resultado.Should().NotBeNull();
        resultado.IsSuccess.Should().BeFalse();
        resultado.ValidationErrors.Should().NotBeEmpty();
        resultado.ValidationErrors.Should().HaveCountGreaterThan(1);
    }

    #endregion

    #region GetCreditoByNfseAsync

    [Fact]
    [Trait("GetCreditoByNfseAsync", "Retorna_Lista")]
    public async Task GetCreditoByNfseAsync_Retorna_dados()
    {
        InitMocks();

        var lista = new List<CreditoResponseDto>()
        {
            responseValido,
            responseValido,
            responseValido,
        };

        _repositoryMock.Setup(x =>
                x.GetAllAsync(
                    It.IsAny<Expression<Func<Entidades.Credito, bool>>>(),
                    It.IsAny<Expression<Func<Entidades.Credito, CreditoResponseDto>>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(lista);


        var servico = new CreditoService(_creditoBusMock.Object, _repositoryMock.Object);
        var resultado = await servico.GetCreditoByNfseAsync("312312");


        resultado.Should().NotBeNull();
        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Should().BeEquivalentTo(lista);
    }

    [Fact]
    [Trait("GetCreditoByNfseAsync", "Retorna_Lista_Vazia")]
    public async Task GetCreditoByNfseAsync_Retorna_Lista_Vazia()
    {
        InitMocks();

        var lista = new List<CreditoResponseDto>();

        _repositoryMock.Setup(x =>
                x.GetAllAsync(
                    It.IsAny<Expression<Func<Entidades.Credito, bool>>>(),
                    It.IsAny<Expression<Func<Entidades.Credito, CreditoResponseDto>>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(lista);


        var servico = new CreditoService(_creditoBusMock.Object, _repositoryMock.Object);
        var resultado = await servico.GetCreditoByNfseAsync("312312");


        resultado.Should().NotBeNull();
        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Count().Should().Be(0);
        resultado.Value.Should().BeEquivalentTo(lista);
    }

    [Fact]
    [Trait("GetCreditoByNfseAsync", "Falha")]
    public async Task GetCreditoByNfseAsync_Deve_Falhar()
    {
        InitMocks();

        var mensagem = "Erro ao carregar dados";

        _repositoryMock.Setup(x =>
                x.GetAllAsync(
                    It.IsAny<Expression<Func<Entidades.Credito, bool>>>(),
                    It.IsAny<Expression<Func<Entidades.Credito, CreditoResponseDto>>>(),
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(mensagem));


        var servico = new CreditoService(_creditoBusMock.Object, _repositoryMock.Object);
        var resultado = await servico.GetCreditoByNfseAsync("312312");


        resultado.Should().NotBeNull();
        resultado.IsSuccess.Should().BeFalse();
        resultado.Errors.Should().NotBeEmpty();
        resultado.Errors.Should().Satisfy(x => x == mensagem);
    }

    #endregion


    #region GetCreditoByCreditoAsync

    [Fact]
    [Trait("GetCreditoByCreditoAsync", "Retorna_Dados_Credito")]
    public async Task GetCreditoByCreditoAsync_Retorna_dados()
    {
        InitMocks();

        _repositoryMock.Setup(x =>
                x.GetOneAsync(
                    It.IsAny<Expression<Func<Entidades.Credito, bool>>>(),
                    It.IsAny<Expression<Func<Entidades.Credito, CreditoResponseDto>>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseValido);


        var servico = new CreditoService(_creditoBusMock.Object, _repositoryMock.Object);
        var resultado = await servico.GetCreditoByCreditoAsync("312312");


        resultado.Should().NotBeNull();
        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Should().BeEquivalentTo(responseValido);
    }

    [Fact]
    [Trait("GetCreditoByCreditoAsync", "Retorna_NotFound")]
    public async Task GetCreditoByCreditoAsync_Retorna_NotFound()
    {
        InitMocks();


        _repositoryMock.Setup(x =>
                x.GetOneAsync(
                    It.IsAny<Expression<Func<Entidades.Credito, bool>>>(),
                    It.IsAny<Expression<Func<Entidades.Credito, CreditoResponseDto>>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((CreditoResponseDto?)null);

        var servico = new CreditoService(_creditoBusMock.Object, _repositoryMock.Object);
        var resultado = await servico.GetCreditoByCreditoAsync("312312");

        resultado.Should().NotBeNull();
        resultado.IsSuccess.Should().BeFalse();
        resultado.IsNotFound().Should().BeTrue();
    }

    [Fact]
    [Trait("GetCreditoByCreditoAsync", "Falha")]
    public async Task GetCreditoByCreditoAsync_Deve_Falhar()
    {
        InitMocks();

        var mensagem = "Erro ao carregar dados";

        _repositoryMock.Setup(x =>
                x.GetOneAsync(
                    It.IsAny<Expression<Func<Entidades.Credito, bool>>>(),
                    It.IsAny<Expression<Func<Entidades.Credito, CreditoResponseDto>>>(),
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(mensagem));


        var servico = new CreditoService(_creditoBusMock.Object, _repositoryMock.Object);
        var resultado = await servico.GetCreditoByCreditoAsync("312312");


        resultado.Should().NotBeNull();
        resultado.IsSuccess.Should().BeFalse();
        resultado.Errors.Should().NotBeEmpty();
        resultado.Errors.Should().Satisfy(x => x == mensagem);
    }

    #endregion

    private void InitMocks()
    {
        _repositoryMock = new Mock<ICreditoRepository>(MockBehavior.Strict);
        _creditoBusMock = new Mock<ICreditoBusService>(MockBehavior.Strict);
    }
}