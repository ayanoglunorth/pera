using Pera.Business.Abstract;
using Pera.DataAccess.Abstract;
using Pera.DTO.DTOs;
using Pera.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pera.Business.Concrete
{
    public class MesajService : IMesajService
    {
        private readonly IMesajRepository _mesajRepository;

        public MesajService(IMesajRepository mesajRepository)
        {
            _mesajRepository = mesajRepository;
        }

        public void MesajGonder(MesajGonderDto model, string gondericiId)
        {
            var mesaj = new Mesaj
            {
                GondericiId = gondericiId,
                AliciId = model.AliciId,
                Icerik = model.Icerik,
                Tarih = DateTime.Now,
                OkunduMu = false
            };
            _mesajRepository.MesajEkle(mesaj);
        }

        public List<MesajDetayDto> GetSohbetDetay(string benimId, string karsiId)
        {
            var mesajlar = _mesajRepository.GetSohbet(benimId, karsiId);

            // Eğer mesaj bana geldiyse ve okunmadıysa, "Okundu" yap
            foreach (var m in mesajlar)
            {
                if (m.AliciId == benimId && !m.OkunduMu)
                {
                    m.OkunduMu = true;
                    _mesajRepository.Guncelle(m);
                }
            }

            return mesajlar.Select(x => new MesajDetayDto
            {
                Id = x.Id,
                GondericiId = x.GondericiId,
                GondericiAd = x.Gonderici.UserName, // Veya Ad + Soyad
                Icerik = x.Icerik,
                Tarih = x.Tarih,
                BenMiGonderdim = (x.GondericiId == benimId),
                OkunduMu = x.OkunduMu
            }).ToList();
        }

        public List<SohbetKutusuDto> GetInbox(string userId)
        {
            // 1. Tüm mesajlarımı çek
            var tumMesajlar = _mesajRepository.GetTumMesajlarim(userId);

            // 2. Konuştuğum kişilere göre grupla
            // Mesajın diğer tarafı kim? (Eğer gönderen bensem alıcıdır, alıcı bensem gönderendir)
            var sohbetler = tumMesajlar
                .GroupBy(x => x.GondericiId == userId ? x.AliciId : x.GondericiId)
                .Select(grup => new
                {
                    KarsiTarafId = grup.Key,
                    // Gruptaki en son mesajı al
                    SonMesaj = grup.OrderByDescending(m => m.Tarih).First(),
                    // Bana gelip de okumadıklarımın sayısı
                    OkunmamisSayisi = grup.Count(m => m.AliciId == userId && !m.OkunduMu)
                })
                .ToList();

            // 3. DTO'ya çevir
            var sonuc = sohbetler.Select(s => new SohbetKutusuDto
            {
                KullaniciId = s.KarsiTarafId,

                // Karşı tarafın adını bulmamız lazım. Son mesajdan çekebiliriz.
                // Eğer son mesajı ben attıysam Alıcı'nın adı, o attıysa Gönderici'nin adı.
                AdSoyad = (s.SonMesaj.GondericiId == userId)
                          ? s.SonMesaj.Alici.UserName
                          : s.SonMesaj.Gonderici.UserName,

                SonMesaj = s.SonMesaj.Icerik,
                SonTarih = s.SonMesaj.Tarih,
                OkunmamisSayisi = s.OkunmamisSayisi
            })
            .OrderByDescending(x => x.SonTarih) // En son konuşulan en üstte
            .ToList();

            return sonuc;
        }
    }
}