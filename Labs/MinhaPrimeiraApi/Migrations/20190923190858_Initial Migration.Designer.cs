﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MinhaPrimeiraApi.Model;

namespace MinhaPrimeiraApi.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20190923190858_Initial Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MinhaPrimeiraApi.Model.Fornecedor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Ativo");

                    b.Property<string>("Documento")
                        .IsRequired()
                        .HasMaxLength(14);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("TipoFornecedor");

                    b.HasKey("Id");

                    b.ToTable("Fornecedores");
                });
#pragma warning restore 612, 618
        }
    }
}
