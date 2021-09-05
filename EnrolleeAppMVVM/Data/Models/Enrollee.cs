using EnrolleeAppMVVM.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataAccessLibrary.Models
{
    public class Enrollee
    {
        [Key]
        [MaxLength(100)]
        public Guid EnrolleeGUID { get; set; }

        [MaxLength(100)]
        [Required]
        public Guid UserGUID { get; set; }

        public string PassportId { get; set; }

        [Required]
        public int StatusId { get; set; }


        public int? EducationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string SecondName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Patronymic { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Email { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string CertificateNumber { get; set; }

        [MaxLength(100)]
        public double? GPA { get; set; }

        [NotMapped]
        public IObservable<int> Specialities { get; set; }

        [ForeignKey("StatusId")]
        public Status Status { get; set; }

        [ForeignKey("UserGUID")]
        public User User { get; set; }

        [ForeignKey("EducationId")]
        public Education Education { get; set; }
    }
}