using Pera.Entity.Entities;
using System.Collections.Generic;

namespace Pera.DataAccess.Abstract
{
    public interface IMesajRepository
    {
        void MesajEkle(Mesaj mesaj);

        // İki kişi arasındaki tüm konuşmayı getir
        List<Mesaj> GetSohbet(string user1Id, string user2Id);

        // Bir kullanıcının dahil olduğu TÜM mesajları getir (Inbox yapmak için lazım)
        List<Mesaj> GetTumMesajlarim(string userId);

        // Mesajı okundu olarak işaretle
        Mesaj GetMesajById(int id);
        void Guncelle(Mesaj mesaj);
    }
}