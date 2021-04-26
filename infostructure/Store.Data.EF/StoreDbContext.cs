using Microsoft.EntityFrameworkCore;
using ShopBook.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Data.EF
{
    public class StoreDbContext : DbContext
    {
        public DbSet<BookDto> Books { get; set; }
        public DbSet<OrderDto> Orders { get; set; }
        public DbSet<OrderItemDto> OrderItems { get; set; }

        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookDto>(action =>
            {
                action.Property(dto => dto.Isbn)
                .HasMaxLength(17)
                .IsRequired();

                action.Property(dto => dto.Title)
                .IsRequired();

                action.Property(dto => dto.Price)
                .HasColumnType("money");

                action.HasData(
                    new BookDto
                    {
                        Id = 1,
                        Isbn =
                    })
            }
            );
        }
    }
}
