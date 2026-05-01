using Microsoft.EntityFrameworkCore; // Include ve ThenInclude için şart
using Pera.DataAccess.Abstract;      // Interface'i bulması için
using Pera.Entity.Entities;          // Ders entity'si için
using Pera.DataAccess;               // AppDbContext'i bulması için
using System.Collections.Generic;
using System.Linq;

namespace Pera.DataAccess.Concrete.EntityFramework // Namespace'i projene göre ayarla
{
    public class DersRepository : IDersRepository
    {
        // Constructor (Yapıcı Metot) ve _context değişkenini SİLDİK.
        // Sebebi: AppDbContext'i manuel (new) ile oluşturacağız.

        public List<Ders> DersleriSinavlarlaGetir()
        {
            // DİKKAT: Burada 'using' bloğu içinde yeni bir context oluşturuyoruz.
            // Bu yöntemle "Dependency Injection" hatası alma ihtimalini sıfıra indiriyoruz.
            using (var context = new AppDbContext())
            {
                return context.Dersler
                    .Include(d => d.Denemeler)       // Dersi ve Sınavlarını getir
                    .ThenInclude(s => s.Sonuclar)    // Sınavların sonuçlarını da getir
                    .ToList();
            }
        }
    }
}