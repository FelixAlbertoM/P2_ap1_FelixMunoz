using Microsoft.EntityFrameworkCore;
using P2_AP1_FelixMunoz.Models;

namespace P2_AP1_FelixMunoz.DAL;

public class Contexto: DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }

    public DbSet<Registro> Registro { get; set; }
}
