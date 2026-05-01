using Microsoft.EntityFrameworkCore;
using Pera.DataAccess.Abstract;
using Pera.Entity;
using Pera.Entity.Entities;
using System.Collections.Generic;
using System.Linq;
using Pera.DataAccess;

namespace Pera.DataAccess.Concrete
{
    public class DenemeRepository : IDenemeRepository
    {
        private readonly AppDbContext _context;

        public DenemeRepository(AppDbContext context)
        {
            _context = context;
        }

        // 1. Sınavın Kendisini Ekleyen Metot (EKSİK OLAN BUYDU)
        public void DenemeEkle(Deneme deneme)
        {
            _context.Denemeler.Add(deneme); // Eğer hata verirse _context.Set<Deneme>().Add(deneme); yap
            _context.SaveChanges();
        }

        // 2. Öğrencinin Sonucunu Ekleyen Metot
        public void DenemeSonucEkle(DenemeSonuc denemeSonuc)
        {
            _context.DenemeSonuclar.Add(denemeSonuc);
            _context.SaveChanges();
        }

        // 3. Öğrencinin Sınavlarını Listeleyen Metot
        public List<DenemeSonuc> GetSonuclarByOgrenci(string ogrenciId)
        {
            return _context.DenemeSonuclar
                .Include(x => x.Deneme)
                .Where(x => x.OgrenciId == ogrenciId)
                .OrderByDescending(x => x.Deneme.Tarih)
                .ToList();
        }

        // 4. Detay Getiren Metot
        public DenemeSonuc GetDetay(int id)
        {
            return _context.DenemeSonuclar
                .Include(x => x.Deneme)
                .FirstOrDefault(x => x.Id == id);
        }

        public void Sil(int id)
        {
            var kayit = _context.DenemeSonuclar.Find(id);
            if (kayit != null)
            {
                _context.DenemeSonuclar.Remove(kayit);
                _context.SaveChanges();
            }
        }

        public List<Ders> GetDersler()
        {
            using (var context = new AppDbContext()) // Context ismin PeraContext ise
            {
                return context.Dersler.ToList();
            }
        }

        public List<Deneme> GetAllDenemeler()
        {
            return _context.Denemeler.OrderByDescending(x => x.Tarih).ToList();
        }

        public List<DenemeSonuc> GetAllSonuclar()
        {
            return _context.DenemeSonuclar
                .Include(x => x.Deneme) // Sınav adını almak için şart
                .ToList();
        }

        // 1. Sınava ait tüm sonuçları getir
        public List<DenemeSonuc> GetSonuclarBySinavId(int sinavId)
        {
            using (var context = new AppDbContext())
            {
                return context.DenemeSonuclar.Where(x => x.DenemeId == sinavId).ToList();
            }
        }

        // 2. Tek bir öğrencinin sonucunu bul
        public DenemeSonuc GetSonuc(int sinavId, string ogrenciId)
        {
            using (var context = new AppDbContext())
            {
                return context.DenemeSonuclar
                              .FirstOrDefault(x => x.DenemeId == sinavId && x.OgrenciId == ogrenciId);
            }
        }

        // 3. Ekleme ve Güncelleme (GenericRepository kullanıyorsan zaten vardır, yoksa ekle)
        public void Add(DenemeSonuc entity)
        {
            using (var context = new AppDbContext())
            {
                context.DenemeSonuclar.Add(entity);
                context.SaveChanges();
            }
        }

        public void Update(DenemeSonuc entity)
        {
            using (var context = new AppDbContext())
            {
                context.DenemeSonuclar.Update(entity);
                context.SaveChanges();
            }
        }
    }

}