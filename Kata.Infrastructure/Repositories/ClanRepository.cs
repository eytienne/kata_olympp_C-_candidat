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
            var clan = await GetClanByNameAsync(nameClan);
            if (clan == null) {
                throw new InvalidOperationException("Army not found");
            }
            var trackedArmy = clan.Armies.Where(a => a.Name == armyName).First();
            trackedArmy.NbUnits = army.NbUnits;
            trackedArmy.Attack = army.Attack;
            trackedArmy.Defense = army.Defense;
            trackedArmy.Health = army.Health;
        }

        public Task DeleteArmyAsync(string nameClan, string nameArmy)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Clan>> GetAllClansAsync()
        {
            throw new NotImplementedException();
        }




    }
}
