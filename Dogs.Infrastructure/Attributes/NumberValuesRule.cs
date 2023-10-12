using Dogs.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogs.Infrastructure.Attributes
{
    public class NumberValuesRule : IValidationRule
    {
        private readonly int[] _disallowedValues;

        public NumberValuesRule(params int[] disallowedValues)
        {
            _disallowedValues = disallowedValues;
        }

        public bool IsValid(object value)
        {
            if (value != null)
            {
                if(double.TryParse(value.ToString(), out double numericValue))
                {
                    if (numericValue <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
