using Core.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Database.Maps;

public sealed class CreditoMap : IEntityTypeConfiguration<Credito>
{
    public void Configure(EntityTypeBuilder<Credito> builder)
    {
        builder.ToTable("credito");
        
        builder
            .HasKey(c => c.Id);
        
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.NumeroCredito)
            .HasColumnName("numero_credito")
            .HasColumnType("VARCHAR(50)")
            .IsRequired();
        builder.Property(x => x.NumeroNfse)
            .HasColumnName("numero_nfse")
            .HasColumnType("VARCHAR(50)")
            .IsRequired();
        builder.Property(x => x.DataConstituicao)
            .HasColumnName("data_constituicao")
            .HasColumnType("DATE")
            .IsRequired();
        builder.Property(x => x.ValorIssqn)
            .HasColumnName("valor_issqn")
            .HasColumnType("DECIMAL(15, 2)")
            .IsRequired();
        builder.Property(x => x.TipoCredito)
            .HasColumnName("tipo_credito")
            .HasColumnType("VARCHAR(50)")
            .IsRequired();
        builder.Property(x => x.NumeroCredito)
            .HasColumnName("numero_credito")
            .HasColumnType("VARCHAR(50)")
            .IsRequired();
        builder.Property(x => x.SimplesNacional)
            .HasColumnName("simples_nacional")
            .HasColumnType("BOOLEAN")
            .IsRequired();
        builder.Property(x => x.NumeroCredito)
            .HasColumnName("numero_credito")
            .HasColumnType("VARCHAR(50)")
            .IsRequired();
        builder.Property(x => x.NumeroCredito)
            .HasColumnName("numero_credito")
            .HasColumnType("VARCHAR(50)")
            .IsRequired();
        
        builder.Property(x => x.Aliquota)
            .HasColumnName("aliquota")
            .HasColumnType("DECIMAL(5, 2)")
            .IsRequired();
        builder.Property(x => x.ValorFaturado)
            .HasColumnName("valor_faturado")
            .HasColumnType("DECIMAL(15, 2)")
            .IsRequired();
        builder.Property(x => x.ValorDeducao)
            .HasColumnName("valor_deducao")
            .HasColumnType("DECIMAL(15, 2)")
            .IsRequired();
        builder.Property(x => x.BaseCalculo)
            .HasColumnName("base_calculo")
            .HasColumnType("DECIMAL(15, 2)")
            .IsRequired();
    }
}