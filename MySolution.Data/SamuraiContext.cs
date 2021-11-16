using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySolution.Domain;
using System;

namespace MySolution.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = (localdb)\\ProjectsV13; Initial Catalog=MySolutionData02")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name},
                LogLevel.Information).EnableSensitiveDataLogging();
    
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(b => b.Samurais)
                .UsingEntity<BattleSamurai>
                (bs => bs.HasOne<Battle>().WithMany(),
                 bs => bs.HasOne<Samurai>().WithMany())
                .Property(bs => bs.DateJoinded)
                .HasDefaultValueSql("getdate()");

            //bidejkji nemame DbSet za Horses, se kriira tabela so imeto na klasata
            //Dokolku sakam da mi se vika razlichno go koristam ovoj metod
            modelBuilder.Entity<Horse>().ToTable("Horses");

        }
    }
}
