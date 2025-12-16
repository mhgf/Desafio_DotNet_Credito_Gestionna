using System.Text;
using Core.Servicos.Credito;
using Microsoft.Azure.ServiceBus;
using Shared.Json;
using Shared.ServiceBus;
using Shared.ServiceBus.Dtos;

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
        => (async (message, token) => await InsereRegistroAsync(message, queueClient));

    private async Task InsereRegistroAsync(Message message, QueueClient queueClient)
    {
        Console.WriteLine("### PROCESSANDO MENSAGEM FILA ###");
        CreditoRequestDto creditoRequestDto;

        #region Deserializacao

        try
        {
            Console.WriteLine("### Deseralizando registro ###");

            var deserialize =
                CustomJsonSerializer.Deserialize<CreditoRequestDto>(Encoding.UTF8.GetString(message.Body));

            creditoRequestDto = deserialize ?? throw new NullReferenceException(nameof(CreditoRequestDto));
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("### Erro Deseralização registro ###");
            Console.WriteLine(e);
            Console.ResetColor();
            throw;
        }
        finally
        {
            Console.WriteLine("### Deseralização finalizada ###");
        }

        #endregion

        #region Exucução

        Console.WriteLine("### Inserindo no banco ###");
        try
        {
            using var scope = _serviceProvider.CreateScope();

            var creditoService = scope.ServiceProvider.GetRequiredService<ICreditoService>();

            var resultado = await creditoService.InsertCreditoAsync(creditoRequestDto);

            if (resultado.IsSuccess)
                await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("### Erro Inserção no banco ###");
            Console.WriteLine(e);
            Console.ResetColor();
            throw;
        }
        finally
        {
            Console.WriteLine("### Finalizando Inserção no banco ###");
        }

        #endregion
    }
}