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
}