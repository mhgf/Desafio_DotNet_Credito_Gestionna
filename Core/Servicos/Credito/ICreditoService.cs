using Ardalis.Result;
using Shared.ServiceBus.Dtos;

namespace Core.Servicos.Credito;

public interface ICreditoService
{
    Task<Result> IntegraCreditoConstituidoAsync(IList<CreditoRequestDto> creditoRequestDtos);
}