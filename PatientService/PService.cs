using DB;
using Helpers.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService
{
    public class PService : IPService
    {
        private readonly Context _dbContext;

        public PService(Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            return await _dbContext.Patients.ToListAsync();
        }

        public async Task<Patient> GetPatientBySSNAsync(string ssn)
        {
            return await _dbContext.Patients.FirstOrDefaultAsync(p => p.SSN == ssn);
        }

        public async Task AddPatientAsync(Patient patient)
        {
            await _dbContext.Patients.AddAsync(patient);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            _dbContext.Patients.Update(patient);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(string ssn)
        {
            var patient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.SSN == ssn);
            if (patient != null)
            {
                _dbContext.Patients.Remove(patient);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
