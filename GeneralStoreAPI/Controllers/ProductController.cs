using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStoreAPI.Controllers
{
    public class ProductController : ApiController
    {
        private readonly ProductDbContext _context = new ProductDbContext();
            
        // api/Product
        [HttpPost]
        public async Task<IHttpActionResult> PostProduct([FromBody]Product model)
        {
            if (model == null)
                return BadRequest("Can't be null");
            else if (model != null && ModelState.IsValid)
            {
                _context.Products.Add(model);
                await _context.SaveChangesAsync();

                return Ok("Success");
            }
            else return BadRequest(ModelState);
        }

        // api/Product
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Product> products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // api/Product?sku={sku}
        [HttpGet]
        public async Task<IHttpActionResult> GetBySku([FromUri] string sku)
        {
            Product product = await _context.Products.FindAsync(sku);

            if (product != null)
                return Ok(product);
            else return NotFound();
        }

        // api/Product?sku={sku}
        [HttpPut]
        public async Task<IHttpActionResult> UpdateProduct([FromUri] string sku, [FromBody] Product updatedProduct)
        {
            if (sku != updatedProduct?.Sku)
                return BadRequest("Uri sku does not match product sku");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Product product = await _context.Products.FindAsync(sku);

            if (product == null)
                return NotFound();

            product.Name = updatedProduct.Name;
            product.Cost = updatedProduct.Cost;
            product.NumberInInventory = updatedProduct.NumberInInventory;

            await _context.SaveChangesAsync();

            return Ok("Product updated");
        }

        // api/Product?sku={sku}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProduct([FromUri] string sku)
        {
            Product product = await _context.Products.FindAsync(sku);

            if (product == null)
                return NotFound();

            _context.Products.Remove(product);

            if (await _context.SaveChangesAsync() == 1)
                return Ok("Product deleted");

            return InternalServerError();
        }
    }
}
