
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Pera.Entity.Entities // Senin namespace yapın neyse onu koru
{
    public class Ders
    {
        // Eski 'DersId' yerine standart 'Id' kullanıyoruz
        public int Id { get; set; }

        public string Ad { get; set; }

        // Artık 'Sinav' değil 'Deneme' tablosuna bağlanacak.
        // Çünkü okul yazılılarını da 'Deneme' tablosunda (Tur="Yazili" olarak) tutuyoruz.
        public List<Deneme> Denemeler { get; set; }
    }
}
