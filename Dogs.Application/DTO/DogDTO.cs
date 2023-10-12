using Dogs.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomValidationAttribute = Dogs.Infrastructure.Attributes.CustomValidationAttribute;

namespace Dogs.Application.DTO
{
    public class DogDTO
    {
        [CustomValidation(typeof(DisallowValuesRule), "string", "int", "decimal")]
        public string Name { get; set; }

        [CustomValidation(typeof(DisallowValuesRule), "string", "int", "decimal")]
        public string Color { get; set; }

        [CustomValidation(typeof(NumberValuesRule))]
        public int TailLength { get; set; }

        [CustomValidation(typeof(NumberValuesRule))]
        public double Weight { get; set; }
    }
}
