namespace bolsafeucn_back.src.Application.DTOs.OfferDTOs;
using bolsafeucn_back.src.Domain.Models;

public class OfferSummaryDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string CompanyName { get; set; }
    public string? Location { get; set; }

    public decimal Remuneration { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime PublicationDate { get; set; }
    public OfferTypes OfferType { get; set; }

    // opcional: si quieres devolver ya formateado el “oferente”
    public string OwnerName { get; set; } = "UCN";
}

public class OfferDetailDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string CompanyName { get; set; }
    public string? Location { get; set; }
    public DateTime PostDate { get; set; }
    public DateTime EndDate { get; set; }
    public required int Remuneration { get; set; }
    public required string OfferType { get; set; }
}
