using System.ComponentModel.DataAnnotations;

namespace EFDataAccessLibrary.Models
{
    public class Status
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public int StatusId { get; set; }

        [Required]
        [MaxLength(100)]
        public string StatusName { get; set; }
    }
}
