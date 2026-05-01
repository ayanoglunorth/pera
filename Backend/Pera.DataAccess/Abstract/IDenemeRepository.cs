using Pera.Entity;
using Pera.Entity.Entities;
using System.Collections.Generic;

namespace Pera.DataAccess.Abstract
{
    public interface IDenemeRepository
    {
        void DenemeEkle(Deneme deneme);
        void DenemeSonucEkle(DenemeSonuc sonuc);
        List<DenemeSonuc> GetSonuclarByOgrenci(string ogrenciId);
        DenemeSonuc GetDetay(int id);
        void Sil(int id);
        List<Deneme> GetAllDenemeler();
        List<DenemeSonuc> GetAllSonuclar();
        List<Ders> GetDersler();
        void Update(DenemeSonuc entity);
        void Add(DenemeSonuc entity);
        DenemeSonuc GetSonuc(int sinavId, string ogrenciId);
        List<DenemeSonuc> GetSonuclarBySinavId(int sinavId);


    }
}