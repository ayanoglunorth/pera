using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pera.Entity.Entities
{
    // IdentityUser sınıfından miras alıyoruz.
    // <int> diyerek ID'lerin sayı (1, 2, 3) olmasını sağlıyoruz. (Yoksa karmaşık harfler olur)
    public class AppUser : IdentityUser
    {
        // Identity bize Username, Email, Password, PhoneNumber gibi alanları zaten veriyor.
        // Biz sadece EKSTRA istediklerimizi buraya yazıyoruz:

        public string Ad { get; set; }
        public string Soyad { get; set; }

        // Senin arayüzde solda resim vardı, onu da ekleyelim:
        public string? ProfilResmi { get; set; }
    }
}
