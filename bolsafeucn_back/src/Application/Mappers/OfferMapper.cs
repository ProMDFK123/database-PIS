using bolsafeucn_back.src.Application.DTOs.OfferDTOs;
using bolsafeucn_back.src.Domain.Models;
using Mapster;

namespace bolsafeucn_back.src.Application.Mappers;

public class OfferMapper
{
    public void ConfigureAllMappings()
    {
        TypeAdapterConfig<Offer, OfferSummaryDto>.NewConfig()
            .Map(dest => dest.Title, src => src.Titulo)
            .Map(dest => dest.CompanyName, src => 
                src.Oferente.UserType == UserType.Empresa ? src.Oferente.Company!.CompanyName : 
                src.Oferente.UserType == UserType.Particular ? $"{src.Oferente.Individual!.Name} {src.Oferente.Individual!.LastName}" : "Desconocido");

        TypeAdapterConfig<Offer, OfferDetailDto>.NewConfig()
            .Map(dest => dest.Title, src => src.Titulo)
            .Map(dest => dest.Description, src => src.Descripcion)
            .Map(dest => dest.PostDate, src => src.FechaPublicacion)
            .Map(dest => dest.EndDate, src => src.FechaFin)
            .Map(dest => dest.Remuneration, src => src.Remuneracion)
            .Map(dest => dest.OfferType, src => src.Tipo.ToString())
            .Map(dest => dest.CompanyName, src => 
                src.Oferente.UserType == UserType.Empresa ? src.Oferente.Company!.CompanyName : 
                src.Oferente.UserType == UserType.Particular ? $"{src.Oferente.Individual!.Name} {src.Oferente.Individual!.LastName}" : "Desconocido");
    }
}
