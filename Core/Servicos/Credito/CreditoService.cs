using Ardalis.Result;
using Core.Servicos.Dtos;
using Core.Servicos.ServiceBus;

namespace Core.Servicos.Credito;

public class CreditoService : ICreditoService
{
    private readonly ICreditoBusService _busService;

    public CreditoService(ICreditoBusService busService)
        => (_busService) = (busService);


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
}