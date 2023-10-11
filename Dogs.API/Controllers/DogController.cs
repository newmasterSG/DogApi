using Dogs.Application.DTO;
using Dogs.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dogs.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DogController : ControllerBase
    {
        private readonly IDogService _dogService;

        public DogController(IDogService dogService)
        {
            _dogService = dogService;
        }

        [HttpGet]
        public async Task<ActionResult<List<DogDTO>>> GetAllDogs(string attribute = "name", string order = "asc", int page = 1, int pageSize = 10)
        {
            var dogs = await _dogService.GetAllDogsAsync(page,pageSize, attribute, order);

            return new JsonResult(dogs);
        }

        [HttpPost]
        public async Task<ActionResult> AddDog([FromBody] DogDTO dog)
        {
            if (ModelState.IsValid)
            {
                if (dog != null)
                {
                    var existingDog = await _dogService.GetDogByName(dog.Name);

                    if (existingDog != null)
                    {
                        return Conflict("The dog with that name already exists in the database.");
                    }

                    await _dogService.AddSync(dog);
                    var dbDog = await _dogService.GetDogByName(dog.Name);
                    var url = Url.Action(nameof(AddDog), new { id = dbDog.Id }) ?? $"/{dbDog.Id}";
                    return Created(url, dog);
                }
            }

            return BadRequest();
        }
    }
}
