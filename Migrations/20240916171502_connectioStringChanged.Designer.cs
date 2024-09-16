﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PingApp.Db;

#nullable disable

namespace PingApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240916171502_connectioStringChanged")]
    partial class connectioStringChanged
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("PingApp.Models.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("IpAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("IpString")
                        .HasColumnType("TEXT");

                    b.Property<int>("LastIpStatus")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastReplyDt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("PingApp.Models.PingResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BufferSizeReceived")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BufferSizeSent")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DeviceId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IpStatus")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ReplyDt")
                        .HasColumnType("TEXT");

                    b.Property<long?>("RoundTripTime")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TimeToLive")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("PingResults");
                });

            modelBuilder.Entity("PingApp.Models.PingResult", b =>
                {
                    b.HasOne("PingApp.Models.Device", "Device")
                        .WithMany("PingResults")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Device");
                });

            modelBuilder.Entity("PingApp.Models.Device", b =>
                {
                    b.Navigation("PingResults");
                });
#pragma warning restore 612, 618
        }
    }
}
