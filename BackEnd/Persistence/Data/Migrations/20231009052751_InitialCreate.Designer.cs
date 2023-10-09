﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

#nullable disable

namespace Persistence.Data.Migrations
{
    [DbContext(typeof(ApiContext))]
    [Migration("20231009052751_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Domain.Entities.FileUpload", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar")
                        .HasColumnName("extension");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar")
                        .HasColumnName("name");

                    b.Property<string>("Route")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar")
                        .HasColumnName("route");

                    b.Property<double>("Size")
                        .HasColumnType("double")
                        .HasColumnName("size");

                    b.Property<int>("TypeFileFk")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TypeFileFk");

                    b.ToTable("FileUpload", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.TypeFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar")
                        .HasColumnName("description");

                    b.HasKey("Id");

                    b.ToTable("typeFile", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.FileUpload", b =>
                {
                    b.HasOne("Domain.Entities.TypeFile", "TypeFile")
                        .WithMany("FileUploads")
                        .HasForeignKey("TypeFileFk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TypeFile");
                });

            modelBuilder.Entity("Domain.Entities.TypeFile", b =>
                {
                    b.Navigation("FileUploads");
                });
#pragma warning restore 612, 618
        }
    }
}
