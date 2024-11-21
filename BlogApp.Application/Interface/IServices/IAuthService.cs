using BlogApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interface.IServices
{
    public interface IAuthService
    {
        Task<string> RegisterUser(RegisterDTO registerDto);
        Task<string> LoginUser(LoginDTO loginDto);
    }
}
