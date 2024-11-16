using DB;
using Helpers.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeasurementService.Repositories
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private readonly Context _context;

        public MeasurementRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Measurement>> GetAllAsync()
        {
            return await _context.Measurements.ToListAsync();
        }

        public async Task<Measurement?> GetByIdAsync(int id)
        {
            return await _context.Measurements.FindAsync(id);
        }

        public async Task AddAsync(Measurement measurement)
        {
            await _context.Measurements.AddAsync(measurement);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Measurement measurement)
        {
            _context.Measurements.Update(measurement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var measurement = await GetByIdAsync(id);
            if (measurement != null)
            {
                _context.Measurements.Remove(measurement);
                await _context.SaveChangesAsync();
            }
        }
    }
}
