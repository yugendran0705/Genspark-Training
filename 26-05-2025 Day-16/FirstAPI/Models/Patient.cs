using System;
using System.ComponentModel.DataAnnotations;

namespace FirstAPI.Models
{
    public class Patient
    {

        [Required]
        public int Id { get; set; }


        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Date of Birth is required.")] 
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Patient), nameof(ValidateDateOfBirth))]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")] 
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Phone number must be between 7 and 20 digits.")] 
        
        public string Phone { get; set; } = string.Empty; 

        
        public static ValidationResult? ValidateDateOfBirth(DateTime dob, ValidationContext context)
        {
            if (dob > DateTime.Today)
            {
                return new ValidationResult("Date of Birth cannot be in the future.", new[] { nameof(DOB) });
            }
            
            if (dob < DateTime.Today.AddYears(-120))
            {
                return new ValidationResult("Date of Birth is too far in the past (max 120 years ago). Expired!", new[] { nameof(DOB) });
            }
            return ValidationResult.Success;
        }
    }
}