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

        // --- Tabloların ---
        public DbSet<Ders> Dersler { get; set; }
        public DbSet<Deneme> Denemeler { get; set; }
        public DbSet<DenemeSonuc> DenemeSonuclar { get; set; }
        public DbSet<Mesaj> Mesajlar { get; set; }

        // Eski tablolar (Eğer bunları artık kullanmıyorsan silebilirsin, hata vermez ama temizlik olur)
        // public DbSet<Sinav> Sinavlar { get; set; } 
        // public DbSet<SinavSonuc> SinavSonuclar { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Identity kullanıyorsan bu satır şart!

            // MESAJLAŞMA İLİŞKİSİ AYARLARI
            builder.Entity<Mesaj>()
                .HasOne(m => m.Gonderici)
                .WithMany() // Bir kullanıcının birden çok gönderdiği mesaj olabilir
                .HasForeignKey(m => m.GondericiId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinirse mesajları silme (Hata vermesin)

            builder.Entity<Mesaj>()
                .HasOne(m => m.Alici)
                .WithMany() // Bir kullanıcının birden çok aldığı mesaj olabilir
                .HasForeignKey(m => m.AliciId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}