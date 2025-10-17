using bolsafeucn_back.src.Application.DTOs.OfferDTOs;
using bolsafeucn_back.src.Domain.Models;
using Mapster;

namespace bolsafeucn_back.src.Application.Mappers;

/// <summary>
/// Configuración de mapeos entre entidades Offer y sus DTOs usando Mapster
/// </summary>
public class OfferMapper
{
    /// <summary>
    /// Configura todos los mapeos relacionados con Offer
    /// </summary>
    public void ConfigureAllMappings()
    {
        // Mapeo de Offer a OfferSummaryDto (resumen para listados)
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

        // Mapeo de Offer a OfferDetailDto (detalle completo de la oferta)
        TypeAdapterConfig<Offer, OfferDetailDto>
            .NewConfig()
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.PostDate, src => src.PublicationDate)
            .Map(dest => dest.EndDate, src => src.EndDate)
            .Map(dest => dest.Remuneration, src => src.Remuneration)
            .Map(dest => dest.OfferType, src => src.OfferType.ToString())
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
