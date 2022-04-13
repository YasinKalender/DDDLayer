using DDDLayer.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDLayer.Application.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokenDto> CreateTokenAsync(LoginDto loginDto);
        Task<TokenDto> CreateTokenRefreshToken(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
        ClientTokenDto CreateTokenByClient(ClientLoginDto clientLoginDto);
    }
}
