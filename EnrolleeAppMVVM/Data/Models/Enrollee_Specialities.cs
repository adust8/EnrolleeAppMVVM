using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataAccessLibrary.Models
{
    public class Enrollee_Specialities
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int Priority { get; set; }

        [Required]
        public Guid EnrolleeGUID { get; set; }

        [Required]
        public int SpecialityId { get; set; }

        [ForeignKey("SpecialityId")]
        public Speciality Speciality { get; set; }

        [ForeignKey("EnrolleeGUID")]
        public Enrollee Enrollee { get; set; }


    }
}
