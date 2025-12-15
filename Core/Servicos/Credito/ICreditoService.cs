using Ardalis.Result;
using Core.Servicos.Dtos;

namespace Core.Servicos.Credito;

public interface ICreditoService
{
    Task<Result> IntegraCreditoConstituidoAsync(IList<CreditoRequestDto> creditoRequestDtos);
}