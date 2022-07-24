namespace Blazor_FullStack_CRUD.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comic>().HasData(
                new Comic { Id = 1, Name = "Marvel"},
                new Comic { Id = 2, Name = "DC"}
            );
            modelBuilder.Entity<SuperHero>().HasData(
                new SuperHero { Id = 1, FirstName = "Matt", LastName = "Murdock", HeroName = "Daredevil", ComicId = 1 },
                new SuperHero { Id = 2, FirstName = "Selina", LastName = "Kyle", HeroName = "Catwoman", ComicId = 2 }
            );
        }

        public DbSet<SuperHero> SuperHeroes { get; set; }
        public DbSet<Comic> Comics { get; set; }
    }
}
