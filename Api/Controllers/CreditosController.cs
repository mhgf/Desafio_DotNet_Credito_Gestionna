using Core.Servicos.Credito;
using Microsoft.AspNetCore.Mvc;
using Ardalis.Result.AspNetCore;
using Core.Servicos.Credito.Dtos;
using Shared.ServiceBus.Dtos;

namespace DesafioGestionna.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditosController : ControllerBase
{
    private readonly ICreditoService _creditoService;

    public CreditosController(ICreditoService creditoService)
        => _creditoService = creditoService;

    [HttpGet("{numeroNfse}")]
    public async Task<ActionResult<IEnumerable<CreditoRequestDto>>> GetByNfse(string numeroNfse,
        CancellationToken cancellationToken = default)
    {
        var resultado = await _creditoService.GetCreditoByNfseAsync(numeroNfse, cancellationToken);
        return resultado.ToActionResult(this);
    }

    [HttpGet("credito/{numeroCredito}")]
    public async Task<ActionResult<CreditoRequestDto>> GetByCredito(string numeroCredito,
        CancellationToken cancellationToken = default)
    {
        var resultado = await _creditoService.GetCreditoByCreditoAsync(numeroCredito, cancellationToken);
        return resultado.ToActionResult(this);
    }

    [HttpPost("integrar-credito-constituido")]
    public async Task<ActionResult<SimplesResponseDto>> IntegrarCreditoConstituidoAsync(
        [FromBody] List<CreditoRequestDto> creditoRequestDtos)
    {
        var resultado = await _creditoService.IntegraCreditoConstituidoAsync(creditoRequestDtos);
        return resultado.ToActionResult(this);
    }
}