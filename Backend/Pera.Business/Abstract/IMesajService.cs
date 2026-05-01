using Pera.DTO.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pera.Business.Abstract
{
    public interface IMesajService
    {
        void MesajGonder(MesajGonderDto model, string gondericiId);
        List<MesajDetayDto> GetSohbetDetay(string benimId, string karsiId);
        List<SohbetKutusuDto> GetInbox(string userId);
    }
}