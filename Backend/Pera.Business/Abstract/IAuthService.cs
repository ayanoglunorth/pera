using Pera.DTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Pera.Business.Abstract
{
    public interface IAuthService
    {
        // Kayıt olma metodu (Geriye hata var mı yok mu döneriz)
        Task<bool> RegisterAsync(RegisterDto model);

        Task<string> LoginAsync(LoginDto model);
        Task<List<StudentSelectionDto>> GetStudentsAsync();
    }
}