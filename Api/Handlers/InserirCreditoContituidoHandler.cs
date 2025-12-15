using System.Text;
using Microsoft.Azure.ServiceBus;
using Shared.ServiceBus;

namespace DesafioGestionna.Api.Handlers;

public class InserirCreditoContituidoHandler : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public InserirCreditoContituidoHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var bus = _serviceProvider.GetRequiredService<ICreditoBusService>();
        await bus.RegistrarHandler(Executar);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    private Func<Message, CancellationToken, Task> Executar(QueueClient queueClient)
        => (async (message, token) =>
        {
            Console.WriteLine("### PROCESSANDO MENSAGEM FILA ###");
            Console.WriteLine($"{DateTime.Now}");
            Console.WriteLine(
                $"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        });

    
}