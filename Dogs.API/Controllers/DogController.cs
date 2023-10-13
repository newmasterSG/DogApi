using Dogs.Application.DTO;
using Dogs.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dogs.API.Controllers
{
    [Route("api/dogs")]
    [ApiController]
    public class DogController : ControllerBase
    {
        private readonly IDogService _dogService;

        public DogController(IDogService dogService)
        {
            _dogService = dogService;
        }

        [HttpGet]
        public async Task<ActionResult<List<DogDTO>>> GetAllDogsAsync(string attribute = "name", string order = "asc", int page = 1, int pageSize = 10)
        {
            var dogs = await _dogService.GetAllDogsAsync(page, pageSize, attribute, order);

            return Ok(dogs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DogDTO>> GetDogAsync(int id)
        {
            if(id <= 0)
            {
                return BadRequest(ModelState);
            }

            var dog = await _dogService.GetDogByIdAsync(id);

            if(dog == null)
            {
                return NotFound();
            }

            return Ok(dog);
        }

        [HttpPost]
        public async Task<ActionResult> AddDogAsync([FromBody] DogDTO dog)
        {
            if (ModelState.IsValid)
            {
                var existingDog = await _dogService.GetDogByNameAsync(dog.Name);

                if (existingDog != null)
                {
                    return Conflict("The dog with that name already exists in the database.");
                }

                await _dogService.AddSync(dog);

                return Created(string.Empty, dog);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            bool isDeleted = await _dogService.DeleteAsync(id);

            int status = isDeleted ? 1 : 0;

            switch (status)
            {
                case 1:
                    return Ok();
                case 0:
                    return BadRequest();
                default:
                    return StatusCode(500, "Unexpected error");
            }
        }

        [HttpPatch("{id}")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] DogDTO patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dog = await _dogService.GetDogByIdAsync(id);

            if (dog == null)
            {
                return NotFound();
            }

            await _dogService.UpdateAsync(id, patchDoc);

            return HttpContext.Request.Method switch
            {
                "PATCH" => NoContent(),
                "PUT" => Ok(patchDoc),
                _ => BadRequest("Unsupported HTTP method"),
            };
        }
    }
}
