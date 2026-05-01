using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pera.Business.Abstract;
using Pera.DataAccess.Abstract;
using Pera.DTO.DTOs;
using Pera.Entity;
using Pera.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pera.Business.Concrete
{
    public class DenemeService : IDenemeService
    {
        private readonly IDenemeRepository _denemeRepository;
        private readonly UserManager<AppUser> _userManager;

        public DenemeService(IDenemeRepository denemeRepository, UserManager<AppUser> userManager)
        {
            _denemeRepository = denemeRepository;
            _userManager = userManager;
        }

        // 1. SINAV TANIMLA
        public void DenemeTanimla(DenemeTanimlaDto model)
        {
            var deneme = new Deneme
            {
                Ad = model.Ad,
                Tarih = model.Tarih,
                Tur = model.Tur,
                DersId = model.DersId
            };
            _denemeRepository.DenemeEkle(deneme);
        }

        // 2. DETAYLI DENEME SONUCU GİRİŞİ (DOĞRU/YANLIŞ HESAPLAMALI)
        public void DenemeGirisiYap(DenemeEkleDto model)
        {
            var sonuc = new DenemeSonuc
            {
                DenemeId = model.DenemeId,
                OgrenciId = model.OgrenciId,
                Puan = 0
            };

            decimal toplamNet = 0;

            foreach (var ders in model.Dersler)
            {
                decimal net = ders.DogruSayisi - (ders.YanlisSayisi / 3.0m);
                toplamNet += net;

                switch (ders.DersAdi)
                {
                    case "Türkçe":
                        sonuc.TurkceDogru = ders.DogruSayisi; sonuc.TurkceYanlis = ders.YanlisSayisi; sonuc.TurkceNet = net; break;
                    case "Matematik":
                        sonuc.MatematikDogru = ders.DogruSayisi; sonuc.MatematikYanlis = ders.YanlisSayisi; sonuc.MatematikNet = net; break;
                    case "Fen Bilimleri":
                        sonuc.FenDogru = ders.DogruSayisi; sonuc.FenYanlis = ders.YanlisSayisi; sonuc.FenNet = net; break;
                    case "İnkılap Tarihi":
                        sonuc.InkilapDogru = ders.DogruSayisi; sonuc.InkilapYanlis = ders.YanlisSayisi; sonuc.InkilapNet = net; break;
                    case "Din Kültürü":
                        sonuc.DinDogru = ders.DogruSayisi; sonuc.DinYanlis = ders.YanlisSayisi; sonuc.DinNet = net; break;
                    case "İngilizce":
                        sonuc.IngilizceDogru = ders.DogruSayisi; sonuc.IngilizceYanlis = ders.YanlisSayisi; sonuc.IngilizceNet = net; break;
                }
            }

            sonuc.ToplamNet = toplamNet;
            sonuc.Puan = 194 + (toplamNet * 4.0m);

            _denemeRepository.DenemeSonucEkle(sonuc);
        }

        // 3. TANIMLARI GETİR
        public List<DenemeListeDto> GetDenemeTanimlari(string tur = null)
        {
            var denemeler = _denemeRepository.GetAllDenemeler();

            if (!string.IsNullOrEmpty(tur))
            {
                denemeler = denemeler.Where(x => x.Tur == tur).ToList();
            }

            return denemeler.Select(x => new DenemeListeDto
            {
                SinavId = x.Id,
                Ad = x.Ad,
                Tarih = x.Tarih
            }).ToList();
        }

        // 4. SONUÇLARI GETİR (ÖĞRENCİ İÇİN)
        public List<DenemeListeDto> GetSonuclarByOgrenci(string ogrenciId)
        {
            var sonuclar = _denemeRepository.GetSonuclarByOgrenci(ogrenciId);

            return sonuclar.Select(x => new DenemeListeDto
            {
                SinavId = x.Id,
                Ad = x.Deneme.Ad,
                Tarih = x.Deneme.Tarih,
                Puan = x.Puan,
                ToplamNet = x.ToplamNet,
                TurkceNet = x.TurkceNet,
                MatematikNet = x.MatematikNet,
                FenNet = x.FenNet,
                InkilapNet = x.InkilapNet,
                DinNet = x.DinNet,
                IngilizceNet = x.IngilizceNet
            }).ToList();
        }

        // 5. DETAY GÖSTER
        public DenemeDetayDto GetDenemeDetay(int sonucId, string userId, string userRole)
        {
            var sonuc = _denemeRepository.GetDetay(sonucId);

            if (sonuc == null) return null;

            // Güvenlik: Öğrenciyse sadece kendi sınavını görebilir
            if (sonuc.OgrenciId != userId && userRole != "Ogretmen")
            {
                return null;
            }

            return new DenemeDetayDto
            {
                SinavId = sonuc.Id,
                Ad = sonuc.Deneme.Ad,
                Tarih = sonuc.Deneme.Tarih,
                Puan = sonuc.Puan,
                ToplamNet = sonuc.ToplamNet,
                Dersler = new List<DersSonucDto>
                {
                    new DersSonucDto { DersAdi="Türkçe", DogruSayisi=sonuc.TurkceDogru, YanlisSayisi=sonuc.TurkceYanlis },
                    new DersSonucDto { DersAdi="Matematik", DogruSayisi=sonuc.MatematikDogru, YanlisSayisi=sonuc.MatematikYanlis },
                    new DersSonucDto { DersAdi="Fen Bilimleri", DogruSayisi=sonuc.FenDogru, YanlisSayisi=sonuc.FenYanlis },
                    new DersSonucDto { DersAdi="İnkılap Tarihi", DogruSayisi=sonuc.InkilapDogru, YanlisSayisi=sonuc.InkilapYanlis },
                    new DersSonucDto { DersAdi="Din Kültürü", DogruSayisi=sonuc.DinDogru, YanlisSayisi=sonuc.DinYanlis },
                    new DersSonucDto { DersAdi="İngilizce", DogruSayisi=sonuc.IngilizceDogru, YanlisSayisi=sonuc.IngilizceYanlis }
                }
            };
        }

        // 6. SİLME
        public void DenemeSil(int id)
        {
            _denemeRepository.Sil(id);
        }

        // 7. SINIF ORTALAMASI
        public List<DenemeListeDto> GetSinifOrtalamasi()
        {
            var tumSonuclar = _denemeRepository.GetAllSonuclar();
            var gruplanmis = tumSonuclar.GroupBy(x => x.DenemeId);
            var liste = new List<DenemeListeDto>();

            foreach (var grup in gruplanmis)
            {
                var sinavBilgisi = grup.First().Deneme;

                liste.Add(new DenemeListeDto
                {
                    SinavId = sinavBilgisi.Id,
                    Ad = sinavBilgisi.Ad + " (Sınıf Ort.)",
                    Tarih = sinavBilgisi.Tarih,
                    Puan = Math.Round(grup.Average(x => x.Puan ?? 0), 2),
                    ToplamNet = Math.Round(grup.Average(x => x.ToplamNet), 2),
                    TurkceNet = Math.Round(grup.Average(x => x.TurkceNet), 2),
                    MatematikNet = Math.Round(grup.Average(x => x.MatematikNet), 2),
                    FenNet = Math.Round(grup.Average(x => x.FenNet), 2),
                    InkilapNet = Math.Round(grup.Average(x => x.InkilapNet), 2),
                    DinNet = Math.Round(grup.Average(x => x.DinNet), 2),
                    IngilizceNet = Math.Round(grup.Average(x => x.IngilizceNet), 2)
                });
            }

            return liste.OrderByDescending(x => x.Tarih).ToList();
        }

        // 8. DERSLERİ GETİR
        public List<Ders> GetTumDersler()
        {
            return _denemeRepository.GetDersler();
        }

        // 9. YAZILI NOTU GİRME - ÖĞRENCİ LİSTESİ GETİR
        public async Task<List<SinavOgrenciListeDto>> GetSinavOgrenciListesiAsync(int sinavId)
        {
            var ogrenciler = await _userManager.GetUsersInRoleAsync("Ogrenci");
            var mevcutSonuclar = _denemeRepository.GetSonuclarBySinavId(sinavId);

            var liste = ogrenciler.Select(ogr => new SinavOgrenciListeDto
            {
                OgrenciId = ogr.Id,
                AdSoyad = ogr.UserName,

                // DÜZELTME BURADA YAPILDI: (int?) cast işlemi eklendi
                Puan = (int?)(mevcutSonuclar.FirstOrDefault(s => s.OgrenciId == ogr.Id)?.Puan)
            }).ToList();

            return liste;
        }

        // 10. YAZILI NOTU GİRME - TOPLU KAYDET
        public void TopluSonucKaydet(TopluSonucGirisDto model)
        {
            foreach (var kayit in model.Kayitlar)
            {
                var mevcutSonuc = _denemeRepository.GetSonuc(model.SinavId, kayit.OgrenciId);

                if (mevcutSonuc != null)
                {
                    mevcutSonuc.Puan = kayit.Puan;
                    _denemeRepository.Update(mevcutSonuc);
                }
                else
                {
                    // Yeni kayıt eklerken diğer alanları 0 ile dolduruyoruz ki hata almayalım
                    var yeniSonuc = new DenemeSonuc
                    {
                        DenemeId = model.SinavId,
                        OgrenciId = kayit.OgrenciId,
                        Puan = kayit.Puan,

                        ToplamNet = 0,
                        TurkceDogru = 0,
                        TurkceYanlis = 0,
                        TurkceNet = 0,
                        MatematikDogru = 0,
                        MatematikYanlis = 0,
                        MatematikNet = 0,
                        FenDogru = 0,
                        FenYanlis = 0,
                        FenNet = 0,
                        InkilapDogru = 0,
                        InkilapYanlis = 0,
                        InkilapNet = 0,
                        DinDogru = 0,
                        DinYanlis = 0,
                        DinNet = 0,
                        IngilizceDogru = 0,
                        IngilizceYanlis = 0,
                        IngilizceNet = 0
                    };
                    _denemeRepository.Add(yeniSonuc);
                }
            }
        }

        // BU METODU HATALI OLANIN YERİNE YAPIŞTIR
        public void DenemeSonucuEkle(DenemeSonucGirisDto model)
        {
            // 1. Önce bu öğrencinin bu sınavda kaydı var mı bakalım
            // (Repository'de GetSonuc metodun olduğunu varsayıyoruz, TopluSonucKaydet'te kullanmışsın)
            var sonuc = _denemeRepository.GetSonuc(model.SinavId, model.OgrenciId);

            bool yeniKayit = false;

            // Kayıt yoksa yeni oluştur
            if (sonuc == null)
            {
                yeniKayit = true;
                sonuc = new DenemeSonuc
                {
                    DenemeId = model.SinavId,
                    OgrenciId = model.OgrenciId,
                    Puan = 0
                };
            }

            decimal toplamNet = 0;

            // 2. Gelen listedeki dersleri tek tek senin tablo sütunlarına eşle
            foreach (var ders in model.Dersler)
            {
                // LGS Mantığı: 3 Yanlış 1 Doğruyu Götürür
                decimal net = ders.DogruSayisi - (ders.YanlisSayisi / 3.0m);
                toplamNet += net;

                // SENİN TABLO YAPINA GÖRE EŞLEŞTİRME
                // Frontend'den gelen ismin ("Turkce" veya "Türkçe") backend ile uyuşması lazım.
                // Burada contains veya tolower ile esneklik sağlıyoruz.
                var dersAdi = ders.DersAdi.ToLower();

                if (dersAdi.Contains("türkçe") || dersAdi.Contains("turkce"))
                {
                    sonuc.TurkceDogru = ders.DogruSayisi;
                    sonuc.TurkceYanlis = ders.YanlisSayisi;
                    sonuc.TurkceNet = net;
                }
                else if (dersAdi.Contains("matematik"))
                {
                    sonuc.MatematikDogru = ders.DogruSayisi;
                    sonuc.MatematikYanlis = ders.YanlisSayisi;
                    sonuc.MatematikNet = net;
                }
                else if (dersAdi.Contains("fen"))
                {
                    sonuc.FenDogru = ders.DogruSayisi;
                    sonuc.FenYanlis = ders.YanlisSayisi;
                    sonuc.FenNet = net;
                }
                else if (dersAdi.Contains("inkılap") || dersAdi.Contains("inkilap"))
                {
                    sonuc.InkilapDogru = ders.DogruSayisi;
                    sonuc.InkilapYanlis = ders.YanlisSayisi;
                    sonuc.InkilapNet = net;
                }
                else if (dersAdi.Contains("din"))
                {
                    sonuc.DinDogru = ders.DogruSayisi;
                    sonuc.DinYanlis = ders.YanlisSayisi;
                    sonuc.DinNet = net;
                }
                else if (dersAdi.Contains("ingilizce") || dersAdi.Contains("ing"))
                {
                    sonuc.IngilizceDogru = ders.DogruSayisi;
                    sonuc.IngilizceYanlis = ders.YanlisSayisi;
                    sonuc.IngilizceNet = net;
                }
            }

            // 3. Genel Toplamları Hesapla
            sonuc.ToplamNet = toplamNet;
            // Basit Puan Hesabı (Formülü kendine göre düzenleyebilirsin)
            sonuc.Puan = 194 + (toplamNet * 4.0m);

            // 4. Veritabanına İşle
            if (yeniKayit)
            {
                _denemeRepository.DenemeSonucEkle(sonuc);
            }
            else
            {
                // Eğer repository'inde Update metodu void ise direkt çağır, değilse ona göre ayarla.
                // TopluSonucKaydet metodunda _denemeRepository.Update(mevcutSonuc) kullanmışsın.
                _denemeRepository.Update(sonuc);
            }
        }
    }
}