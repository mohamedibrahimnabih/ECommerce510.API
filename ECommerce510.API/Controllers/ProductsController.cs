using Microsoft.AspNetCore.Mvc;

namespace ECommerce510.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var products = _productRepository.Get(includes: [e => e.Category]);

            return Ok(products.Adapt<IEnumerable<ProductResponse>>());
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var product = _productRepository.GetOne(e => e.Id == id);

            if(product is not null)
            {
                return Ok(product.Adapt<ProductResponse>());
            }

            return NotFound();
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] ProductRequest productRequest, CancellationToken cancellationToken)
        {
            if(productRequest.File is not null && productRequest.File.Length > 0)
            {
                var fileName = await SaveFileAsync(productRequest);

                var product = productRequest.Adapt<Product>();
                product.MainImg = fileName;
                var productCreated = await _productRepository.CreateAsync(product, cancellationToken);

                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product.Adapt<ProductResponse>());
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] ProductRequest productRequest, CancellationToken cancellationToken)
        {
            var product = _productRepository.GetOne(e => e.Id == id, tracked: false);
            var productInDb = productRequest.Adapt<Product>();

            if (product is not null)
            {
                
                if(productRequest.File is not null && productRequest.File.Length > 0)
                {
                    var fileName = await SaveFileAsync(productRequest);

                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "images", product.MainImg);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    product.MainImg = fileName;
                }
                else
                {
                    productInDb.MainImg = product.MainImg;
                }

                var productUpdated = await _productRepository.EditAsync(productInDb, cancellationToken);

                return NoContent();
            }

            return NotFound();
        }

        private async Task<string> SaveFileAsync(ProductRequest productRequest)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productRequest.File.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await productRequest.File.CopyToAsync(stream);
            }

            return fileName;
        }

        [HttpPatch("UpdateToggle/{id}")]
        public async Task<IActionResult> UpdateToggle([FromRoute] int id, CancellationToken cancellationToken)
        {
            var productInDb = _productRepository.GetOne(e => e.Id == id);

            if (productInDb is not null)
            {
                productInDb.Status = !productInDb.Status;

                await _productRepository.CommitAsync(cancellationToken);

                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var productInDb = _productRepository.GetOne(e => e.Id == id);

            if (productInDb is not null)
            {
                await _productRepository.DeleteAsync(productInDb, cancellationToken);

                return NoContent();
            }

            return NotFound();
        }
    }
}
