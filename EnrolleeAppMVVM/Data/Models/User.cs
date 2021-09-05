using System;
using System.ComponentModel.DataAnnotations;

namespace EFDataAccessLibrary.Models
{
    public class User
    {
        [Required]
        [Key]
        [MaxLength(100)]
        public Guid UserGUID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Login { get; set; }

        public DateTime? LastPasswordChangeDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        public int RoleId { get; set; }

        public Role Role { get; set; }
    }
}
