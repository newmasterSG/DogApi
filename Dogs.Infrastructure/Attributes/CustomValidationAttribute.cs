using Dogs.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogs.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class CustomValidationAttribute : ValidationAttribute, IValidationRule
    {
        private readonly IValidationRule validationRule;

        public CustomValidationAttribute(Type ruleType, params object[] ruleParameters)
        {
            if (typeof(IValidationRule).IsAssignableFrom(ruleType))
            {
                validationRule = (IValidationRule)Activator.CreateInstance(ruleType, ruleParameters);
            }
            else
            {
                throw new ArgumentException("The ruleType must implement IValidationRule.");
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationRule.IsValid(value))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"The value '{value}' is not allowed for this property.");
            }
        }
    }
}
