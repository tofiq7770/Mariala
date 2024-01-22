using Mariala.Areas.Admin.ViewModels.Positions;
using Mariala.DAL;
using Mariala.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Mariala.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        private readonly AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Position> positions = await _context.Positions.Include(x => x.Teams).ToListAsync();
            return View(positions);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionVM create)
        {
            if (!ModelState.IsValid) return View(create);

            bool result = await _context.Positions.AnyAsync(x => x.Name.Trim().ToLower() == create.Name.Trim().ToLower());
            if (result)
            {
                ModelState.AddModelError("Name", "does exist");
                return View(create);
            }
            Position position = new Position()
            {
                Name = create.Name
            };
            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            
            Position position = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
            
            if (position == null) return NotFound();

            UpdatePositionVM update = new UpdatePositionVM
            {
                Name = position.Name,
            };

            return View(update);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdatePositionVM update)
        {
            if (!ModelState.IsValid) return View(update);

            Position position = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (position == null) return NotFound();

            bool result = await _context.Positions.AnyAsync(x => x.Name.Trim().ToLower() == update.Name.Trim().ToLower() && x.Id == id);

            if (result)
            {
                ModelState.AddModelError("Name", "is exists");
                return View(update);
            }

            position.Name = update.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Position Position = await _context.Positions.FirstOrDefaultAsync(y => y.Id == id);
            if (Position == null) return NotFound();

            _context.Positions.Remove(Position);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
