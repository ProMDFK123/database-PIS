using bolsafeucn_back.src.Application.DTOs;

namespace bolsafeucn_back.src.Application.Services.Interfaces
{
    public interface IJobApplicationService
    {
        Task<JobApplicationResponseDto> CreateApplicationAsync(string studentId, CreateJobApplicationDto dto);
        Task<IEnumerable<JobApplicationResponseDto>> GetStudentApplicationsAsync(string studentId);
        Task<bool> ValidateStudentEligibilityAsync(string studentId);
    }
}