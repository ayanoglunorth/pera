using Microsoft.EntityFrameworkCore;
using Pera.DataAccess.Abstract;
using Pera.Entity.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Pera.DataAccess.Concrete.EntityFramework
{
    public class MesajRepository : IMesajRepository
    {
        public void MesajEkle(Mesaj mesaj)
        {
            using (var context = new AppDbContext())
            {
                context.Mesajlar.Add(mesaj);
                context.SaveChanges();
            }
        }

        public List<Mesaj> GetSohbet(string user1Id, string user2Id)
        {
            using (var context = new AppDbContext())
            {
                // İki kişi arasındaki mesajlaşma (Ben ona attım VEYA O bana attı)
                return context.Mesajlar
                    .Include(x => x.Gonderici) // İsimleri alabilmek için
                    .Include(x => x.Alici)
                    .Where(x => (x.GondericiId == user1Id && x.AliciId == user2Id) ||
                                (x.GondericiId == user2Id && x.AliciId == user1Id))
                    .OrderBy(x => x.Tarih) // Eskiden yeniye sırala
                    .ToList();
            }
        }

        public List<Mesaj> GetTumMesajlarim(string userId)
        {
            using (var context = new AppDbContext())
            {
                // Benim gönderdiğim veya aldığım her şeyi getir
                return context.Mesajlar
                    .Include(x => x.Gonderici)
                    .Include(x => x.Alici)
                    .Where(x => x.GondericiId == userId || x.AliciId == userId)
                    .OrderByDescending(x => x.Tarih) // En yeniler üstte
                    .ToList();
            }
        }

        public Mesaj GetMesajById(int id)
        {
            using (var context = new AppDbContext())
            {
                return context.Mesajlar.FirstOrDefault(x => x.Id == id);
            }
        }

        public void Guncelle(Mesaj mesaj)
        {
            using (var context = new AppDbContext())
            {
                context.Mesajlar.Update(mesaj);
                context.SaveChanges();
            }
        }
    }
}