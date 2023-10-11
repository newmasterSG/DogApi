using Dogs.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogs.Infrastructure.Attributes
{
    public class DisallowValuesRule : IValidationRule
    {
        private readonly string[] disallowedValues;

        public DisallowValuesRule(params string[] disallowedValues)
        {
            this.disallowedValues = disallowedValues;
        }

        public bool IsValid(object value)
        {
            if (value != null)
            {
                string strValue = value.ToString();
                foreach (var disallowedValue in disallowedValues)
                {
                    if (string.Equals(strValue, disallowedValue, StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
