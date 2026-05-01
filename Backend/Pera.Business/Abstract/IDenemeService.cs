using Pera.DTO.DTOs;
using Pera.Entity.Entities;
using System.Collections.Generic;

namespace Pera.Business.Abstract
{
    public interface IDenemeService
    {
        // 1. Sınav Tanımlama (Admin/Öğretmen)
        void DenemeTanimla(DenemeTanimlaDto model);

        // 2. Sonuç Girişi
        void DenemeGirisiYap(DenemeEkleDto model);

        // 3. Sınav Seçimi İçin Liste (Dropdown)
        List<DenemeListeDto> GetDenemeTanimlari(string tur = null);

        // 4. Öğrencinin Sonuçlarını Listeleme
        List<DenemeListeDto> GetSonuclarByOgrenci(string ogrenciId);

        // 5. Detay ve Silme
        DenemeDetayDto GetDenemeDetay(int sonucId, string userId, string userRole);
        void DenemeSil(int id);

        List<DenemeListeDto> GetSinifOrtalamasi();

        List<Ders> GetTumDersler();

        // 1. Öğrenci Listesi Getir (Async çünkü UserManager kullanıyoruz)
        Task<List<SinavOgrenciListeDto>> GetSinavOgrenciListesiAsync(int sinavId);

        // 2. Toplu Kaydet
        void TopluSonucKaydet(TopluSonucGirisDto model);

        void DenemeSonucuEkle(DenemeSonucGirisDto model);
    }
}