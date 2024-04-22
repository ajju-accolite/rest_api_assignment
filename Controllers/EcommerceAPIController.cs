using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restapi_assignment.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace restapi_assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EcommerceAPIController : ControllerBase
    {
        private readonly ECommerceDbContext context;

        public EcommerceAPIController(ECommerceDbContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var data = await context.Products.ToListAsync();
            return Ok(data) ;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            return product;

        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve // Configure to handle object cycles
            };

            var new_product = await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            //return Ok(new_product);
            return Ok(new_product.Entity);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
           if(id != product.ProductId)
            {
                return BadRequest();
            }
           context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(product);

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var deleteproduct = await context.Products.FindAsync(id);  
            if(deleteproduct == null)
            {
                return NotFound();
            }
            context.Products.Remove(deleteproduct);
            await context.SaveChangesAsync();
            return Ok();

        }
    }
}
