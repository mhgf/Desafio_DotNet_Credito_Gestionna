using Ardalis.Result;
using Core.Servicos.Credito.Dtos;
using Shared.ServiceBus.Dtos;

namespace Core.Servicos.Credito;

public interface ICreditoService
{
    Task<Result<SimplesResponseDto>> IntegraCreditoConstituidoAsync(IList<CreditoRequestDto> creditoRequestDtos);

    Task<Result> InsertCreditoAsync(CreditoRequestDto creditoRequestDto);

    Task<Result<IEnumerable<CreditoResponseDto>>> GetCreditoByNfseAsync(string numeroNfse, CancellationToken cancellationToken = default);
    Task<Result<CreditoResponseDto>> GetCreditoByCreditoAsync(string numeroCredito, CancellationToken cancellationToken = default);
}