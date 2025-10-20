using Bogus.DataSets;
using bolsafeucn_back.src.Application.DTO.BaseResponse;
using bolsafeucn_back.src.Application.DTO.PublicationDTO;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Serilog;

namespace bolsafeucn_back.src.Application.Services.Implements
{
    /// <summary>
    /// Servicio para la gestión de publicaciones (Ofertas y Compra/Venta)
    /// </summary>
    public class PublicationService : IPublicationService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IBuySellRepository _buySellRepository;

        public PublicationService(
            IOfferRepository offerRepository,
            IBuySellRepository buySellRepository
        )
        {
            _offerRepository = offerRepository;
            _buySellRepository = buySellRepository;
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
                    PublicationDate = DateTime.SpecifyKind(offerDTO.PublicationDate, DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(offerDTO.ExpirationDate, DateTimeKind.Utc),
                    Remuneration = (int)offerDTO.Remuneration,
                    OfferType = OfferTypes.Trabajo, // Por defecto, se puede ajustar según la categoría
                    UserId = currentUser.Id,
                    User = currentUser,
                    Type = Types.Offer,
                    IsActive = false,
                    Active = false,
                };

                await _offerRepository.CreateOfferAsync(offer);
                return new GenericResponse<string>(
                    message: "Oferta creada exitosamente",
                    data: offer.Id.ToString(),
                    success: true
                );
            }
            catch (Exception)
            {
                Log.Error("Error al crear la oferta para el usuario {UserId}", currentUser.Id);
                return new GenericResponse<string>(
                    message: "Error al crear la oferta",
                    data: null,
                    success: false
                );
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
                    IsActive = false,
                };

                await _buySellRepository.CreateBuySellAsync(buySell);
                return new GenericResponse<string>(
                    message: "Publicación de compra/venta creada exitosamente",
                    data: buySell.Id.ToString(),
                    success: true
                );
            }
            catch (Exception)
            {
                Log.Error("Error al crear la publicación de compra/venta para el usuario {UserId}", currentUser.Id);
                return new GenericResponse<string>(
                    message: "Error al crear la publicación de compra/venta",
                    data: null,
                    success: false
                );
            }
        }
    }
}
