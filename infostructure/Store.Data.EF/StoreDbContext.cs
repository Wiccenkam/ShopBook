using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using ShopBook.Data;
using System;
using System.Collections.Generic;
using System.Linq;


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
            BuildBooks(modelBuilder);
            BuildOrders(modelBuilder);
            BuildOrderItems(modelBuilder);
        }

        private void BuildOrderItems(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItemDto>(action =>
            {
                action.Property(dto => dto.Price)
                                .HasColumnType("money");
                action.HasOne(dto => dto.Order)
                                .WithMany(dto => dto.ItemsDtos)
                                .IsRequired();

            });
        }

        private void BuildOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDto>(action =>
            {
                action.Property(dto => dto.CellPhone)
                                    .HasMaxLength(20);
                action.Property(dto => dto.DeliveryUniqueCode)
                                    .HasMaxLength(40);
                action.Property(dto => dto.DeliveryPrice)
                                    .HasColumnType("money");
                action.Property(dto => dto.PaymentServiceName)
                                    .HasMaxLength(40);
                action.Property(dto => dto.DeliveryParameters)
                                    .HasConversion(value => JsonConvert.SerializeObject(value),
                value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                                    .Metadata.SetValueComparer(DictionaryComparer);
                
                action.Property(dto => dto.PaymentParameters)
                                    .HasConversion(value => JsonConvert.SerializeObject(value),
                value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                                    .Metadata.SetValueComparer(DictionaryComparer);
                

            }
            );
        }
        private static readonly ValueComparer DictionaryComparer =
            new ValueComparer<Dictionary<string, string>>((dictionary1, dictionary2) =>
           dictionary1.SequenceEqual(dictionary2),
            dictionary => dictionary.Aggregate(
                0,
                (a, p) => HashCode.Combine(HashCode.Combine(a, p.Key.GetHashCode()),p.Value.GetHashCode()))
        );

        private static void BuildBooks(ModelBuilder modelBuilder)
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
                        Isbn = "ISBN0201896831",
                        Author = "D.Knuth",
                        Title = "Art of Programin, Vol.1",
                        Description = "This first volume in the series begins with basic programming concepts and techniques, then focuses more particularly on information structures–the representation of information inside a computer, the structural relationships between data elements and how to deal with them efficiently. Elementary applications are given to simulation, numerical methods, symbolic computing, software and system design. Dozens of simple and important algorithms and techniques have been added to those of the previous edition. The section on mathematical preliminaries has been extensively revised to match present trends in research.",
                        Price = 24.59m
                    },
                    new BookDto
                    {
                        Id = 2,
                        Isbn = "ISBN0134757599",
                        Author = "Martin Fowler",
                        Title = "Refactoring",
                        Description = "For more than twenty years, experienced programmers worldwide have relied on Martin Fowler’s Refactoring to improve the design of existing code and to enhance software maintainability, as well as to make existing code easier to understand..This eagerly awaited new edition has been fully updated to reflect crucial changes in the programming landscape. Refactoring, Second Edition, features an updated catalog of refactoring's and includes JavaScript code examples, as well as new functional examples that demonstrate refactoring without classes.",
                        Price = 30.59m
                    },
                    new BookDto
                    {
                        Id = 3,
                        Isbn = "ISBN5496004330",
                        Author = "Jeffrey Richter",
                        Title = "CLR via C#",
                        Description = "Dig deep and master the intricacies of the common language runtime, C#, and .NET development. Led by programming expert Jeffrey Richter, a longtime consultant to the Microsoft .NET team - you’ll gain pragmatic insights for building robust, reliable, and responsive apps and components.",
                        Price = 34.59m
                    }
                    );
            }
                        );
        }
    }
}
