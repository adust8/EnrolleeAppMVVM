using EnrolleeAppMVVM.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataAccessLibrary.Models
{
    public class Speciality
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public int SpecialityId { get; set; }

        [Required]
        [MaxLength(100)]
        public int NameId { get; set; }

        public string SpecialityCode { get; set; }

        [Required]
        [MaxLength(100)]
        public int StudyPeriodId { get; set; }

        public int MinimumEducationLevel { get; set; }

        public int FinancingId { get; set; }

        [ForeignKey("MinimumEducationLevel")]
        public Education Education { get; set; }

        [ForeignKey("FinancingId")]
        public Financing Financing { get; set; }

        [ForeignKey("StudyPeriodId")]
        public StudyPeriod StudyPeriod { get; set; }

        [ForeignKey("NameId")]
        public SpecialityName SpecialityName { get; set; }
    }
}