using Core.Servicos.Credito;
using Core.Servicos.Dtos;
using Microsoft.AspNetCore.Mvc;
using Ardalis.Result.AspNetCore;

namespace DesafioGestionna.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditosController : ControllerBase
{
    private readonly ICreditoService _creditoService;

    public CreditosController(ICreditoService creditoService)
        => _creditoService = creditoService;

    [HttpPost("integrar-credito-constituido")]
    public async Task<ActionResult> IntegrarCreditoConstituidoAsync([FromBody] List<CreditoRequestDto> creditoRequestDtos)
    {
        var resultado = await _creditoService.IntegraCreditoConstituidoAsync(creditoRequestDtos);
        return resultado.ToActionResult(this);
    }
}