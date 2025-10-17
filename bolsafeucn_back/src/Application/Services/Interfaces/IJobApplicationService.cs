using bolsafeucn_back.src.Application.DTOs;

namespace bolsafeucn_back.src.Application.Services.Interfaces
{
    public interface IJobApplicationService
    {
        Task<JobApplicationResponseDto> CreateApplicationAsync(
            int studentId,
            CreateJobApplicationDto dto
        );
        Task<IEnumerable<JobApplicationResponseDto>> GetStudentApplicationsAsync(int studentId);
        Task<bool> ValidateStudentEligibilityAsync(int studentId);
    }
}
