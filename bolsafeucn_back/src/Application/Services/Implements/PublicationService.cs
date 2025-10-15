using bolsafeucn_back.src.Application.DTO.BaseResponse;
using bolsafeucn_back.src.Application.DTO.PublicationDTO;
using bolsafeucn_back.src.Application.Services.Interfaces;

namespace bolsafeucn_back.src.Application.Services.Implements
{
    public class PublicationService : IPublicationService
    {
        public async Task<GenericResponse<string>> CreateOfferAsync(CreateOfferDTO offerDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<GenericResponse<string>> CreatePurchaseAsync(
            CreatePurchaseDTO purchaseDTO
        )
        {
            throw new NotImplementedException();
        }
    }
}
