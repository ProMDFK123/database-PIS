using bolsafeucn_back.src.Application.DTOs.BaseResponse;
using bolsafeucn_back.src.Application.DTOs.PublicationDTO;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace bolsafeucn_back.src.Application.Services.Implements
{
    /// <summary>
    /// Servicio para la gestión de publicaciones (Ofertas y Compra/Venta)
    /// </summary>
    public class PublicationService : IPublicationService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IBuySellRepository _buySellRepository;
        private readonly ILogger<PublicationService> _logger;

        private readonly IPublicationRepository _publicationRepository;
        private readonly IMapper _mapper;

        public PublicationService(
            IOfferRepository offerRepository,
            IBuySellRepository buySellRepository,
            ILogger<PublicationService> logger,
            IPublicationRepository publicationRepository,
            IMapper mapper
        )
        {
            _offerRepository = offerRepository;
            _buySellRepository = buySellRepository;
            _logger = logger;
            _publicationRepository = publicationRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Crea una nueva oferta laboral o de voluntariado
        /// </summary>
        public async Task<GenericResponse<string>> CreateOfferAsync(
            CreateOfferDTO offerDTO,
            GeneralUser currentUser
        )
        {
            try
            {
                var offer = new Offer
                {
                    Title = offerDTO.Title,
                    Description = offerDTO.Description,
                    PublicationDate = DateTime.UtcNow,
                    EndDate = offerDTO.EndDate,
                    DeadlineDate = offerDTO.DeadlineDate,
                    Remuneration = (int)offerDTO.Remuneration,
                    OfferType = offerDTO.OfferType,
                    Location = offerDTO.Location,
                    Requirements = offerDTO.Requirements,
                    ContactInfo = offerDTO.ContactInfo,
                    IsCvRequired = offerDTO.IsCvRequired,
                    UserId = currentUser.Id,
                    User = currentUser,
                    Type = Types.Offer,
                    IsActive = true,
                };

                var createdOffer = await _offerRepository.CreateOfferAsync(offer);

                _logger.LogInformation(
                    "Oferta creada exitosamente. ID: {OfferId}, Título: {Title}, Usuario: {UserId}",
                    createdOffer.Id,
                    createdOffer.Title,
                    currentUser.Id
                );

                return new GenericResponse<string>(
                    "Oferta creada exitosamente",
                    $"Oferta ID: {createdOffer.Id}"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al crear oferta para el usuario {UserId}",
                    currentUser.Id
                );
                return new GenericResponse<string>($"Error al crear la oferta: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Crea una nueva publicación de compra/venta
        /// </summary>
        public async Task<GenericResponse<string>> CreateBuySellAsync(
            CreateBuySellDTO buySellDTO,
            GeneralUser currentUser
        )
        {
            try
            {
                var buySell = new BuySell
                {
                    Title = buySellDTO.Title,
                    Description = buySellDTO.Description,
                    UserId = currentUser.Id,
                    User = currentUser,
                    Type = Types.BuySell,
                    Price = buySellDTO.Price,
                    Category = buySellDTO.Category,
                    Location = buySellDTO.Location,
                    ContactInfo = buySellDTO.ContactInfo,
                    PublicationDate = DateTime.UtcNow,
                    IsActive = true,
                };

                var createdBuySell = await _buySellRepository.CreateBuySellAsync(buySell);

                _logger.LogInformation(
                    "Publicación de compra/venta creada exitosamente. ID: {BuySellId}, Título: {Title}, Usuario: {UserId}",
                    createdBuySell.Id,
                    createdBuySell.Title,
                    currentUser.Id
                );

                return new GenericResponse<string>(
                    "Publicación de compra/venta creada exitosamente",
                    $"Publicación ID: {createdBuySell.Id}"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al crear publicación de compra/venta para el usuario {UserId}",
                    currentUser.Id
                );
                return new GenericResponse<string>(
                    $"Error al crear la publicación de compra/venta: {ex.Message}",
                    null
                );
            }
        }

        public async Task<IEnumerable<PublicationsDTO>> GetMyPublishedPublicationsAsync(
            string userId
        )
        {
            // 1. Llama al Repositorio para obtener los datos de la BD
            var publications = await _publicationRepository.GetPublishedPublicationsByUserIdAsync(
                userId
            );

            // 2. Mapea las entidades a DTOs
            var publicationsDto = _mapper.Adapt<IEnumerable<PublicationsDTO>>(
                (TypeAdapterConfig)publications
            );

            // 3. Devuelve el DTO
            return publicationsDto;
        }

        public async Task<IEnumerable<PublicationsDTO>> GetMyRejectedPublicationsAsync(
            string userId
        )
        {
            // 1. Llama al repositorio
            var publications = await _publicationRepository.GetRejectedPublicationsByUserIdAsync(
                userId
            );
            // 2. Mapea y devuelve el DTO
            return _mapper.Adapt<IEnumerable<PublicationsDTO>>((TypeAdapterConfig)publications);
        }

        // --- IMPLEMENTACIÓN PENDING ("InProcess") ---
        public async Task<IEnumerable<PublicationsDTO>> GetMyPendingPublicationsAsync(string userId)
        {
            // 1. Llama al repositorio
            var publications = await _publicationRepository.GetPendingPublicationsByUserIdAsync(
                userId
            );
            // 2. Mapea y devuelve el DTO
            return _mapper.Adapt<IEnumerable<PublicationsDTO>>((TypeAdapterConfig)publications);
        public async Task<IEnumerable<BuySellSummaryDto>> GetAllPendingBuySellsAsync()
        {
            _logger.LogInformation("Obteniendo publicaciones de compra/venta pendientes de validación");

            var BuySells = await _buySellRepository.GetAllPendingBuySellsAsync();
            var list = BuySells.ToList();
            var result = list.Select(o =>
            {
                var ownerName =
                    o.User?.UserType == UserType.Empresa
                        ? (o.User.Company?.CompanyName ?? "Empresa desconocida")
                    : o.User?.UserType == UserType.Particular
                        ? $"{(o.User.Individual?.Name ?? "").Trim()} {(o.User.Individual?.LastName ?? "").Trim()}".Trim()
                    : (o.User?.UserName ?? "UCN");

                return new BuySellSummaryDto
                {
                    Id = o.Id,
                    Title = o.Title,
                    Category = o.Category,
                    Price = o.Price,
                    UserName = ownerName,
                };
            }).ToList();

            return result;
        }
    }
}
