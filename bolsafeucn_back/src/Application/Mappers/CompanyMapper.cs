using bolsafeucn_back.src.Application.DTOs.AuthDTOs;
using bolsafeucn_back.src.Domain.Models;
using Mapster;

namespace bolsafeucn_back.src.Application.Mappers
{
    public class CompanyMapper
    {
        public void ConfigureAllMappings()
        {
            ConfigureAuthMapping();
        }

        public void ConfigureAuthMapping()
        {
            TypeAdapterConfig<RegisterCompanyDTO, GeneralUser>
                .NewConfig()
                .Map(dest => dest.UserName, src => src.Email)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.PhoneNumber, src => src.Telefono)
                .Map(dest => dest.Rut, src => src.Rut)
                .Map(dest => dest.TipoUsuario, src => UserType.Empresa)
                .Map(dest => dest.Bloqueado, src => false)
                .Map(dest => dest.EmailConfirmed, src => false);

            TypeAdapterConfig<RegisterCompanyDTO, Company>
                .NewConfig()
                .Map(dest => dest.NombreEmpresa, src => src.NombreEmpresa)
                .Map(dest => dest.RazonSocial, src => src.RazonSocial)
                .Map(dest => dest.Calificacion, src => 0.0f);
        }
    }
}
