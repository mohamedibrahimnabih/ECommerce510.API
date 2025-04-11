using Microsoft.AspNetCore.Mvc;

namespace ECommerce510.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var categories = _categoryRepository.Get();

            return Ok(categories.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetOne([FromRoute] int id)
        {
            var category = _categoryRepository.GetOne(e => e.Id == id);

            if (category is not null)
                return Ok(category);

            return NotFound();
        }

        [HttpPost("")]
        public IActionResult Create([FromBody] Category category)
        {
            _categoryRepository.Create(category);
            _categoryRepository.Commit();

            //return Created($"{Request.Scheme}://{Request.Host}/api/Categories/{category.Id}", category);
            return CreatedAtAction(nameof(GetOne), new { id = category.Id }, category);
        }


        [HttpPatch("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] Category category)
        {
            var categoryInDb = _categoryRepository.GetOne(e => e.Id == id);
            if (categoryInDb is not null)
            {
                categoryInDb.Name = category.Name;
                categoryInDb.Description = category.Description;

                _categoryRepository.Commit();

                return NoContent();
            }

            return NotFound();
        }

        [HttpPatch("UpdateToggle/{id}")]
        public IActionResult UpdateToggle([FromRoute] int id)
        {
            var categoryInDb = _categoryRepository.GetOne(e => e.Id == id);

            if (categoryInDb is not null)
            {
                categoryInDb.Status = !categoryInDb.Status;

                _categoryRepository.Commit();

                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var categoryInDb = _categoryRepository.GetOne(e => e.Id == id);

            if (categoryInDb is not null)
            {
                _categoryRepository.Delete(categoryInDb);

                _categoryRepository.Commit();

                return NoContent();
            }

            return NotFound();
        }
    }
}
