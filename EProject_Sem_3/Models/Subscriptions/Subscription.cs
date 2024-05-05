using EProject_Sem_3.Models.Plans;
using EProject_Sem_3.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models.Subscriptions {
    public class Subscription : BaseEntity {

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PlanId { get; set; }

        [Required]
        public DateTime ExpiredAt { get; set; }

        public required User User { get; set; }

        public required Plan Plan { get; set; }

    }
}
