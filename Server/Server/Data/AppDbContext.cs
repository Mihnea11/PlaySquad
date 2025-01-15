using Microsoft.EntityFrameworkCore;
using Server.Models.Entities;

namespace Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<SoccerField> SoccerFields { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User-Role relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();

            // SoccerField-Owner relationship
            modelBuilder.Entity<SoccerField>()
                .HasOne(sf => sf.Owner)
                .WithMany(u => u.OwnedFields)
                .HasForeignKey(sf => sf.OwnerId)
                .IsRequired();

            // Booking-SoccerField relationship
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Field)
                .WithMany(sf => sf.Bookings)
                .HasForeignKey(b => b.FieldId)
                .IsRequired();

            // Booking-Creator relationship
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Creator)
                .WithMany(u => u.OwnedBookings)
                .HasForeignKey(b => b.CreatorId)
                .IsRequired();

            // Booking-WaitingList relationship
            modelBuilder.Entity<Booking>()
                .HasMany(b => b.WaitingList)
                .WithMany(u => u.RequestedBookings);

            // Booking-ApprovedParticipants relationship
            modelBuilder.Entity<Booking>()
                .HasMany(b => b.ApprovedParticipants)
                .WithMany(u => u.ApprovedBookings);
        }
    }
}

