﻿// <auto-generated />
using System;
using MarsRover.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MarsRover.Infrastructure.Migrations
{
    [DbContext(typeof(MarsRoverContext))]
    [Migration("20200920213923_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("MarsRover.Domain.DomainModels.Command", b =>
                {
                    b.Property<int>("EnumId")
                        .HasColumnName("Id")
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("character varying(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Creator")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Modifier")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(200)")
                        .HasMaxLength(200);

                    b.HasKey("EnumId");

                    b.ToTable("commands","marsrover");
                });

            modelBuilder.Entity("MarsRover.Domain.DomainModels.Direction", b =>
                {
                    b.Property<int>("EnumId")
                        .HasColumnName("Id")
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("character varying(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Creator")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Modifier")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(200)")
                        .HasMaxLength(200);

                    b.HasKey("EnumId");

                    b.ToTable("directions","marsrover");
                });

            modelBuilder.Entity("MarsRover.Domain.DomainModels.Plateau", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Creator")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Modifier")
                        .HasColumnType("text");

                    b.Property<int>("XCoordinate")
                        .HasColumnName("XCoordinate")
                        .HasColumnType("integer");

                    b.Property<int>("YCoordinate")
                        .HasColumnName("YCoordinate")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("plateaus","marsrover");
                });

            modelBuilder.Entity("MarsRover.Domain.DomainModels.Rover", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Creator")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Modifier")
                        .HasColumnType("text");

                    b.Property<int>("_directionId")
                        .HasColumnName("DirectionId")
                        .HasColumnType("integer");

                    b.Property<Guid>("_plateauId")
                        .HasColumnName("PlateauId")
                        .HasColumnType("uuid");

                    b.Property<int>("_xCoordinate")
                        .HasColumnName("XCoordinate")
                        .HasColumnType("integer");

                    b.Property<int>("_yCoordinate")
                        .HasColumnName("YCoordinate")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("_directionId");

                    b.HasIndex("_plateauId");

                    b.ToTable("rovers","marsrover");
                });

            modelBuilder.Entity("MarsRover.Domain.DomainModels.Rover", b =>
                {
                    b.HasOne("MarsRover.Domain.DomainModels.Direction", "Direction")
                        .WithMany()
                        .HasForeignKey("_directionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MarsRover.Domain.DomainModels.Plateau", "Plateau")
                        .WithMany()
                        .HasForeignKey("_plateauId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}