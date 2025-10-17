using bolsafeucn_back.src.Application.DTOs.BaseResponse;
using bolsafeucn_back.src.Application.DTOs.PublicationDTO;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace bolsafeucn_back.src.Application.Services.Implements
{
    /// <summary>
    /// Servicio para la gestión de publicaciones (Ofertas y Compra/Venta)
    /// </summary>
    public class PublicationService : IPublicationService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IBuySellRepository _buySellRepository;
        private readonly ILogger<PublicationService> _logger;

        public PublicationService(
            IOfferRepository offerRepository,
            IBuySellRepository buySellRepository,
            ILogger<PublicationService> logger
        )
        {
            _offerRepository = offerRepository;
            _buySellRepository = buySellRepository;
            _logger = logger;
        }

        /// <summary>
        /// Crea una nueva oferta laboral o de voluntariado
        /// </summary>
        public async Task<GenericResponse<string>> CreateOfferAsync(
            CreateOfferDTO offerDTO,
            GeneralUser currentUser
        )
        {
            try
            {
                var offer = new Offer
                {
                    Title = offerDTO.Title,
                    Description = offerDTO.Description,
                    PublicationDate = DateTime.UtcNow,
                    EndDate = offerDTO.EndDate,
                    DeadlineDate = offerDTO.DeadlineDate,
                    Remuneration = (int)offerDTO.Remuneration,
                    OfferType = offerDTO.OfferType,
                    Location = offerDTO.Location,
                    Requirements = offerDTO.Requirements,
                    ContactInfo = offerDTO.ContactInfo,
                    UserId = currentUser.Id,
                    User = currentUser,
                    Type = Types.Offer,
                    IsActive = true,
                };

                var createdOffer = await _offerRepository.CreateOfferAsync(offer);

                _logger.LogInformation(
                    "Oferta creada exitosamente. ID: {OfferId}, Título: {Title}, Usuario: {UserId}",
                    createdOffer.Id,
                    createdOffer.Title,
                    currentUser.Id
                );

                return new GenericResponse<string>(
                    "Oferta creada exitosamente",
                    $"Oferta ID: {createdOffer.Id}"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al crear oferta para el usuario {UserId}",
                    currentUser.Id
                );
                return new GenericResponse<string>($"Error al crear la oferta: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Crea una nueva publicación de compra/venta
        /// </summary>
        public async Task<GenericResponse<string>> CreateBuySellAsync(
            CreateBuySellDTO buySellDTO,
            GeneralUser currentUser
        )
        {
            try
            {
                var buySell = new BuySell
                {
                    Title = buySellDTO.Title,
                    Description = buySellDTO.Description,
                    UserId = currentUser.Id,
                    User = currentUser,
                    Type = Types.BuySell,
                    Price = buySellDTO.Price,
                    Category = buySellDTO.Category,
                    Location = buySellDTO.Location,
                    ContactInfo = buySellDTO.ContactInfo,
                    PublicationDate = DateTime.UtcNow,
                    IsActive = true,
                };

                var createdBuySell = await _buySellRepository.CreateBuySellAsync(buySell);

                _logger.LogInformation(
                    "Publicación de compra/venta creada exitosamente. ID: {BuySellId}, Título: {Title}, Usuario: {UserId}",
                    createdBuySell.Id,
                    createdBuySell.Title,
                    currentUser.Id
                );

                return new GenericResponse<string>(
                    "Publicación de compra/venta creada exitosamente",
                    $"Publicación ID: {createdBuySell.Id}"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al crear publicación de compra/venta para el usuario {UserId}",
                    currentUser.Id
                );
                return new GenericResponse<string>(
                    $"Error al crear la publicación de compra/venta: {ex.Message}",
                    null
                );
            }
        }
    }
}
