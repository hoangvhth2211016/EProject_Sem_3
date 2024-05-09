using System.ComponentModel.DataAnnotations;
using EProject_Sem_3.Models.Subscriptions;

namespace EProject_Sem_3.Models.Users {
    public class UserRes {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }

        public string Phone { get; set; }

        public string Avatar { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<SubscriptionRes> Subscriptions { get; set; }
    }
}
