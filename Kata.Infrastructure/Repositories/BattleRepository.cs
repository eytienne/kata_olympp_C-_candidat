using Kata.Domain.Entities;
using Kata.Domain.Repositories;
using Kata.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kata.Infrastructure.Repositories
{
    public class BattleRepository : IBattleRepository
    {
        private readonly KataDBContext _context;
        protected DbSet<BattleReport> DbSet => _context.Set<BattleReport>();

        public BattleRepository (KataDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BattleReport>> GetAllBattlesReport() => await DbSet.ToListAsync();

        public async Task<BattleReport?> GetBattleReportById(int Id) => await DbSet.FindAsync(Id);

        public async Task<BattleReport> SaveBattleReport() {
            await _context.SaveChangesAsync();
            return await DbSet.LastAsync();
        }
    }
}
