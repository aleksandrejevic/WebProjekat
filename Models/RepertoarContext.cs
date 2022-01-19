using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class RepertoarContext : DbContext
    {
        public DbSet<Bioskop> Bioskopi {get; set;}
        public DbSet<Film> Filmovi {get; set;}
        public DbSet<Datum> Datumi {get; set;}
        public DbSet<PocetakPrikazivanja> PoceciPrikazivanja {get; set;}
        public DbSet<Spoj> BioskopiFilmovi {get; set;}

        public RepertoarContext(DbContextOptions options) : base(options)
        {

        }
    }
}