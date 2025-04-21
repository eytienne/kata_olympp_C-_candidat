using System.Threading.Tasks;
using Kata.Domain.Entities;
using Kata.Domain.Repositories;
using Kata.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kata.Infrastructure.Repositories
{
    public class ClanRepository : IClanRepository
    {
        private readonly KataDBContext _context;
        protected DbSet<Clan> DbSet => _context.Set<Clan>();
        protected DbSet<Army> ArmyDbSet => _context.Set<Army>();

        public ClanRepository(KataDBContext context)
        {
            _context = context;
        }
        public Task<Clan?> GetArmyByNameClanAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Clan?> GetClanByNameAsync(string name) => await DbSet.Where(clan => clan.Name == name).FirstAsync();

        public async Task AddArmyAsync(string nameClan, Army army)
        {
            var clan = await GetClanByNameAsync(nameClan);
            clan?.Armies.Add(army);
        }
        public async Task UpdateArmyAsync(string nameClan, string armyName, Army army)
        {
            var trackedArmy = await GetClanArmy(nameClan, armyName);
            trackedArmy.NbUnits = army.NbUnits;
            trackedArmy.Attack = army.Attack;
            trackedArmy.Defense = army.Defense;
            trackedArmy.Health = army.Health;
        }

        public async Task DeleteArmyAsync(string nameClan, string nameArmy)
        {
            var trackedArmy = await GetClanArmy(nameClan, nameArmy);
            ArmyDbSet.Remove(trackedArmy);
        }

        public async Task<IEnumerable<Clan>> GetAllClansAsync() => await DbSet.ToListAsync();

        private async Task<Army> GetClanArmy(string clanName, string armyName) {
            var clan = await GetClanByNameAsync(clanName);
            if (clan == null) {
                throw new InvalidOperationException("Army not found");
            }
            var army = clan.Armies.Where(a => a.Name == armyName).First();
            return army;
        }
    }
}
