using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookStore_Technical_Investigation.Models;

namespace BookStore_Technical_Investigation.Data
{
    public class BookStore_Technical_InvestigationContext : DbContext
    {
        public BookStore_Technical_InvestigationContext (DbContextOptions<BookStore_Technical_InvestigationContext> options)
            : base(options)
        {
        }

        public DbSet<BookStore_Technical_Investigation.Models.BookModel> BookModel { get; set; }
    }
}
