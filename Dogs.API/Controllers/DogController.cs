using Dogs.Application.DTO;
using Dogs.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<List<DogDTO>>> GetAllDogs()
        {
            var dogs = await _dogService.GetAllDogs();

            return new JsonResult(dogs);
        }
    }
}
