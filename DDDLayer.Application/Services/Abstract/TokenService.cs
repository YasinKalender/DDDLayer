using DDDLayer.Application.DTOs;
using DDDLayer.Application.JWT;
using DDDLayer.Application.Utilities;
using DDDLayer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DDDLayer.Application.Services.Abstract
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly CustomTokenOption _customTokenOption;

        public TokenService(UserManager<User> userManager, IOptions<CustomTokenOption> options)
        {
            _userManager = userManager;
            _customTokenOption = options.Value;
        }

        public TokenDto CreateToken(User user)
        {
            var accessTokenExpration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration);
            var refreshTokenExpration = DateTime.Now.AddMinutes(_customTokenOption.RefreshTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_customTokenOption.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _customTokenOption.Issuer,
                 //notBefore: DateTime.Now,
                expires: accessTokenExpration,
                 claims: GetClaims(user, _customTokenOption.Audience),
                 signingCredentials: signingCredentials);
            
            
            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpration,
                RefreshTokenExpiration = refreshTokenExpration

            };

            return tokenDto;


        }

        public ClientTokenDto CreateTokenByClient(Client clientDto)
        {
            var accessTokenExpration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_customTokenOption.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _customTokenOption.Issuer,
                expires: accessTokenExpration,
                notBefore: DateTime.Now,
                claims: GetClaimsByClient(clientDto),
                signingCredentials: signingCredentials
                );

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new ClientTokenDto
            {
                AccessToken = token,
                AccessTokenExpration = accessTokenExpration,


            };
            return tokenDto;

        }

        private IEnumerable<Claim> GetClaims(User user, List<string> audience)
        {
            var userList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.Name,user.FirstName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),


            };

            userList.AddRange(audience.Select(i => new Claim(JwtRegisteredClaimNames.Aud, i)));

            return userList;

        }

        private IEnumerable<Claim> GetClaimsByClient(Client clientDto)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, clientDto.Id.ToString()));

            claims.AddRange(clientDto.Audiences.Select(i => new Claim(JwtRegisteredClaimNames.Aud, i)));

            return claims;
        }

        private string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();

        }


    }
}
