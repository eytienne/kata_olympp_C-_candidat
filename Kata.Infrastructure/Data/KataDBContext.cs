using Kata.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kata.Infrastructure.Data
{
    public class KataDBContext : DbContext
    {
        public KataDBContext(DbContextOptions<KataDBContext> options) : base(options) { }

        // public DbSet<Clan> Clans { get; set; }
        // public DbSet<Army> Armies { get; set; }
        // public DbSet<BattleReport> BattleReports { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            modelBuilder.Entity<Clan>()
            .HasMany(e => e.BattleReports)
            .WithMany(e => e.InitialClans);

            modelBuilder.Entity<BattleReport>()
            .HasOne(br => br.Winner)
            .WithMany()
            .HasForeignKey(br => br.WinnerId);

            modelBuilder.Entity<BattleReport>()
            .Property(br => br.History)
            .HasConversion(
                v => JsonSerializer.Serialize(v, options),
                v => JsonSerializer.Deserialize<List<BattleReport.BattleStep>>(v, options)
            );
            modelBuilder.Entity<BattleReport>()
            .Property(br => br.Status)
            .HasConversion<string>();

        }
    }
}
