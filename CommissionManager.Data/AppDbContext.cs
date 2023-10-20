using CommissionManager.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<Customer> Customers { get; set; }


        //Add option save Database to a custom location such as Onedrive for backup.

        public string DbPath { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            var folder = "\"C:\\Users\\charl\\OneDrive\\ArtistDatabase\"";
            
            DbPath = System.IO.Path.Join(folder, ".db");
        }

        

    }
}
