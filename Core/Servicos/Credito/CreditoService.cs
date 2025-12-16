using Ardalis.Result;
using Core.Repositorios;
using Shared.ServiceBus;
using Shared.ServiceBus.Dtos;

namespace Core.Servicos.Credito;

public class CreditoService : ICreditoService
{
    private readonly ICreditoBusService _busService;
    private readonly ICreditoRepository _creditoRepository;

    public CreditoService(ICreditoBusService busService, ICreditoRepository creditoRepository)
        => (_busService, _creditoRepository) = (busService, creditoRepository);


    public async Task<Result> IntegraCreditoConstituidoAsync(IList<CreditoRequestDto> creditoRequestDtos)
    {
        var validador = new CreditoRequestDtoValidator();
        foreach (var requestDto in creditoRequestDtos)
        {
            var validacao = await validador.ValidateAsync(requestDto);
            if (!validacao.IsValid)
                return Result.Invalid(validacao.Errors.Select(x => new ValidationError(x.ErrorMessage)));
        }

        return await _busService.EnviarCreditoConstituido(creditoRequestDtos);
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
}