using System.Linq.Expressions;
using Core.Entidades;

namespace Core.Repositorios;

public interface ICreditoRepository
{
    Task SalvarAsync(CancellationToken cancellationToken = default);

    Task AdicionarAsync(Credito credito, CancellationToken cancellationToken = default);

    Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<Credito, bool>> where,
        Expression<Func<Credito, TResult>> selector, CancellationToken cancellationToken = default);

    Task<TResult?> GetOneAsync<TResult>(Expression<Func<Credito, bool>> where,
        Expression<Func<Credito, TResult>> selector, CancellationToken cancellationToken = default);
}