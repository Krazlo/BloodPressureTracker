using DB;
using Helpers.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementService
{
    public class MService : IMService
    {
        private readonly Context _dbContext;

        public MService(Context dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddMeasurementAsync(Measurement measurement)
        {
            await _dbContext.Measurements.AddAsync(measurement);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteMeasurementAsync(int id)
        {
            var measurement = await _dbContext.Measurements.FirstOrDefaultAsync(m => m.ID == id);
            if (measurement != null)
            {
                _dbContext.Measurements.Remove(measurement);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Measurement>> GetAllMeasurementsAsync()
        {
            return await _dbContext.Measurements.Include(m => m.Patient).ToListAsync();
        }

        public async Task<Measurement> GetMeasurementByIdAsync(int id)
        {
            return await _dbContext.Measurements
            .Include(m => m.Patient)
            .FirstOrDefaultAsync(m => m.ID == id);
        }

        public async Task<List<Measurement>> GetMeasurementsByPatientSSNAsync(string patientSSN)
        {
            return await _dbContext.Measurements
            .Where(m => m.PatientSSN == patientSSN)
            .Include(m => m.Patient)
            .ToListAsync();
        }

        public async Task UpdateMeasurementAsync(Measurement measurement)
        {
            _dbContext.Measurements.Update(measurement);
            await _dbContext.SaveChangesAsync();
        }
    }
}
