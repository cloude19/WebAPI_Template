using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Pratice.Model;
using Microsoft.EntityFrameworkCore;

namespace API_Pratice.Data
{
    public class QuotesDbContext : DbContext //class is intended to connect with entity/Database
    {
        /*construstor needs to be here as the AddDbContext in the startup.cs needs to pass
         the options to the base DbContext*/
        public QuotesDbContext(DbContextOptions<QuotesDbContext> option): base(option)
        {

        }
        public DbSet<Quote> Quotes { get; set; }
    }
}
