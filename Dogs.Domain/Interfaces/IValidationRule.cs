using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogs.Domain.Interfaces
{
    public interface IValidationRule
    {
        bool IsValid(object value);
    }
}
