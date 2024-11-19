using Helpers.Entities;

namespace MeasurementService.Repositories
{
    public interface IMeasurementRepository
    {
        Task<IEnumerable<Measurement>> GetAllAsync();
        Task<Measurement?> GetByIdAsync(int id);
        Task AddAsync(Measurement measurement);
        Task UpdateAsync(Measurement measurement);
        Task DeleteAsync(int id);
        Task<IEnumerable<Measurement>> GetAllBySSNAsync(string ssn);
    }
}
