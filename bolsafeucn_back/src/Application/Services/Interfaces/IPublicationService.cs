using bolsafeucn_back.src.Application.DTO.BaseResponse;
using bolsafeucn_back.src.Application.DTO.PublicationDTO;

namespace bolsafeucn_back.src.Application.Services.Interfaces
{
    public interface IPublicationService
    {
        Task<GenericResponse<string>> CreateOfferAsync(CreateOfferDTO publicationDTO);
        Task<GenericResponse<string>> CreatePurchaseAsync(CreatePurchaseDTO publicationDTO);
    }
}
