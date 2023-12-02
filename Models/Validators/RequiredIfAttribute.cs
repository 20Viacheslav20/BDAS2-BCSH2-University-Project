using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Validators
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _otherProperty;
        private readonly object _otherPropertyValue;

        public RequiredIfAttribute(string otherProperty, object otherPropertyValue)
        {
            _otherProperty = otherProperty;
            _otherPropertyValue = otherPropertyValue;
        }

        protected override ValidationResult IsValid(object validationPropertyValue, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_otherProperty);
            if (property == null)
            {
                return new ValidationResult($"Unknown property: {_otherProperty}");
            }

            var otherValue = property.GetValue(validationContext.ObjectInstance);

            if (Equals(otherValue, _otherPropertyValue))
            {
                if (validationPropertyValue == null || string.IsNullOrWhiteSpace(validationPropertyValue.ToString()))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
