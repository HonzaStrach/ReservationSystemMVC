using System;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.RoomType) // Room has one RoomType
                .WithMany(t => t.Rooms) // RoomType has many Rooms
                .HasForeignKey(r => r.RoomTypeId); // Foreign key in Room pointing to RoomType
        }

    }
}
