using Auth.Data;
using Auth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Auth1.Controllers
{
    public class Messages2Controller : Controller
    {
        private readonly ITempDataDictionary _tempDataDictionary;

        private ApplicationDbContext _context;
                public Messages2Controller(ApplicationDbContext context)
        {
            _context = context;
        }
        private bool ForumMessagesExists(int id)
        {
            //return (_context.ForumMessages?.Any(e => e.Id == id)).GetValueOrDefault();
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
        public IActionResult CreateMessagesByTopic()
        {


            ViewData["ForumTopicId"] = new SelectList(_context.ForumTopic, "Id", "TopicTitle");
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult CreateMessagesByTopic([Bind("Id,ForumTopicId,TopicTitle,MessageTitle,Message,CreatedAt")] ForumMessages model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                _context.ForumMessages.Add(model);
                _context.SaveChanges();
                return RedirectToAction("MessagesByTopic");
            }
            ViewData["ForumTopicId"] = new SelectList(_context.ForumTopic, "Id", "TopicTitle", model.ForumTopicId);
            return View(model);
        }
    }
}
