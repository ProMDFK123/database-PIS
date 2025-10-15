using bolsafeucn_back.src.Application.DTOs.AuthDTOs;
using bolsafeucn_back.src.Domain.Models;
using Mapster;

namespace bolsafeucn_back.src.Application.Mappers
{
    public class IndividualMapper
    {
        public void ConfigureAllMappings()
        {
            ConfigureAuthMapping();
        }

        public void ConfigureAuthMapping()
        {
            TypeAdapterConfig<RegisterIndividualDTO, GeneralUser>
                .NewConfig()
                .Map(dest => dest.UserName, src => src.Email)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                .Map(dest => dest.Rut, src => src.Rut)
                .Map(dest => dest.UserType, src => UserType.Particular)
                .Map(dest => dest.Banned, src => false)
                .Map(dest => dest.EmailConfirmed, src => false);

            TypeAdapterConfig<RegisterIndividualDTO, Individual>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.Rating, src => 0.0f);
        }
    }
}
