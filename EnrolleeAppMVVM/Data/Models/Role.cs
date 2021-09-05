using System.ComponentModel.DataAnnotations;

namespace EFDataAccessLibrary.Models
{
    public class Role
    {
        [Required]
        [MaxLength(10)]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoleName { get; set; }
    }
}
