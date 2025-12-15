using Ardalis.Result;
using Microsoft.Azure.ServiceBus;
using Shared.ServiceBus.Dtos;

namespace Shared.ServiceBus;

public interface ICreditoBusService
{
    Task<Result> EnviarCreditoConstituido(IEnumerable<CreditoRequestDto> creditos);

    Task RegistrarHandler(Func<QueueClient, Func<Message, CancellationToken, Task>> handler);
}