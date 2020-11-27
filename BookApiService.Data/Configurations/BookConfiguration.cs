using BookApiService.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookApiService.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder
                .HasKey(b => b.Id);

            builder
                .Property(b => b.Id)
                .UseIdentityColumn();

            builder
                .Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(b => b.Author)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .ToTable("Books");
        }
    }
}
