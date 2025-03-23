using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Domain.Entities;

namespace BlogApp.Application.Helpers.TokenHelper
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(Users user);
        Task<string> GenerateRefreshToken();
        Task<ClaimsPrincipal?> GetTokenPrincipal(string token);
    }
}
