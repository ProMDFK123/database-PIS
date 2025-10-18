using bolsafeucn_back.src.Application.DTOs.JobAplicationDTO;

namespace bolsafeucn_back.src.Application.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de gestión de postulaciones a ofertas laborales
    /// </summary>
    public interface IJobApplicationService
    {
        /// <summary>
        /// Crea una nueva postulación de un estudiante a una oferta (postulación directa)
        /// </summary>
        Task<JobApplicationResponseDto> CreateApplicationAsync(int studentId, int offerId);

        /// <summary>
        /// Obtiene todas las postulaciones de un estudiante
        /// </summary>
        Task<IEnumerable<JobApplicationResponseDto>> GetStudentApplicationsAsync(int studentId);

        /// <summary>
        /// Obtiene todas las postulaciones recibidas para una oferta específica
        /// </summary>
        Task<IEnumerable<JobApplicationResponseDto>> GetApplicationsByOfferIdAsync(int offerId);

        /// <summary>
        /// Obtiene todas las postulaciones de todas las ofertas de una empresa
        /// </summary>
        Task<IEnumerable<JobApplicationResponseDto>> GetApplicationsByCompanyIdAsync(int companyId);

        /// <summary>
        /// Actualiza el estado de una postulación (Pendiente, Aceptado, Rechazado)
        /// </summary>
        Task<bool> UpdateApplicationStatusAsync(int applicationId, string newStatus, int companyId);

        /// <summary>
        /// Valida si un estudiante es elegible para postular
        /// </summary>
        Task<bool> ValidateStudentEligibilityAsync(int studentId, bool isCvRequired = true);
    }
}
