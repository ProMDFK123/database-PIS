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
                .Map(dest => dest.PhoneNumber, src => src.Telefono)
                .Map(dest => dest.Rut, src => src.Rut)
                .Map(dest => dest.TipoUsuario, src => UserType.Particular)
                .Map(dest => dest.Bloqueado, src => false)
                .Map(dest => dest.EmailConfirmed, src => false);

            TypeAdapterConfig<RegisterIndividualDTO, Individual>
                .NewConfig()
                .Map(dest => dest.Nombre, src => src.Nombre)
                .Map(dest => dest.Apellido, src => src.Apellido)
                .Map(dest => dest.Calificacion, src => 0.0f);
        }
    }
}
