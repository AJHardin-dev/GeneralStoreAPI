using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GeneralStoreAPI.Models
{
    public class TransactionDbContext : DbContext
    {
        public TransactionDbContext() : base("DefaultConnection")
        {

        }

        public DbSet<Transaction> Transactions { get; set; }
    }
}