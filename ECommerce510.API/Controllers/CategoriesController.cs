using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce510.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

            //List<CategoryResponse> categoryResponses = [];
            //foreach (var item in categories)
            //{
            //    CategoryResponse categoryResponse = new()
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        Description = item.Description,
            //        Status = item.Status
            //    };

            //    categoryResponses.Add(categoryResponse);
            //}

            //List<CategoryResponse> categoryResponses = [];

            //categoryResponses.AddRange(categories.Select(e => new CategoryResponse()
            //{
            //    Id = e.Id,
            //    Name = e.Name,
            //    Description = e.Description,
            //    Status = e.Status
            //}));

            return Ok(categories.Adapt<IEnumerable<CategoryResponse>>());
        }

        [HttpGet("{id}")]
        public IActionResult GetOne([FromRoute] int id)
        {
            var category = _categoryRepository.GetOne(e => e.Id == id);

            if (category is not null)
            {
                //CategoryResponse categoryResponse = new()
                //{
                //    Id = category.Id,
                //    Name = category.Name,
                //    Description = category.Description,
                //    Status = category.Status
                //};

                return Ok(category.Adapt<CategoryResponse>());
            }

            return NotFound();
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CategoryRequest categoryRequest, CancellationToken cancellationToken)
        {
            var category = categoryRequest.Adapt<Category>();

            var categoryCreated = await _categoryRepository.CreateAsync(category, cancellationToken);

            //return Created($"{Request.Scheme}://{Request.Host}/api/Categories/{category.Id}", category);
            return CreatedAtAction(nameof(GetOne), new { id = categoryCreated.Id }, category.Adapt<CategoryResponse>());
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CategoryRequest categoryRequest, CancellationToken cancellationToken)
        {
            var categoryInDb = _categoryRepository.GetOne(e => e.Id == id);
            if (categoryInDb is not null)
            {
                categoryInDb.Name = categoryRequest.Name;
                categoryInDb.Description = categoryRequest.Description;

                await _categoryRepository.CommitAsync(cancellationToken);

                return NoContent();
            }

            return NotFound();
        }

        [HttpPatch("UpdateToggle/{id}")]
        public async Task<IActionResult> UpdateToggle([FromRoute] int id, CancellationToken cancellationToken)
        {
            var categoryInDb = _categoryRepository.GetOne(e => e.Id == id);

            if (categoryInDb is not null)
            {
                categoryInDb.Status = !categoryInDb.Status;

                await _categoryRepository.CommitAsync(cancellationToken);

                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var categoryInDb = _categoryRepository.GetOne(e => e.Id == id);

            if (categoryInDb is not null)
            {
                await _categoryRepository.DeleteAsync(categoryInDb, cancellationToken);

                return NoContent();
            }

            return NotFound();
        }
    }
}
