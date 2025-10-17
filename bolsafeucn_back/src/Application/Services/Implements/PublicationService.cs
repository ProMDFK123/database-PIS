using bolsafeucn_back.src.Application.DTO.BaseResponse;
using bolsafeucn_back.src.Application.DTO.PublicationDTO;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;

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
                    PublicationDate = offerDTO.PublicationDate,
                    EndDate = offerDTO.ExpirationDate,
                    DeadlineDate = offerDTO.ExpirationDate,
                    Remuneration = (int)offerDTO.Remuneration,
                    OfferType = OfferTypes.Trabajo, // Por defecto, se puede ajustar según la categoría
                    UserId = currentUser.Id,
                    User = currentUser,
                    Type = Types.Offer,
                    IsActive = false,
                    Active = false,
                };

                await _offerRepository.CreateOfferAsync(offer);
            }
            catch (Exception)
            {
                // TODO: Registrar el error en los logs
            }
            throw new NotImplementedException();
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
            }
            catch (Exception)
            {
                // TODO: Registrar el error en los logs
            }
            throw new NotImplementedException();
        }
    }
}
