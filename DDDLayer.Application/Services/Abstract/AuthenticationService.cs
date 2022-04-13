using DDDLayer.Application.DTOs;
using DDDLayer.Application.JWT;
using DDDLayer.Application.Services.Interfaces;
using DDDLayer.Domain.Entities;
using DDDLayer.Shared.Interfaces;
using DDDLayer.Shared.Uow;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDLayer.Application.Services.Abstract
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<RefreshToken> _userRefreshTokenService;

        public AuthenticationService(UserManager<User> userManager,IOptions<List<Client>> options,ITokenService tokenService,IUnitOfWork unitOfWork, IBaseRepository<RefreshToken> userRefreshTokenService)
        {
            _userManager = userManager;
            _clients = options.Value;
            _unitOfWork = unitOfWork;
            _userRefreshTokenService = userRefreshTokenService;
            _tokenService = tokenService;
        }

        public async Task<TokenDto> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto==null)
            {
                throw new ArgumentException(nameof(loginDto));
            }

            var user =await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return null;
            }

            if (!await _userManager.CheckPasswordAsync(user,loginDto.Password))
            {
                return null;
            }

            var token = _tokenService.CreateToken(user);
            var userRefreshToken = await _userRefreshTokenService.GetSingleAsync("Select * from RefreshToken where UserId=@UserId", new { UserId=user.Id});
            if (userRefreshToken == null)
            {
                await _userRefreshTokenService.AddAsync(new RefreshToken { UserId = user.Id, Code = token.RefreshToken, Expration = token.RefreshTokenExpiration });
            }

            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expration = token.RefreshTokenExpiration;


            }



            return token;

        }

        public  ClientTokenDto CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.ClientSecret == clientLoginDto.ClientSecret);

            if (client == null)
            {
                return null;
            }

            var token = _tokenService.CreateTokenByClient(client);

            return token;
        }

        public async Task<TokenDto> CreateTokenRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.GetSingleAsync("Select * from RefreshToken where code=@code ", new { code = refreshToken });

            if (existRefreshToken == null)
            {
                return null;
            }

            var user =await _userManager.FindByIdAsync(existRefreshToken.UserId);
            if (user==null)
            {
                return null;
            }

            var tokenDto =_tokenService.CreateToken(user);
            existRefreshToken.Code = tokenDto.RefreshToken;
            existRefreshToken.Expration = tokenDto.RefreshTokenExpiration;

            return tokenDto;

        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var existRefreshToken = _userRefreshTokenService.GetAllAsync().Result.Where(i => i.Code == refreshToken).FirstOrDefault();

            if (existRefreshToken==null)
            {
              await Task.FromResult<object>("");
            }

            await _unitOfWork.CommitAsync();

            await Task.FromResult<object>("");




        }
    }
}
