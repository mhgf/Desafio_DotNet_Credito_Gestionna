using Ardalis.Result;
using Core.Repositorios;
using Core.Servicos.Credito.Dtos;
using Shared.ServiceBus;
using Shared.ServiceBus.Dtos;

namespace Core.Servicos.Credito;

public class CreditoService : ICreditoService
{
    private readonly ICreditoBusService _busService;
    private readonly ICreditoRepository _creditoRepository;

    public CreditoService(ICreditoBusService busService, ICreditoRepository creditoRepository)
        => (_busService, _creditoRepository) = (busService, creditoRepository);


    public async Task<Result<SimplesResponseDto>> IntegraCreditoConstituidoAsync(
        IList<CreditoRequestDto> creditoRequestDtos)
    {
        var validador = new CreditoRequestDtoValidator();
        foreach (var requestDto in creditoRequestDtos)
        {
            var validacao = await validador.ValidateAsync(requestDto);
            if (!validacao.IsValid)
                return Result.Invalid(validacao.Errors.Select(x => new ValidationError(x.ErrorMessage)));
        }

        var resultado = await _busService.EnviarCreditoConstituido(creditoRequestDtos);

        if (resultado.IsSuccess) return Result.Success(new SimplesResponseDto(true));
        return resultado;
    }

    public async Task<Result> InsertCreditoAsync(CreditoRequestDto creditoRequestDto)
    {
        var validacao = await new CreditoRequestDtoValidator().ValidateAsync(creditoRequestDto);
        if (!validacao.IsValid)
            return Result.Invalid(validacao.Errors.Select(x => new ValidationError(x.ErrorMessage)));

        try
        {
            var credito = Entidades.Credito.Create(
                creditoRequestDto.NumeroCredito,
                creditoRequestDto.NumeroNfse,
                creditoRequestDto.DataConstituicao,
                creditoRequestDto.ValorIssqn,
                creditoRequestDto.TipoCredito,
                creditoRequestDto.SimplesNacional,
                creditoRequestDto.Aliquota,
                creditoRequestDto.ValorFaturado,
                creditoRequestDto.ValorDeducao,
                creditoRequestDto.BaseCalculo);

            await _creditoRepository.AdicionarAsync(credito);
            await _creditoRepository.SalvarAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }

        return Result.Success();
    }

    public async Task<Result<IEnumerable<CreditoResponseDto>>> GetCreditoByNfseAsync(string numeroNfse,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var lista = await _creditoRepository.GetAllAsync(
                (credito => credito.NumeroNfse == numeroNfse),
                credito => new CreditoResponseDto(
                    credito.NumeroCredito,
                    credito.NumeroNfse,
                    credito.DataConstituicao,
                    credito.ValorIssqn,
                    credito.TipoCredito,
                    credito.SimplesNacional,
                    credito.Aliquota,
                    credito.ValorFaturado,
                    credito.ValorDeducao,
                    credito.BaseCalculo));

            return Result.Success(lista);
        }
        catch (Exception e)
        {
            return Result.Error(e.Message);
        }
    }

    public async Task<Result<CreditoResponseDto>> GetCreditoByCreditoAsync(string numeroCredito,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var credito = await _creditoRepository.GetOneAsync(
                credito => credito.NumeroCredito == numeroCredito,
                credito => new CreditoResponseDto(
                    credito.NumeroCredito,
                    credito.NumeroNfse,
                    credito.DataConstituicao,
                    credito.ValorIssqn,
                    credito.TipoCredito,
                    credito.SimplesNacional,
                    credito.Aliquota,
                    credito.ValorFaturado,
                    credito.ValorDeducao,
                    credito.BaseCalculo));

            return credito is null ? Result.NotFound(numeroCredito) : Result.Success(credito);
        }
        catch (Exception e)
        {
            return Result.Error(e.Message);
        }
    }
}