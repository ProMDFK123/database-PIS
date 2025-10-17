using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Interfaces
{
    public interface IJobApplicationRepository
    {
        Task<JobApplication> AddAsync(JobApplication application);
        Task<JobApplication?> GetByStudentAndOfferAsync(string studentId, int offerId);
        Task<IEnumerable<JobApplication>> GetByStudentIdAsync(string studentId);
        Task<IEnumerable<JobApplication>> GetByOfferIdAsync(int offerId);
        Task<bool> UpdateAsync(JobApplication application);
    }
}