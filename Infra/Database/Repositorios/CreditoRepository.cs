using System.Linq.Expressions;
using Core.Entidades;
using Core.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Repositorios;

public sealed class CreditoRepository : ICreditoRepository
{
    private readonly DbSet<Credito> _dbSet;
    private readonly CreditoContext _context;

    public CreditoRepository(CreditoContext context)
    {
        _context = context;
        _dbSet = context.Creditos;
    }

    public Task SalvarAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);

    public async Task AdicionarAsync(Credito credito, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(credito, cancellationToken);

    public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<Credito, bool>> where,
        Expression<Func<Credito, TResult>> selector, CancellationToken cancellationToken = default)
        => await _dbSet.Where(where).Select(selector).ToListAsync(cancellationToken);

    public Task<TResult?> GetOneAsync<TResult>(Expression<Func<Credito, bool>> where,
        Expression<Func<Credito, TResult>> selector, CancellationToken cancellationToken = default)
    => _dbSet.Where(where).Select(selector).FirstOrDefaultAsync(cancellationToken);
}