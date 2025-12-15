using System.Text;
using System.Text.Json;
using Ardalis.Result;
using Azure.Messaging.ServiceBus.Administration;
using Core.Servicos.Dtos;
using Core.Servicos.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using Shared.Config;
using Shared.Json;

namespace Infra.Servicos.ServiceBus;

public sealed class CreditoBusService : ICreditoBusService, IDisposable, IAsyncDisposable
{
    private readonly QueueClient _queueClient;

    public CreditoBusService(IOptions<ServiceBusConfig> serviceBusConfig)
    {
        InitQueueAsync(serviceBusConfig.Value).GetAwaiter().GetResult();
        _queueClient = new QueueClient(serviceBusConfig.Value.ConnectionString,
            serviceBusConfig.Value.QueueName);
    }

    private async Task InitQueueAsync(ServiceBusConfig serviceBusConfig)
    {
        var adminClient = new ServiceBusAdministrationClient(serviceBusConfig.ConnectionString);
        if (!await adminClient.QueueExistsAsync(serviceBusConfig.QueueName).ConfigureAwait(false))
            await adminClient.CreateQueueAsync(serviceBusConfig.QueueName).ConfigureAwait(false);
    }

    public async Task<Result> EnviarCreditoConstituido(IEnumerable<CreditoRequestDto> creditos)
    {
        try
        {
            foreach (var credito in creditos)
            {
                var msgSerializada = CustomJsonSerializer.Serialize(credito);
                var mensagem = new Message(Encoding.UTF8.GetBytes(msgSerializada));
                await _queueClient.SendAsync(mensagem);
            }
        }
        catch (Exception e)
        {
            return Result.Error(e.Message);
        }

        return Result.Success();
    }

    public void Dispose() => DisposeAsync().GetAwaiter().GetResult();

    public async ValueTask DisposeAsync() => await _queueClient.CloseAsync().ConfigureAwait(false);
}