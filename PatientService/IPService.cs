using Helpers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService
{
    public interface IPService
    {
        Task<List<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientBySSNAsync(string ssn);
        Task AddPatientAsync(Patient patient);
        Task UpdatePatientAsync(Patient patient);
        Task DeletePatientAsync(string ssn);
    }
}
