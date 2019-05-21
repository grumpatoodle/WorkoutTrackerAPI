using System.Collections.Generic;
using System.Linq;
using StarterProject.Api.Common;
using StarterProject.Api.Data;
using StarterProject.Api.Features.Users.Dtos;
using StarterProject.Api.Security;

namespace StarterProject.Api.Features.Users
{
    public interface IUserRepository
    {
        UserGetDto GetUser(int userId);
        List<UserGetDto> GetAllUsers();
        UserGetDto CreateUser(UserCreateDto userCreateDto);
        UserGetDto EditUser(int userId, UserEditDto userUpdateDto);
        UserGetDto EditUserRole(int userId, UserRoleEditDto userRoleEditDto);
        void DeleteUser(int userId);
    }

    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public UserGetDto GetUser(int userId)
        {
            return _context
                .Set<User>()
                .Select(x => new UserGetDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Username = x.Username,
                    Email = x.Email,
                    Role = x.Role
                })
                .FirstOrDefault(x => x.Id == userId);
        }

        public List<UserGetDto> GetAllUsers()
        {
            return _context
                .Set<User>()
                .Select(x => new UserGetDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Username = x.Username,
                    Email = x.Email,
                    Role = x.Role
                })
                .ToList();
        }

        public UserGetDto CreateUser(UserCreateDto userCreateDto)
        {
            var passwordHash = new PasswordHash(userCreateDto.Password);

            var user = new User
            {
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                Username = userCreateDto.Username,
                Email = userCreateDto.Email,
                Role = Constants.Users.Roles.User,
                PasswordSalt = passwordHash.Salt,
                PasswordHash = passwordHash.Hash
            };

            _context.Set<User>().Add(user);
            _context.SaveChanges();

            var userGetDto = new UserGetDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };

            return userGetDto;
        }

        public UserGetDto EditUser(int userId, UserEditDto userEditDto)
        {
            var passwordHash = new PasswordHash(userEditDto.Password);
            var user = _context.Set<User>().Find(userId);

            user.FirstName = userEditDto.FirstName;
            user.LastName = userEditDto.LastName;
            user.Username = userEditDto.Username;
            user.Email = userEditDto.Email;
            user.PasswordSalt = passwordHash.Salt;
            user.PasswordHash = passwordHash.Hash;

            _context.SaveChanges();

            var userGetDto = new UserGetDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            }; 

            userGetDto.Id = user.Id;

            return userGetDto;
        }

        public UserGetDto EditUserRole(int userId, UserRoleEditDto userRoleEditDto)
        {
            var user = _context.Set<User>().Find(userId);

            user.Role = userRoleEditDto.Role;

            _context.SaveChanges();

            var userGetDto = new UserGetDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };

            userGetDto.Id = user.Id;

            return userGetDto;
        }

        public void DeleteUser(int userId)
        {
            var user = _context.Set<User>().Find(userId);
            _context.Set<User>().Remove(user);
            _context.SaveChanges();
        }
    }
}
