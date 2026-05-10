using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pera.Entity.Entities;

namespace Pera.DataAccess
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        // Parametreli Constructor (Dependency Injection için)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Parametresiz Constructor — EF migration araçları için gerekli
        public AppDbContext()
        {
        }

        // 3. Bağlantı Ayarı (OnConfiguring) - PostgreSQL fallback
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Pera;Username=postgres;Password=ayanoglu");
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