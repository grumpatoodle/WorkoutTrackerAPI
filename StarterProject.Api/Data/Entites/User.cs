using StarterProject.Api.Data.Entites;
using System.Collections.Generic;

namespace StarterProject.Api.Features.Users
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public List<Equipment> Equipments { get; set; }
        public List<Routine> Routines { get; set; }
        public List<Exercise> Exercises { get; set; }
    }
}