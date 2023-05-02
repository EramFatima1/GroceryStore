using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroceryStore.ViewModels;

namespace GroceryStore.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<tblUsers> tblUsers { get; set; }
        public DbSet<tblProducts> tblProducts { get; set; }
        public DbSet<tblAddress> tblAddress { get; set; }
        public DbSet<tblCart> tblCart { get; set; }
        public DbSet<tblOrder> tblOrder { get; set; }
        public DbSet<tblOrderDetails> tblOrderDetails { get; set; }
        public DbSet<tblCountry> tblCountry { get; set; }
        public DbSet<tblState> tblState { get; set; }
        public DbSet<tblCity> tblCity { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblOrderDetails>()
                .HasKey(a => new { a.OrderId, a.UserId, a.ProductId });
        }
    }
}
