﻿// <auto-generated />
using System;
using AlzaBox.API.WebExample.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AlzaBox.API.WebExample.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221103152641_webhookheaders")]
    partial class webhookheaders
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("AlzaBox.API.WebExample.Data.__EFMigrationsHistory", b =>
                {
                    b.Property<string>("MigrationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductVersion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("MigrationId");

                    b.ToTable("__EFMigrationsHistory");
                });

            modelBuilder.Entity("AlzaBox.API.WebExample.Data.ChangeStatusRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RequestBody")
                        .HasColumnType("TEXT");

                    b.Property<string>("RequestHeader")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ChangeStatusRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
