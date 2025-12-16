using Core.Entidades;

namespace Core.Repositorios;

public interface ICreditoRepository
{
    Task SalvarAsync(CancellationToken cancellationToken =  default);
    
    Task AdicionarAsync(Credito credito, CancellationToken cancellationToken = default);
}