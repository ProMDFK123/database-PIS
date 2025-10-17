using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace bolsafeucn_back.src.Infrastructure.Repositories.Implements
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly AppDbContext _context;

        public JobApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobApplication> AddAsync(JobApplication application)
        {
            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task<JobApplication?> GetByStudentAndOfferAsync(string studentId, int offerId)
        {
            return await _context.JobApplications
                .Include(ja => ja.Estudiante)
                .Include(ja => ja.OfertaLaboral)
                .FirstOrDefaultAsync(ja => ja.EstudianteId == studentId && ja.OfertaLaboralId == offerId);
        }

        public async Task<IEnumerable<JobApplication>> GetByStudentIdAsync(string studentId)
        {
            return await _context.JobApplications
                .Include(ja => ja.OfertaLaboral)
                .Where(ja => ja.EstudianteId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobApplication>> GetByOfferIdAsync(int offerId)
        {
            return await _context.JobApplications
                .Include(ja => ja.Estudiante)
                .Where(ja => ja.OfertaLaboralId == offerId)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(JobApplication application)
        {
            _context.JobApplications.Update(application);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}