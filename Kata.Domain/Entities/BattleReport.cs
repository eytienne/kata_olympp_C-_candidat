using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Domain.Entities
{
    public class BattleReport
    {
        public int Id { get; set; }
        public int WinnerId { get; set; }
        public Clan? Winner { get; set; }
        public StatusType Status { get; set; }

        [MinLength(2), MaxLength(2)]
        public ICollection<Clan> InitialClans { get; set; } = [];
        public ICollection<BattleStep> History { get; set; }

        public enum StatusType
        {
            DRAW,
            VICTORY
        }

        public class BattleStep {
            public string NameArmy1 { get; set; }
            public string NameArmy2 { get; set; }
            public int DamageArmy1 { get; set; }
            public int DamageArmy2 { get; set; }
            public int NbRemainingSoldiersArmy1 { get; set; }
            public int NbRemainingSoldiersArmy2 { get; set; }
        }
    }
}
