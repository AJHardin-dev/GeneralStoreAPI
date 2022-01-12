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

        // GET all
        // api/Product
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Product> products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // get by id
        // api/Product/{id}
        [HttpGet]
        public async Task<IHttpActionResult> GetBySku([FromUri] string sku)
        {
            Product product = await _context.Products.FindAsync(sku);

            if (product != null)
                return Ok(product);
            else return NotFound();
        }
    }
}
