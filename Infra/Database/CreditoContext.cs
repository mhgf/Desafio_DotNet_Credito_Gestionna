using Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database;

public sealed class CreditoContext : DbContext
{
    public DbSet<Credito> Creditos { get; set; } = null!;

    public CreditoContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }
}