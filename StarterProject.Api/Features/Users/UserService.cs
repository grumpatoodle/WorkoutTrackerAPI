using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StarterProject.Api.Data;
using StarterProject.Api.Dtos.Users;
using StarterProject.Api.Security;
using StarterProject.Api.Features.Users;
using StarterProject.Api.Features.Users.Dtos;
using StarterProject.Api.Helpers;

namespace StarterProject.Api.Services
{
    public interface IUserService
    {
        UserAuthenticationGetDto Authenticate(UserAuthenticationDto userAuthenticationDto);
    }

    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly AppSettings _appSettings;

        public UserService(
            DataContext context, 
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public UserAuthenticationGetDto Authenticate(UserAuthenticationDto userAuthenticationDto)
        {
            var username = userAuthenticationDto.Username;
            var password = userAuthenticationDto.Password;

            var user = _context.Users.SingleOrDefault(x => x.Username == username);

            if (user == null)
                return null;

            var passwordHash = new PasswordHash(user.PasswordSalt, user.PasswordHash);

            if (!passwordHash.Verify(password))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            _context.SaveChanges();

            var userToReturn = new UserAuthenticationGetDto
            {
                UserId = user.Id,
                Token = tokenHandler.WriteToken(token)
            };
            
            return userToReturn;
        }
    }
}
