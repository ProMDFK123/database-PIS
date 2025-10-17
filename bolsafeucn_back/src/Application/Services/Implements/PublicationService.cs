using bolsafeucn_back.src.Application.DTO.BaseResponse;
using bolsafeucn_back.src.Application.DTO.PublicationDTO;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;

namespace bolsafeucn_back.src.Application.Services.Implements
{
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
                    ExpirationDate = offerDTO.ExpirationDate,
                    Remuneration = offerDTO.Remuneration,
                    ImagesURL = offerDTO.ImagesURL,
                    UserId = currentUser.Id,
                    User = currentUser,
                    Type = Types.Offer,
                    IsActive = false,
                };

                await _offerRepository.CreateOfferAsync(offer);
            }
            catch (Exception ex) { }
            throw new NotImplementedException();
        }

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
                    ImagesURL = buySellDTO.ImagesURL,
                    UserId = currentUser.Id,
                    User = currentUser,
                    Type = Types.BuySell,
                    Price = buySellDTO.Price,
                    IsActive = false,
                };

                await _buySellRepository.CreateBuySellAsync(buySell);
            }
            catch (Exception ex) { }
            throw new NotImplementedException();
        }
    }
}
