namespace bolsafeucn_back.src.Application.DTOs.OfferDTOs;

public class OfferSummaryDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string CompanyName { get; set; }
    public string? Location { get; set; }
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
