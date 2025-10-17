using bolsafeucn_back.src.Application.DTOs.OfferDTOs;
using bolsafeucn_back.src.Domain.Models;
using Mapster;

namespace bolsafeucn_back.src.Application.Mappers;

public class OfferMapper
{
    public void ConfigureAllMappings()
    {
        TypeAdapterConfig<Offer, OfferSummaryDto>
            .NewConfig()
            .Map(dest => dest.Title, src => src.Title)
            .Map(
                dest => dest.CompanyName,
                src =>
                    src.User.UserType == UserType.Empresa ? src.User.Company!.CompanyName
                    : src.User.UserType == UserType.Particular
                        ? $"{src.User.Individual!.Name} {src.User.Individual!.LastName}"
                    : "Desconocido"
            );

        TypeAdapterConfig<Offer, OfferDetailDto>
            .NewConfig()
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.PostDate, src => src.PublicationDate)
            .Map(dest => dest.EndDate, src => src.FechaFin)
            .Map(dest => dest.Remuneration, src => src.Remuneracion)
            .Map(dest => dest.OfferType, src => src.Tipo.ToString())
            .Map(
                dest => dest.CompanyName,
                src =>
                    src.User.UserType == UserType.Empresa ? src.User.Company!.CompanyName
                    : src.User.UserType == UserType.Particular
                        ? $"{src.User.Individual!.Name} {src.User.Individual!.LastName}"
                    : "Desconocido"
            );
    }
}
