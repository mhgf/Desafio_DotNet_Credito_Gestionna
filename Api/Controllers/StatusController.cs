using Infra.Database;
using Microsoft.AspNetCore.Mvc;
using Shared.ServiceBus;

namespace DesafioGestionna.Api.Controllers;

[ApiController]
[Route("api")]
public class StatusController : ControllerBase
{
    private readonly CreditoContext _context;
    private readonly ICreditoBusService? _creditoService;

    public StatusController(CreditoContext context, ICreditoBusService creditoService)
    {
        _context = context;
        _creditoService = creditoService;
    }

    [HttpGet("self")]
    public ActionResult Self()
    {
        return Ok("Pronto.");
    }

    [HttpGet("ready")]
    public async Task<ActionResult> Ready()
    {
        try
        {
            var status = await _context.Database.CanConnectAsync();
            if (!status) return StatusCode(503, "Database não disponivel.");

            if (_creditoService is null)
                return StatusCode(503, "ServiceBus não disponivel.");

            return Ok("Pronto.");
        }
        catch (Exception e)
        {
            return StatusCode(503, e.Message);
        }
    }
}