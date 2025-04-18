using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Domain.Entities
{
    public class Army
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NbUnits { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Health { get; set; }
        public Clan? Clan { get; set; }
    }
}
