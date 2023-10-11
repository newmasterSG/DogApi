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
        [Required(ErrorMessage = "Name is required")]
        [CustomValidation(typeof(DisallowValuesRule), "string", "int", "decimal")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Color is required")]
        [CustomValidation(typeof(DisallowValuesRule), "string", "int", "decimal")]
        public string Color { get; set; }

        [Required(ErrorMessage = "TailLength is required")]
        public int TailLength { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        public double Weight { get; set; }
    }
}
