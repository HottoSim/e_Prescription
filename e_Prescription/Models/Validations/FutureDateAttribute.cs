using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.Validations
{
    public class FutureDateAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            DateTime dateValue;
            if (DateTime.TryParse(value.ToString(), out dateValue))
            {
                return dateValue > DateTime.Now;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The {name} field must be a date in the future.";
        }
    }
}
