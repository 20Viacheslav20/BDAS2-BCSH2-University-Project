using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Validators
{
    public class BornNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is string))
            {
                return new ValidationResult("Invalid Rodne Cislo format.");
            }

            string rodneCislo = (string)value;

            if (rodneCislo.Length != 10 || !IsNumeric(rodneCislo))
            {
                return new ValidationResult("Invalid Rodne Cislo format.");
            }

            int year = int.Parse(rodneCislo.Substring(0, 2));
            int month = int.Parse(rodneCislo.Substring(2, 2));
            int day = int.Parse(rodneCislo.Substring(4, 2));

            if (month > 50)
            {
                month -= 50;
            }

            try
            {
                DateTime birthdate = new DateTime(year < 54 ? 2000 + year : 1900 + year, month, day);
            }
            catch (Exception)
            {
                return new ValidationResult("Invalid Rodne Cislo format.");
            }

            return ValidationResult.Success;
        }

        private bool IsNumeric(string value)
        {
            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
