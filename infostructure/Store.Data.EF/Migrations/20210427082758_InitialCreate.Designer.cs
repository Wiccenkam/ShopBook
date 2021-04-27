﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Store.Data.EF;

namespace Store.Data.EF.Migrations
{
    [DbContext(typeof(StoreDbContext))]
    [Migration("20210427082758_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ShopBook.Data.BookDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Isbn")
                        .IsRequired()
                        .HasColumnType("nvarchar(17)")
                        .HasMaxLength(17);

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Author = "D.Knuth",
                            Description = "This first volume in the series begins with basic programming concepts and techniques, then focuses more particularly on information structures–the representation of information inside a computer, the structural relationships between data elements and how to deal with them efficiently. Elementary applications are given to simulation, numerical methods, symbolic computing, software and system design. Dozens of simple and important algorithms and techniques have been added to those of the previous edition. The section on mathematical preliminaries has been extensively revised to match present trends in research.",
                            Isbn = "ISBN0201896831",
                            Price = 24.59m,
                            Title = "Art of Programin, Vol.1"
                        },
                        new
                        {
                            Id = 2,
                            Author = "Martin Fowler",
                            Description = "For more than twenty years, experienced programmers worldwide have relied on Martin Fowler’s Refactoring to improve the design of existing code and to enhance software maintainability, as well as to make existing code easier to understand..This eagerly awaited new edition has been fully updated to reflect crucial changes in the programming landscape. Refactoring, Second Edition, features an updated catalog of refactoring's and includes JavaScript code examples, as well as new functional examples that demonstrate refactoring without classes.",
                            Isbn = "ISBN0134757599",
                            Price = 30.59m,
                            Title = "Refactoring"
                        },
                        new
                        {
                            Id = 3,
                            Author = "Jeffrey Richter",
                            Description = "Dig deep and master the intricacies of the common language runtime, C#, and .NET development. Led by programming expert Jeffrey Richter, a longtime consultant to the Microsoft .NET team - you’ll gain pragmatic insights for building robust, reliable, and responsive apps and components.",
                            Isbn = "ISBN5496004330",
                            Price = 34.59m,
                            Title = "CLR via C#"
                        });
                });

            modelBuilder.Entity("ShopBook.Data.OrderDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CellPhone")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("DeliveryDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeliveryParameters")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DeliveryPrice")
                        .HasColumnType("money");

                    b.Property<string>("DeliveryUniqueCode")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("PaymentDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentParameters")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentServiceName")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ShopBook.Data.OrderItemDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("ShopBook.Data.OrderItemDto", b =>
                {
                    b.HasOne("ShopBook.Data.OrderDto", "Order")
                        .WithMany("ItemsDtos")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
