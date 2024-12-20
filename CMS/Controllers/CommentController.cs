using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Enums;
using CMS.Models;

namespace CMS.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comment
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CommentModel
                .Include(c => c.Page)
                .Include(c => c.User)
                .ThenInclude(u => u.IdentityUser);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentModel = await _context.CommentModel
                .Include(c => c.Page)
                .Include(c => c.User)
                .ThenInclude(u => u.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentModel == null)
            {
                return NotFound();
            }

            return View(commentModel);
        }

        // GET: Comment/Create
        public IActionResult Create()
        {
            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Title");

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", GetUserId());

            ViewData["CreatedAt"] = DateTime.Now.ToString("yyyy-MM-dd");

            ViewData["Status"] = EnumExtensions.ToSelectList<InteractionStatusesEnum>();

            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] int pageId, [FromForm] int userId, [FromForm] DateTime createdAt, [FromForm] string description, [FromForm] InteractionStatusesEnum status)
        {
            var commentModel = new CommentModel
            {
                PageId = pageId,
                Page = await _context.PageModel.FindAsync(pageId),
                UserId = userId,
                User = await _context.UserModel.FindAsync(userId),
                CreatedAt = createdAt,
                Description = description,
                Status = status
            };

            if (ModelState.IsValid)
            {
                _context.Add(commentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Title", commentModel.PageId);

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", commentModel.UserId);

            ViewData["CreatedAt"] = commentModel.CreatedAt.ToString("yyyy-MM-dd");

            ViewData["Status"] = EnumExtensions.ToSelectList<InteractionStatusesEnum>(commentModel.Status);

            return View(commentModel);
        }

        // GET: Comment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentModel = await _context.CommentModel.FindAsync(id);
            if (commentModel == null)
            {
                return NotFound();
            }

            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Title", commentModel.PageId);

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", commentModel.UserId);

            ViewData["Status"] = EnumExtensions.ToSelectList<InteractionStatusesEnum>(commentModel.Status);

            return View(commentModel);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] int pageId, [FromForm] int userId, [FromForm] DateTime createdAt, [FromForm] string description, [FromForm] InteractionStatusesEnum status)
        {
            var commentModel = await _context.CommentModel.FindAsync(id);
            if (commentModel == null)
            {
                return NotFound();
            }

            commentModel.PageId = pageId;
            commentModel.Page = await _context.PageModel.FindAsync(pageId);
            commentModel.UserId = userId;
            commentModel.User = await _context.UserModel.FindAsync(userId);
            commentModel.CreatedAt = createdAt;
            commentModel.Description = description;
            commentModel.Status = status;

            if (ModelState.IsValid)
            {
                _context.Update(commentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Title", commentModel.PageId);

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", commentModel.UserId);

            ViewData["Status"] = EnumExtensions.ToSelectList<InteractionStatusesEnum>(commentModel.Status);

            return View(commentModel);
        }

        // GET: Comment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentModel = await _context.CommentModel
                .Include(c => c.Page)
                .Include(c => c.User)
                .ThenInclude(u => u.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentModel == null)
            {
                return NotFound();
            }

            return View(commentModel);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commentModel = await _context.CommentModel.FindAsync(id);
            if (commentModel != null)
            {
                _context.CommentModel.Remove(commentModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private int GetUserId()
        {
            string IdentifierUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _context.UserModel
                .Where(u => u.IdentityUser.Id == IdentifierUserId)
                .Select(u => u.Id)
                .FirstOrDefault();
        }
    }
}
