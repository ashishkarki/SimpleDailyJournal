﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleDailyJournal.Data;

#nullable disable

namespace SimpleDailyJournal.Migrations
{
    [DbContext(typeof(JournalDbContext))]
    [Migration("20241107120341_UpdateMoodPropToEnum")]
    partial class UpdateMoodPropToEnum
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("SimpleDailyJournal.Models.JournalEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("Mood")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("JournalEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
