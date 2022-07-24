using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blazor_FullStack_CRUD.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;

        public SuperHeroController(DataContext context)
        {
            _context = context;
        }

        // YOU DO NOT NEED THIS ANYMORE bcs the database has been created
        //public static List<Comic> comics = new List<Comic> {
        //    new Comic {Id = 1, Name = "Marvel"},
        //    new Comic {Id = 2, Name = "DC"}
        //};

        //public static List<SuperHero> heroes = new List<SuperHero> {
        //    new SuperHero {
        //        Id = 1,
        //        FirstName = "Matt",
        //        LastName = "Murdock",
        //        HeroName = "Daredevil",
        //        Comic = comics[0],
        //        ComicId = 1,
        //    },
        //    new SuperHero
        //    {
        //        Id = 2,
        //        FirstName = "Selina",
        //        LastName = "Kyle",
        //        HeroName = "Catwoman",
        //        Comic = comics[1],
        //        ComicId = 2,
        //    }
        //};

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetSuperHeroes()
        {
            var heroes = await _context.SuperHeroes.Include(sh => sh.Comic).ToListAsync();
            return Ok(heroes);
        }

        [HttpGet("comics")]
        public async Task<ActionResult<List<Comic>>> GetComics()
        {
            var comics = await _context.Comics.ToListAsync();
            return Ok(comics);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<SuperHero>> GetSingleHero(int id)
        {
            var hero = await _context.SuperHeroes
                .Include(h => h.Comic)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (hero == null)
            {
                return NotFound("No heroes found.");
            }
            return Ok(hero);
        }

        // CREATE NEW HERO
        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> CreateSuperHero(SuperHero hero)
        {
            hero.Comic = null;
            _context.SuperHeroes.Add(hero);
            await _context.SaveChangesAsync();

            return Ok(await GetDbHeroes());
        }

        //UPDATE HERO
        [HttpPut("{id}")]
        public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(SuperHero hero, int id)
        {
            var dbHero = await _context.SuperHeroes
                .Include(sh => sh.Comic)
                .FirstOrDefaultAsync(sh => sh.Id == id);
            if (dbHero == null)
                return NotFound("No hero found.");
            dbHero.FirstName = hero.FirstName;
            dbHero.LastName = hero.LastName;
            dbHero.HeroName = hero.HeroName;
            dbHero.ComicId = hero.ComicId;

            await _context.SaveChangesAsync();

            return Ok(await GetDbHeroes());
        }

        // DELETE HERO
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int id)
        {
            var dbHero = await _context.SuperHeroes
                .Include(sh => sh.Comic)
                .FirstOrDefaultAsync(sh => sh.Id == id);
            if (dbHero == null)
                return NotFound("No hero found.");

            _context.SuperHeroes.Remove(dbHero);
            await _context.SaveChangesAsync();
            return Ok(await GetDbHeroes());
        }

        private async Task<List<SuperHero>> GetDbHeroes()
        {
            return await _context.SuperHeroes.Include(sh => sh.Comic).ToListAsync();
        }
    }
}
