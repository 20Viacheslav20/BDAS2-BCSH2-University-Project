using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Validators
{
    public class ClubCardPriceLessThanActualPriceAttribute : ValidationAttribute
    {
        private readonly string _actualPricePropertyName;

        public ClubCardPriceLessThanActualPriceAttribute(string actualPricePropertyName)
        {
            _actualPricePropertyName = actualPricePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var actualPriceProperty = validationContext.ObjectType.GetProperty(_actualPricePropertyName);

            if (actualPriceProperty == null)
            {
                throw new ArgumentException($"Property {_actualPricePropertyName} not found on {validationContext.ObjectType}");
            }

            var actualPriceValue = (double) actualPriceProperty.GetValue(validationContext.ObjectInstance, null);

            if (value is double clubCardPrice && clubCardPrice > actualPriceValue)
            {
                return new ValidationResult($"ClubCard Price cannot be greater than Actual Price.");
            }

            return ValidationResult.Success;
        }
    }
}
