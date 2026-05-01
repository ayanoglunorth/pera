using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pera.DTO.DTOs
{
    public class DersNotlariDTO
    {
        public int DersId { get; set; }
        public string DersAdi {  get; set; }
        public List<SinavSonucDTO> Sinavlar { get; set; }
    }
    public class SinavSonucDTO
    {
        public string SinavAdi { get; set; }
        public string Tarih { get; set; }

        public decimal Puan {  get; set; }
    }
}
