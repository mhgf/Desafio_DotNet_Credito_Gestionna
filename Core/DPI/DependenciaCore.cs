using Core.Servicos.Credito;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DPI;

public static class DependenciaCore
{
    public static void AddDependicaCore(this IServiceCollection services)
    {
        services.AddScoped<ICreditoService, CreditoService>();
    }
}