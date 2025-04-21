using Kata.Domain.Entities;
using Kata.Application.Services;
using Kata.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Kata.Domain.Repositories;
using System.Threading.Tasks;
using Kata.Infrastructure.Repositories;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using Microsoft.Identity.Client;

namespace Kata.Tests
{
    public class ClanTests
    {
        static readonly Clan[] clans = [
            new Clan { Name = "Troy", Armies = [
                new Army { Name = "army1", NbUnits = 100, Attack = 100, Defense = 100, Health = 100 },
                new Army { Name = "army1_1", NbUnits = 100, Attack = 1000, Defense = 100, Health = 100 },
            ]},
            new Clan { Name = "Greece", Armies = [
                new Army { Name = "army2_2", NbUnits = 50, Attack = 50, Defense = 500, Health = 100 },
            ]},
        ];

        private static async Task<KataDBContext> CreateTestContext()
        {
            var options = new DbContextOptionsBuilder<KataDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new KataDBContext(options);

            var Clans = context.Set<Clan>();

            await Clans.AddRangeAsync(clans);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task Test1()
        {
            var context = await CreateTestContext();
            var service = new BattleService(Substitute.For<IBattleRepository>(), Substitute.For<ClanRepository>(context), context);

            var result = await service.Battle();

            Assert.Equivalent(new BattleReport {
                Status = BattleReport.StatusType.DRAW,
                Winner = null,
                InitialClans = clans,
                History = [
                    new  BattleReport.BattleStep {
                        NameArmy1 = "army1",
                        NameArmy2 = "army2_2",
                        DamageArmy1 = -7500,
                        DamageArmy2 = 5000,
                        NbRemainingSoldiersArmy1 = 100,
                        NbRemainingSoldiersArmy2 = 0,
                    },
                    new  BattleReport.BattleStep {
                        NameArmy1 = "army1",
                        NameArmy2 = "army2_2",
                        DamageArmy1 = -7500,
                        DamageArmy2 = -15000,
                        NbRemainingSoldiersArmy1 = 100,
                        NbRemainingSoldiersArmy2 = 50,
                    },
                    new  BattleReport.BattleStep {
                        NameArmy1 = "army1",
                        NameArmy2 = "army2_2",
                        DamageArmy1 = -7500,
                        DamageArmy2 = -15000,
                        NbRemainingSoldiersArmy1 = 100,
                        NbRemainingSoldiersArmy2 = 50,
                    },
                ]
            }, result);
        }
    }
}