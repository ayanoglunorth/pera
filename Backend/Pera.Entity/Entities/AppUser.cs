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
        // Identity already gives us Username, Email, Password, PhoneNumber etc.
        // We only add extra fields we need:

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? ProfilePicture { get; set; }
    }
}
