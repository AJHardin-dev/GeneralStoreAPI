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
        private readonly ProductDbContext _context = new ProductDbContext();

        // api/Transaction
        [HttpPost]
        public async Task<IHttpActionResult> PostTransaction([FromBody] Transaction model)
        {
            if (model == null)
                return BadRequest("Can't be null");
            
            else if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            else
            { 
                Product product = await _context.Products.FindAsync(model.ProductSku);

                if (product == null)
                    return BadRequest("Could not find product sku for this transaction");
                
                else if (product.IsInStock == false)
                    return BadRequest("Product out of stock");
               
                else if (!product.CheckStock(model.ItemCount))
                    return BadRequest("Not enough stock to complete transaction");
                
                else
                { 
                    _context.Transactions.Add(model);
                    product.NumberInInventory -= model.ItemCount;
                    await _context.SaveChangesAsync();
                    return Ok("Success");
                }
            }
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
            Customer customer = await _context.Customers.FindAsync(customerId);
            
            if (customer == null)
                return NotFound();
            else
            {
                List<Transaction> transactions = await _context.Transactions.ToListAsync();
                List<Transaction> customerTransactions = new List<Transaction>{};
                foreach (Transaction tx in transactions)
                {
                    if (tx.CustomerID == customerId)
                        customerTransactions.Add(tx);
                }
                return Ok(customerTransactions);
            }
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

            Product product = await _context.Products.FindAsync(transaction.ProductSku);

            product.NumberInInventory += transaction.ItemCount;
            _context.Transactions.Remove(transaction);

            if (await _context.SaveChangesAsync() == 2)
                return Ok("Transaction deleted");

            return InternalServerError();
        }
    }
}
