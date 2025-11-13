using Microsoft.AspNetCore.Mvc;
using Notus.Models.Class;
using Notus.Services;

namespace Notus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly ClassServices _service;

        public ClassController(ClassServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Class>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetById(int id)
        {
            var cls = await _service.GetByIdAsync(id);
            if (cls == null) return NotFound();
            return Ok(cls);
        }

        [HttpPost]
        public async Task<ActionResult<Class>> Add(Class cls)
        {
            var added = await _service.AddAsync(cls);
            return CreatedAtAction(nameof(GetById), new { id = added.Id }, added);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Class>> Update(int id, Class cls)
        {
            cls.Id = id; 

            var updated = await _service.UpdateAsync(cls);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
