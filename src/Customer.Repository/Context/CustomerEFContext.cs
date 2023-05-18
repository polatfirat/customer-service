using Customer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Repository.Context
{
    public class CustomerEFContext : DbContext
    {
        public CustomerEFContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Customers> Customers { get; set; }
    }
}
