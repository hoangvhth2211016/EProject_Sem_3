using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models.Users {
    public class ChangePasswordDto {

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

    }
}
