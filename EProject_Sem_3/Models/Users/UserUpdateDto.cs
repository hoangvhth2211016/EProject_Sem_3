using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models.Users {
    public class UserUpdateDto {

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Username { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? Avatar { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

    }
}
