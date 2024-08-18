using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.Validations
{
    public class NotInFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime > DateTime.Now)
                {
                    return new ValidationResult("Date of Birth cannot be in the future.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
