using EProject_Sem_3.Models.Plans;

namespace EProject_Sem_3.Models.Subscriptions {
    public class SubscriptionRes {

        public int Id { get; set; }

        public DateTime ExpiredAt { get; set; }

        public PlanRes Plan { get; set; }

    }
}