using Helpers.Entities;
using Microsoft.AspNetCore.Mvc;
using PatientService.Repositories;

namespace PatientService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : Controller
    {
        private readonly IPatientRepository _repository;

        public PatientController(IPatientRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetAll()
        {
            var patients = await _repository.GetAllAsync();
            return Ok(patients);
        }

        [HttpGet("{ssn}")]
        public async Task<ActionResult<Patient>> GetBySSN(string ssn)
        {
            var patient = await _repository.GetBySSNAsync(ssn);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Patient patient)
        {
            await _repository.AddAsync(patient);
            return CreatedAtAction(nameof(GetBySSN), new { ssn = patient.SSN }, patient);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string ssn, Patient patient)
        {
            if (ssn != patient.SSN) return BadRequest();
            await _repository.UpdateAsync(patient);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string ssn)
        {
            await _repository.DeleteAsync(ssn);
            return NoContent();
        }
    }
}
