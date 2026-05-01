using Pera.DTO.DTOs;
using Pera.Entity.Entities;
using System.Collections.Generic;

namespace Pera.Business.Abstract
{
    public interface IDersService
    {
        // GÜNCELLEME: Metoda (string ogrenciId) parametresini ekledik.
        // Böylece servisten not isterken "Hangi öğrencinin notu?" diye sorabileceğiz.
        List<DersNotlariDTO> GetOgrenciDersNotlari(string ogrenciId);




    }


}