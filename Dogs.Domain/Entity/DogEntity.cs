using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogs.Domain.Entity
{
    public class DogEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Color { get; set; }

        public int TailLength {  get; set; }

        public double Weight { get; set; }
    }
}
