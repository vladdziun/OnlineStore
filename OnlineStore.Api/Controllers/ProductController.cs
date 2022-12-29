using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Api.Repositories.Contracts;
using OnlineStore.Models.Dtos;
using OnlineStore.Api.Extensions;


namespace OnlineStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Gets all product items.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetProducts();
                var productCategories = await _productRepository.GetCategories();

                if( products == null || productCategories == null ) 
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto(productCategories);
                    return Ok(productDtos);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
                throw;
            }
        }

        /// <summary>
        /// Gets product by id.
        /// </summary>
        /// <param name="id">Id</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetProduct(id);

                if (product == null)
                {
                    return BadRequest();
                }
                else
                {

                    var productDto = product.ConvertToDto();

                    return Ok(productDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }

        /// <summary>
        /// Creates product.
        /// </summary>
        /// <param name="productDto">ProductDto</param>
        [HttpPost("create")]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductDto productDto)
        {
            try
            {
                var createdProduct = await _productRepository.CreateProduct(productDto);

                if (createdProduct == null)
                {
                    return BadRequest();
                }
                else
                {
                    var createdProductDto = createdProduct.ConvertToDto();

                    return Ok(createdProductDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error updating data in the database");
            }
        }

        /// <summary>
        /// Updates product
        /// </summary>
        /// <param name="productUpdateDto">ProductDto</param>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> UpdateProductById(ProductDto productUpdateDto)
        {
            try
            {
                var product = await _productRepository.UpdateProduct(productUpdateDto);

                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    var productDto = product.ConvertToDto();

                    return Ok(productDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error updating data in the database");
            }
        }


        /// <summary>
        /// Deletes product by id.
        /// </summary>
        /// <param name="id">Id</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDto>> DeleteProduct(int id)
        {
            try
            {
                await _productRepository.DeleteProduct(id);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error removing data from the database");
            }
        }
    }
}
