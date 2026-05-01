using System.Collections.Generic;
using Pera.Entity.Entities;

namespace Pera.DataAccess.Abstract
{
    public interface IDersRepository
    {
        // Sadece ihtiyacımız olanı tutalım, kafa karışıklığı olmasın
        List<Ders> DersleriSinavlarlaGetir();
    }
}