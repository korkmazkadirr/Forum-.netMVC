using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Auth.Data;
using Auth.Models;
using Microsoft.AspNetCore.Authorization;

namespace Auth.Controllers
{
    [Authorize(Roles = "rol2")]
    public class LeadEntitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeadEntitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeadEntities
        public async Task<IActionResult> Index()
        {
            return _context.Lead != null ?
                        View(await _context.Lead.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Lead'  is null.");
        }

        // GET: LeadEntities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lead == null)
            {
                return NotFound();
            }

            var leadEntity = await _context.Lead
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leadEntity == null)
            {
                return NotFound();
            }

            return View(leadEntity);
        }

        // GET: LeadEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeadEntities/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Phone,Email,Adress,Gender,Birthdate,City")] LeadEntity leadEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leadEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leadEntity);
        }

        // GET: LeadEntities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lead == null)
            {
                return NotFound();
            }

            var leadEntity = await _context.Lead.FindAsync(id);
            if (leadEntity == null)
            {
                return NotFound();
            }
            return View(leadEntity);
        }

        // POST: LeadEntities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Phone,Email,Adress,Gender,Birthdate,City")] LeadEntity leadEntity)
        {
            if (id != leadEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leadEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeadEntityExists(leadEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leadEntity);
        }

        // GET: LeadEntities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lead == null)
            {
                return NotFound();
            }

            var leadEntity = await _context.Lead
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leadEntity == null)
            {
                return NotFound();
            }

            return View(leadEntity);
        }

        // POST: LeadEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lead == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Lead'  is null.");
            }
            var leadEntity = await _context.Lead.FindAsync(id);
            if (leadEntity != null)
            {
                _context.Lead.Remove(leadEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeadEntityExists(int id)
        {
            return (_context.Lead?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
