using Auth.Data;
using Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Auth.Controllers
{
    [Authorize(Roles = "rol1")]
    public class ForumTopicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForumTopicController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: ForumTopicEntities
        public async Task<IActionResult> Index()
        {
            return _context.ForumTopic != null ?
                        View(await _context.ForumTopic.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.ForumTopic'  is null.");

        }
        // GET: ForumTopic/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ForumTopic model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now; 
                _context.ForumTopic.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        //GET: ForumTopicEntities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ForumTopic == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopic
                .FirstOrDefaultAsync(m => m.Id == id);
            if (forumTopic == null)
            {
                return NotFound();
            }

            return View(forumTopic);
        }


        // GET: ForumTopicEntities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ForumTopic == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopic.FindAsync(id);
            if (forumTopic == null)
            {
                return NotFound();
            }
            ViewData["ForumTopicId"] = new SelectList(_context.ForumTopic, "Id,TopicTitle", forumTopic.TopicTitle);
            return View(forumTopic);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TopicTitle,CreatedAt")] ForumTopic forumTopic)
        {
            if (id != forumTopic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forumTopic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForumTopicExists(forumTopic.Id))
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
            ViewData["ForumTopicId"] = new SelectList(_context.ForumTopic, "Id,TopicTitle", forumTopic.TopicTitle);
            return View(forumTopic);
        }


        // GET: ForumTopicEntities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ForumTopic == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopic
                .FirstOrDefaultAsync(m => m.Id == id);
            if (forumTopic == null)
            {
                return NotFound();
            }

            return View(forumTopic);
        }

        // POST: ForumTopicEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ForumTopic == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ForumTopic'  is null.");
            }
            var forumTopic = await _context.ForumTopic.FindAsync(id);
            if (forumTopic != null)
            {
                _context.ForumTopic.Remove(forumTopic);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ForumTopicExists(int id)
        {
            return (_context.ForumTopic?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Messages(int id)
        {
            var topic = await _context.ForumTopic.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            var messages = await _context.ForumMessages.Where(m => m.ForumTopicId == id).ToListAsync();
            ViewBag.TopicTitle = topic.TopicTitle;
            return View(messages);
        }
        public IActionResult MessagesByTopic(int forumTopicId)
        {
            var topic = _context.ForumTopic.FirstOrDefault(t => t.Id == forumTopicId);

            if (topic == null)
            {
                return NotFound();
            }

            ViewBag.TopicTitle = topic.TopicTitle;

            var messages = _context.ForumMessages
                .Where(m => m.ForumTopicId == forumTopicId)
                .OrderByDescending(m => m.CreatedAt)
                .ToList();

            return View(messages);
        }


    }
}

