using Helpers.Entities;
using MeasurementService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeasurementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        private readonly IMeasurementRepository _repository;

        public MeasurementController(IMeasurementRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetAll()
        {
            var measurements = await _repository.GetAllAsync();
            return Ok(measurements);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Measurement>> GetById(int id)
        {
            var measurement = await _repository.GetByIdAsync(id);
            if (measurement == null) return NotFound();
            return Ok(measurement);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Measurement measurement)
        {
            await _repository.AddAsync(measurement);
            return CreatedAtAction(nameof(GetById), new { id = measurement.ID }, measurement);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Measurement measurement)
        {
            if (id != measurement.ID) return BadRequest();
            await _repository.UpdateAsync(measurement);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
