﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Order.API.Infrastructure.Migrations
{
    [DbContext(typeof(OrderDbContext))]
    [Migration("20240319094803_InitialOrderDb")]
    partial class InitialOrderDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Order.API.Entities.BuyerOrder", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("Order.API.Entities.BuyerOrder", b =>
                {
                    b.OwnsMany("Order.API.Entities.OrderItem", "Items", b1 =>
                        {
                            b1.Property<string>("OrderId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<string>("PictureUrl")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<double>("Price")
                                .HasColumnType("float");

                            b1.Property<string>("ProductId")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("ProductName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("Quantity")
                                .HasColumnType("int");

                            b1.Property<int>("Status")
                                .HasColumnType("int");

                            b1.HasKey("OrderId", "Id");

                            b1.ToTable("OrderItem");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
