using Kata.Domain.Entities;
using Kata.Domain.Interfaces;
using Kata.Domain.Repositories;
using Kata.Infrastructure.Data; // ou le namespace où est défini KataDBContext
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kata.Application.Services
{
    public class BattleService : IBattleService
    {
        private readonly IBattleRepository _battleRepository;
        private readonly IClanRepository _clanRepository;

        private readonly KataDBContext _context;

        public BattleService(IBattleRepository battleRepository, IClanRepository clanRepository, KataDBContext context)
        {
            _battleRepository = battleRepository;
            _clanRepository = clanRepository;
            _context = context;
        }
        /// <summary>
        /// Generates a battle between the armies of  the first and the last Clan
        /// Returns a battle report after saving it for historisation
        /// </summary>
        /// <returns></returns>
        public async Task<BattleReport> Battle()
        {
            var clans = await _clanRepository.GetAllClansAsync();
            var clan1 = clans.First();
            var clan2 = clans.Last();
            var armies1 = clan1.Armies.GetEnumerator();
            var armies2 = clan2.Armies.GetEnumerator();
            var battleReport = new BattleReport {
                InitialClans = [clan1, clan2]
            };
            var army1 = DeepClone(armies1.Current);
            var army2 = DeepClone(armies2.Current);
            while (LiveArmy(army1) && LiveArmy(army2)) {
                var damageArmy1 = army2.Attack*army2.NbUnits - army1.Defense*army1.NbUnits;
                var damageArmy2 = army1.Attack*army1.NbUnits - army2.Defense*army2.NbUnits;
                army1.NbUnits -= damageArmy1/army1.Health;
                army2.NbUnits -= damageArmy2/army2.Health;
                await Task.WhenAll(
                    _clanRepository.UpdateArmyAsync(clan1.Name, army1.Name, army1),
                    _clanRepository.UpdateArmyAsync(clan2.Name, army1.Name, army1)
                );

                battleReport.History.Add(new BattleReport.BattleStep {
                    NameArmy1 = army1.Name,
                    NameArmy2 = army2.Name,
                    DamageArmy1 = damageArmy1,
                    DamageArmy2 = damageArmy2,
                    NbRemainingSoldiersArmy1 = army1.NbUnits,
                    NbRemainingSoldiersArmy2 = army2.NbUnits,
                });
                if (!LiveArmy(army1)) {
                    armies1.MoveNext();
                    army1 = DeepClone(armies1.Current);
                }
                if (!LiveArmy(army2)) {
                    armies2.MoveNext();
                    army2 = DeepClone(armies2.Current);
                }
            }
            battleReport.Winner = LiveArmy(army1) ? clan1 : (LiveArmy(army2) ? clan2 : null);
            battleReport.Status = battleReport.Winner == null ? BattleReport.StatusType.DRAW : BattleReport.StatusType.VICTORY;

            await _context.SaveChangesAsync();
            return battleReport;
        }

        private static bool LiveArmy(in Army? army) {
            return army != null && army.NbUnits > 0;
        }

        private static T DeepClone<T>(T obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(json)!;
        }
    }
}
