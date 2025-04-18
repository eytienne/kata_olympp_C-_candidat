using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Domain.Entities
{
    public class Clan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Army> Armies { get; } = [];
        public ICollection<BattleReport> BattleReports { get; } = [];
    }
}
