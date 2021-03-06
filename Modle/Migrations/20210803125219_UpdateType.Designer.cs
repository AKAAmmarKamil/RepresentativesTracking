// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Modle.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20210803125219_UpdateType")]
    partial class UpdateType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Modle.Model.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("ExchangeRate")
                        .HasColumnType("float");

                    b.Property<bool>("IsAcceptAutomaticCurrencyExchange")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RepresentativeCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("Modle.Model.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompanyID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyID");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Modle.Model.Order", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("AddOrderDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("CustomerID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("DeliveryOrderDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("EndLatitude")
                        .HasColumnType("float");

                    b.Property<double>("EndLongitude")
                        .HasColumnType("float");

                    b.Property<string>("ReceiptImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("StartLatitude")
                        .HasColumnType("float");

                    b.Property<double?>("StartLongitude")
                        .HasColumnType("float");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("UserID");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Modle.Model.Products", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OrderID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double?>("PriceInIQD")
                        .HasColumnType("float");

                    b.Property<double?>("PriceInUSD")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Modle.Model.RepresentativeLocation", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("bit");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<DateTimeOffset>("LocationDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<Guid?>("OrderID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("OrderID");

                    b.HasIndex("UserID");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("Modle.Model.User", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Activated")
                        .HasColumnType("bit");

                    b.Property<Guid?>("CompanyID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Type")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Modle.Model.Customer", b =>
                {
                    b.HasOne("Modle.Model.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Modle.Model.Order", b =>
                {
                    b.HasOne("Modle.Model.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Modle.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Modle.Model.Products", b =>
                {
                    b.HasOne("Modle.Model.Order", "Order")
                        .WithMany("Products")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Modle.Model.RepresentativeLocation", b =>
                {
                    b.HasOne("Modle.Model.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderID");

                    b.HasOne("Modle.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Modle.Model.User", b =>
                {
                    b.HasOne("Modle.Model.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Modle.Model.Order", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
