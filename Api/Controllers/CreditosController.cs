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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CreditoResponseDto>))]
    public async Task<ActionResult<IEnumerable<CreditoResponseDto>>> GetByNfse(string numeroNfse,
        CancellationToken cancellationToken = default)
    {
        var resultado = await _creditoService.GetCreditoByNfseAsync(numeroNfse, cancellationToken);
        return resultado.ToActionResult(this);
    }

    [HttpGet("credito/{numeroCredito}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreditoResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreditoResponseDto>> GetByCredito(string numeroCredito,
        CancellationToken cancellationToken = default)
    {
        var resultado = await _creditoService.GetCreditoByCreditoAsync(numeroCredito, cancellationToken);
        return resultado.ToActionResult(this);
    }

    [HttpPost("integrar-credito-constituido")]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(SimplesResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SimplesResponseDto>> IntegrarCreditoConstituidoAsync(
        [FromBody] List<CreditoRequestDto> creditoRequestDtos)
    {
        var resultado = await _creditoService.IntegraCreditoConstituidoAsync(creditoRequestDtos);

        return resultado.IsSuccess ? Accepted(resultado.Value) : resultado.ToActionResult(this);
    }
}