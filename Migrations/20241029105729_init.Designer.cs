﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using _3_Calculator;

#nullable disable

namespace _3_Calculator.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20241029105729_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.35")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("_3_Calculator.Entities.CalculationResult", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Expression")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<double>("Result")
                        .HasColumnType("double");

                    b.HasKey("ID");

                    b.HasIndex("Expression");

                    b.ToTable("CalculationResults");
                });
#pragma warning restore 612, 618
        }
    }
}