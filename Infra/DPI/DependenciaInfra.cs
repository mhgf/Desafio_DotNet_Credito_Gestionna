using Infra.Servicos.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Shared.ServiceBus;

namespace Infra.DPI;

public static class DependenciaInfra
{
    public static void AddDependenciaInfra(this IServiceCollection services)
    {
        services.AddSingleton<ICreditoBusService, CreditoBusService>();
    }
}