using System;
using Microsoft.EntityFrameworkCore;

namespace EloisaSantos.Models;

public class AppDataContext : DbContext
{
    public DbSet<Consumo> Consumos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=lucascristo_eloisasantos.db");
    }
}
