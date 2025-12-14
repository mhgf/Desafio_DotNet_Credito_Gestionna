namespace DesafioGestionna.Entidades;

public class Credito
{
    public long Id { get; private set; }
    public string NumeroCredito { get; private set; }
    public string NumeroNfse { get; private set; }
    public DateOnly DataConstituicao { get; private set; }
    public decimal ValorIssqn { get; private set; }
    public string TipoCredito { get; private set; }
    public bool SimplesNacional { get; private set; }
    public decimal Aliquata { get; private set; }
    public decimal ValorFaturado { get; private set; }
    public decimal ValorDeducao { get; private set; }
    public decimal BaseCalculo { get; private set; }
}