﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReservationSystemMVC.Models;

namespace ReservationSystemMVC.Data
{
    public class ReservationSystemMVCContext : DbContext
    {
        public ReservationSystemMVCContext (DbContextOptions<ReservationSystemMVCContext> options)
            : base(options)
        {
        }

        public DbSet<ReservationSystemMVC.Models.Room> Room { get; set; } = default!;
        public DbSet<ReservationSystemMVC.Models.RoomType> RoomType { get; set; } = default!;
        public DbSet<ReservationSystemMVC.Models.RoomEquipment> RoomEquipment { get; set; } = default!;
        public DbSet<ReservationSystemMVC.Models.RoomRate> RoomRate { get; set; } = default!;
        public DbSet<ReservationSystemMVC.Models.RoomRateRebate> RoomRateRebate { get; set; } = default!;
        public DbSet<RoomRateRoomRateRebate> RoomRateRoomRateRebates { get; set; } = default!;
        public DbSet<RoomRoomEquipment> RoomRoomEquipments { get; set; } = default!;
        public DbSet<Season> Seasons { get; set; } = default!;
        public DbSet<SeasonDate> SeasonDates { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.RoomType) // Room has one RoomType
                .WithMany(t => t.Rooms) // RoomType has many Rooms
                .HasForeignKey(r => r.RoomTypeId); // Foreign key in Room pointing to RoomType

            modelBuilder.Entity<RoomRoomEquipment>()
                .HasKey(rre => new { rre.RoomId, rre.RoomEquipmentId });

            modelBuilder.Entity<RoomRoomEquipment>()
                .HasOne(rre => rre.Room)
                .WithMany(r => r.RoomRoomEquipments)
                .HasForeignKey(rre => rre.RoomId);

            modelBuilder.Entity<RoomRoomEquipment>()
                .HasOne(rre => rre.RoomEquipment)
                .WithMany(re => re.RoomRoomEquipments)
                .HasForeignKey(rre => rre.RoomEquipmentId);

            // Configure many-to-many relationship between RoomRate and RoomRateRebate
            modelBuilder.Entity<RoomRateRoomRateRebate>()
                .HasKey(rrrr => new { rrrr.RoomRateId, rrrr.RoomRateRebateId });

            modelBuilder.Entity<RoomRateRoomRateRebate>()
                .HasOne(rrrr => rrrr.RoomRate)
                .WithMany(rr => rr.RoomRateRoomRateRebates)
                .HasForeignKey(rrrr => rrrr.RoomRateId);

            modelBuilder.Entity<RoomRateRoomRateRebate>()
                .HasOne(rrrr => rrrr.RoomRateRebate)
                .WithMany(rrr => rrr.RoomRateRoomRateRebates)
                .HasForeignKey(rrrr => rrrr.RoomRateRebateId);
        }

    }
}
