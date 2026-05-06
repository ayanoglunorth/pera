using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pera.Entity;
using Pera.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pera.DataAccess
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        // 1. Parametreli Constructor (Dependency Injection için)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 2. Parametresiz (Boş) Constructor (Repository'de new'lemek için)
        public AppDbContext()
        {
        }

        // 3. Bağlantı Ayarı (OnConfiguring) - İŞTE EKSİK OLAN BU!
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Eğer dışarıdan bir ayar verilmediyse (ki new AppDbContext() dediğimizde verilmiyor),
            // buradaki ayarı kullan.
            if (!optionsBuilder.IsConfigured)
            {
                // DİKKAT: Buraya kendi connection string'ini yazmalısın.
                // Genelde şöyledir: "Server=.;Database=PeraDb;Trusted_Connection=True;MultipleActiveResultSets=true"
                // Veya "Server=(localdb)\\mssqllocaldb;Database=PeraDb;..." olabilir.
                // appsettings.json dosyanı kontrol et, oradakiyle aynı olsun.

                optionsBuilder.UseSqlServer("Server=.;Database=PeraDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        // --- Tables ---
        public DbSet<Course> Courses { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UploadedResult> UploadedResults { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Required if using Identity!

            // MESSAGING RELATIONSHIP SETTINGS
            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany() // A user can have many sent messages
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict); // Don't delete messages if user deleted (avoid errors)

            builder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany() // A user can have many received messages
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}