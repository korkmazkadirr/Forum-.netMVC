using Auth.Data;
using Auth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Auth.Controllers
{
    public class ForumMessagesController : Controller
    {
        private ApplicationDbContext _context;

        public ForumMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Messages()
        {
            //var applicationDbContext = _context.ForumMessages.Include(u => u.TopicTitle);
            //return View(await applicationDbContext.ToListAsync());

            return _context.ForumMessages != null ?
                     View(await _context.ForumMessages.ToListAsync()) :
                     Problem("Entity set 'ApplicationDbContext.ForumMessages'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ForumMessages == null)
            {
                return NotFound();
            }

            var forumMessages = await _context.ForumMessages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (forumMessages == null)
            {
                return NotFound();
            }

            return View(forumMessages);
        }
        public IActionResult Create()
        {

            ViewData["ForumTopicId"] = new SelectList(_context.ForumTopic, "Id", "TopicTitle");
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind("Id,ForumTopicId,TopicTitle,MessageTitle,Message,CreatedAt")] ForumMessages model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                _context.ForumMessages.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Messages");
            }
            ViewData["ForumTopicId"] = new SelectList(_context.ForumTopic, "Id", "TopicTitle", model.ForumTopicId);
            return View(model);
        }

        // GET: ForumMessageEntities/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ForumMessages == null)
            {
                return NotFound();
            }

            var forumMessages = await _context.ForumMessages.FindAsync(id);
            if (forumMessages == null)
            {
                return NotFound();
            }
            ViewData["ForumTopicId"] = new SelectList(_context.ForumTopic, "Id", "TopicTitle", forumMessages.ForumTopicId);

            return View(forumMessages);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ForumTopicId,TopicTitle,MessageTitle,Message,CreatedAt")] ForumMessages forumMessages)
        {
            if (id != forumMessages.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forumMessages);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForumMessagesExists(forumMessages.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Messages));
            }
            ViewData["ForumTopicId"] = new SelectList(_context.ForumTopic, "Id", "TopicTitle", forumMessages.ForumTopicId);

            return View(forumMessages);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ForumMessages == null)
            {
                return NotFound();
            }
            var forumMessages = await _context.ForumMessages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (forumMessages == null)
            {
                return NotFound();
            }
            return View(forumMessages);
        }

        // POST: ForumMessageEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ForumMessages == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ForumMessages'  is null.");
            }
            var forumMessages = await _context.ForumMessages.FindAsync(id);
            if (forumMessages != null)
            {
                _context.ForumMessages.Remove(forumMessages);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Messages));
        }
        private bool ForumMessagesExists(int id)
        {
            return _context.ForumMessages.Any(e => e.Id == id);
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