using Mariala.Areas.Admin.ViewModels.Teams;
using Mariala.DAL;
using Mariala.Models;
using Mariala.Utilities.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mariala.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Team> Teams = await _context.Teams.Include(x => x.Positions).ToListAsync();
            return View(Teams);
        }
        public async Task<IActionResult> Create()
        {
            CreateTeamVM create = new CreateTeamVM()
            {
                Positions = await _context.Positions.ToListAsync()
            };
            return View(create);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamVM create)
        {
            if (!ModelState.IsValid)
            {
                create.Positions = await _context.Positions.ToListAsync();
                return View(create);
            }

            bool result = await _context.Teams.AnyAsync(x => x.Name.Trim().ToLower() == create.Name.Trim().ToLower());
            if (result)
            {
                create.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Name", "does exist");
                return View(create);
            }
            if (!create.Photo.ValidateType())
            {
                create.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Photo", "false type");
                return View(create);

            }
            if (!create.Photo.ValidateSize(10))
            {
                create.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Photo", "10mb");
                return View(create);

            }
            Team team = new Team
            {
                Name = create.Name,
                Description = create.Description,
                FbLink = create.FbLink,
                TwitLink = create.TwitLink,
                LinkedLink = create.LinkedLink,
                PositionId = create.PositionId,
                Image = await create.Photo.Create(_env.WebRootPath, "assets", "img")
            };

            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (team == null) return NotFound();

            UpdateTeamVM update = new UpdateTeamVM
            {
                Name = team.Name,

                Description = team.Description,
                FbLink = team.FbLink,
                TwitLink = team.TwitLink,
                LinkedLink = team.LinkedLink,
                PositionId = team.PositionId,
                Positions = await _context.Positions.ToListAsync(),
            };

            return View(update);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateTeamVM update)
        {
            if (!ModelState.IsValid)
            {
                update.Positions = await _context.Positions.ToListAsync();
                return View(update);
            }
            Team team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (team == null) return NotFound();

            bool result = await _context.Teams.AnyAsync(x => x.Name.Trim().ToLower() == update.Name.Trim().ToLower() && x.Id == id);

            if (result)
            {
                update.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Name", "is exists");
                return View(update);
            }

            if (update.Photo is not null)
            {
                if (update.Photo.ValidateType())
                {
                    update.Positions = await _context.Positions.ToListAsync();
                    ModelState.AddModelError("Name", "not valid");
                    return View(update);
                }
                if (update.Photo.ValidateSize(10))
                {
                    update.Positions = await _context.Positions.ToListAsync();
                    ModelState.AddModelError("Name", "is exists");
                    return View(update);
                }
                team.Image.Delete(_env.WebRootPath, "assets", "img");
                team.Image = await update.Photo.Create(_env.WebRootPath, "assets", "img");
            }
            team.Name = update.Name;
            team.PositionId = update.PositionId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Team team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null) return NotFound();
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
