using Pera.Business.Abstract;       // Interface için
using Pera.DataAccess.Abstract;     // Repository Interface için
using Pera.DTO.DTOs;                // DTO'lar için
using System.Collections.Generic;
using System.Linq;                  // Select (Mapping) işlemi için

namespace Pera.Business.Concrete
{
    public class DersService : IDersService
    {
        private readonly IDersRepository _dersRepository;

        public DersService(IDersRepository dersRepository)
        {
            _dersRepository = dersRepository;
        }

        // DİKKAT: Metoda 'ogrenciId' parametresi ekledik. 
        // Çünkü hangi öğrencinin notunu çekeceğimizi bilmemiz lazım.
        public List<DersNotlariDTO> GetOgrenciDersNotlari(string ogrenciId)
        {
            // 1. Veritabanından Dersleri ve bağlı Denemelerini çek
            // (Repository'deki metodun adı buysa kalsın, değilse GetDerslerWithSinavlar vb. olabilir)
            var gelenVeri = _dersRepository.DersleriSinavlarlaGetir();

            // 2. Entity -> DTO Dönüştürme
            var sonucListesi = gelenVeri.Select(ders => new DersNotlariDTO
            {
                DersAdi = ders.Ad, // Entity'de 'Ad' demiştik

                // Dersin içindeki 'Denemeler' listesini dönüyoruz
                // SADECE 'Yazili' olanları filtreliyoruz ki Deneme Sınavları (LGS vb.) buraya karışmasın.
                Sinavlar = ders.Denemeler
                    .Where(x => x.Tur == "Yazili")
                    .Select(sinav => new SinavSonucDTO
                    {
                        // Entity'de 'Baslik' yok, 'Ad' var. Onu düzelttik.
                        SinavAdi = sinav.Ad,

                        Tarih = sinav.Tarih.ToString("dd.MM.yyyy"),

                        // KRİTİK NOKTA: O sınava ait sonuçlar içinden, 
                        // SADECE bizim öğrencinin (ogrenciId) sonucunu buluyoruz.
                        // ...
                        Puan = sinav.Sonuclar
                           .Where(s => s.OgrenciId == ogrenciId)
                           .Select(s => (decimal?)s.Puan) // Önce nullable çeviriyoruz ki hata çıkmasın
                           .FirstOrDefault() ?? 0 // EĞER NULL GELİRSE 0 YAZ (Sihirli kod bu)                                                 // ...
                    }).ToList()

            }).ToList();

            return sonucListesi;
        }
    }
}