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
    public class TransactionController : ApiController
    {
        private readonly TransactionDbContext _context = new TransactionDbContext();

        // api/Transaction
        // still to do:
        //    verify product is in stock
        //    verify enough product to complete transaction
        //    remove products that were purchased
        [HttpPost]
        public async Task<IHttpActionResult> PostTransaction([FromBody] Transaction model)
        {
            if (model == null)
                return BadRequest("Can't be null");
            else if (model != null && ModelState.IsValid)
            {
                _context.Transactions.Add(model);
                await _context.SaveChangesAsync();

                return Ok("Success");
            }
            else return BadRequest(ModelState);
        }

        // api/Transaction
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Transaction> transactions = await _context.Transactions.ToListAsync();
            return Ok(transactions);
        }

        // api/Transaction/{customerid}
        [HttpGet]
        public async Task<IHttpActionResult> GetAllByCustomerId (int customerId)
        {
            // get all by cust id
            return BadRequest("Not ready yet");
        }

        // api/Transaction/{id}
        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            Transaction transaction = await _context.Transactions.FindAsync(id);

            if (transaction != null)
                return Ok(transaction);
            else return NotFound();
        }

        // api/Transaction/{id}
        [HttpPut]
        public async Task<IHttpActionResult> UpdateTransaction([FromUri] int id, [FromBody] Transaction updatedTransaction)
        {
            if (id != updatedTransaction?.Id)
                return BadRequest("Uri ID does not match transaction ID");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Transaction transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
                return NotFound();

            // to do:
            //    verify product changes
            //    update product inventory
            transaction.CustomerID = updatedTransaction.CustomerID;
            transaction.ProductSku = updatedTransaction.ProductSku;
            transaction.ItemCount = updatedTransaction.ItemCount;
            transaction.DateOfTransaction = updatedTransaction.DateOfTransaction;

            await _context.SaveChangesAsync();

            return Ok("Transaction updated");
        }

        // api/Transaction/{id}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteTransaction([FromUri] int id)
        {
            Transaction transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
                return NotFound();

            // to do:
            //    update product inventory
            _context.Transactions.Remove(transaction);

            if (await _context.SaveChangesAsync() == 2)
                return Ok("Product deleted");

            return InternalServerError();
        }
    }
}
