using StarterProject.Api.Features.Users;

namespace StarterProject.Api.Data.Entites
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
