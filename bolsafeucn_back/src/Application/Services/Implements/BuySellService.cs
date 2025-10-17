using bolsafeucn_back.src.Application.DTOs.PublicationDTO;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace bolsafeucn_back.src.Application.Services.Implements
{
    /// <summary>
    /// Servicio para la gestión de publicaciones de compra/venta
    /// </summary>
    public class BuySellService : IBuySellService
    {
        private readonly IBuySellRepository _buySellRepository;
        private readonly ILogger<BuySellService> _logger;

        public BuySellService(IBuySellRepository buySellRepository, ILogger<BuySellService> logger)
        {
            _buySellRepository = buySellRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<BuySellSummaryDto>> GetActiveBuySellsAsync()
        {
            try
            {
                var buySells = await _buySellRepository.GetAllActiveAsync();

                var buySellDtos = buySells.Select(bs => new BuySellSummaryDto
                {
                    Id = bs.Id,
                    Title = bs.Title,
                    Category = bs.Category,
                    Price = bs.Price,
                    Location = bs.Location,
                    PublicationDate = bs.PublicationDate,
                    FirstImageUrl = bs.Images.FirstOrDefault()?.Url,
                    UserId = bs.UserId,
                    UserName = bs.User.UserName ?? "Usuario",
                });

                _logger.LogInformation(
                    "Recuperadas {Count} publicaciones de compra/venta activas",
                    buySellDtos.Count()
                );
                return buySellDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener publicaciones de compra/venta activas");
                throw;
            }
        }

        public async Task<BuySellDetailDto?> GetBuySellDetailsAsync(int buySellId)
        {
            try
            {
                var buySell = await _buySellRepository.GetByIdAsync(buySellId);

                if (buySell == null)
                {
                    _logger.LogWarning(
                        "Publicación de compra/venta con ID {BuySellId} no encontrada",
                        buySellId
                    );
                    return null;
                }

                var buySellDto = new BuySellDetailDto
                {
                    Id = buySell.Id,
                    Title = buySell.Title,
                    Description = buySell.Description,
                    Category = buySell.Category,
                    Price = buySell.Price,
                    Location = buySell.Location,
                    ContactInfo = buySell.ContactInfo,
                    PublicationDate = buySell.PublicationDate,
                    IsActive = buySell.IsActive,
                    ImageUrls = buySell.Images.Select(img => img.Url).ToList(),
                    UserId = buySell.UserId,
                    UserName = buySell.User.UserName ?? "Usuario",
                    UserEmail = buySell.User.Email ?? "",
                };

                _logger.LogInformation(
                    "Detalles de publicación de compra/venta {BuySellId} recuperados exitosamente",
                    buySellId
                );
                return buySellDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al obtener detalles de la publicación de compra/venta {BuySellId}",
                    buySellId
                );
                throw;
            }
        }
    }
}
