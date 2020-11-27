using BookApiService.Core.Models;
using BookApiService.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BookApiService.Data
{
    public class BookDBContext : DbContext
    {
        public BookDBContext(DbContextOptions<BookDBContext> options) : base(options)
        { }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new BookConfiguration());
        }
    }
}
