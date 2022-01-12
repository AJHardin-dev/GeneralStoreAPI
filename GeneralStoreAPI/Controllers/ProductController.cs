using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
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

    }
}
