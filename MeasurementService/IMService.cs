using Helpers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementService
{
    public interface IMService
    {
        Task<List<Measurement>> GetAllMeasurementsAsync();
        Task<List<Measurement>> GetMeasurementsByPatientSSNAsync(string patientSSN);
        Task<Measurement> GetMeasurementByIdAsync(int id);
        Task AddMeasurementAsync(Measurement measurement);
        Task UpdateMeasurementAsync(Measurement measurement);
        Task DeleteMeasurementAsync(int id);
    }
}
