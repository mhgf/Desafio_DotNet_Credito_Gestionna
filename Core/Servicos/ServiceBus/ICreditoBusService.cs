using Ardalis.Result;
using Core.Servicos.Dtos;

namespace Core.Servicos.ServiceBus;

public interface ICreditoBusService
{
    Task<Result> EnviarCreditoConstituido(IEnumerable<CreditoRequestDto> creditos);
}