using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Models;

namespace CMS.Controllers
{
    public class EntryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EntryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Entry
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EntryModel
                .Include(e => e.Page)
                .Include(c => c.User)
                .ThenInclude(u => u.IdentityUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Entry/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entryModel = await _context.EntryModel
                .Include(e => e.Page)
                .Include(c => c.User)
                .ThenInclude(u => u.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entryModel == null)
            {
                return NotFound();
            }

            return View(entryModel);
        }

        // GET: Entry/Create
        public IActionResult Create()
        {
            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Title");

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", GetUserId());

            ViewData["CreatedAt"] = DateTime.Now.ToString("yyyy-MM-dd");

            return View();
        }

        // POST: Entry/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] int pageId, [FromForm] int? userId, [FromForm] DateTime createdAt)
        {
            var entryModel = new EntryModel
            {
                PageId = pageId,
                Page = await _context.PageModel.FindAsync(pageId),
                CreatedAt = createdAt
            };

            if (userId != null)
            {
                entryModel.UserId = userId;
                entryModel.User = await _context.UserModel.FindAsync(userId);
            }

            if (ModelState.IsValid)
            {
                _context.Add(entryModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Title", entryModel.PageId);

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", entryModel.UserId);

            ViewData["CreatedAt"] = entryModel.CreatedAt.ToString("yyyy-MM-dd");

            return View(entryModel);
        }

        // GET: Entry/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entryModel = await _context.EntryModel.FindAsync(id);
            if (entryModel == null)
            {
                return NotFound();
            }

            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Title", entryModel.PageId);

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", entryModel.UserId);

            return View(entryModel);
        }

        // POST: Entry/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] int pageId, [FromForm] int? userId, [FromForm] DateTime createdAt)
        {
            var entryModel = await _context.EntryModel.FindAsync(id);
            if (entryModel == null)
            {
                return NotFound();
            }

            entryModel.PageId = pageId;
            entryModel.Page = await _context.PageModel.FindAsync(pageId);

            if (userId != null)
            {
                entryModel.UserId = userId;
                entryModel.User = await _context.UserModel.FindAsync(userId);
            }

            entryModel.CreatedAt = createdAt;

            if (ModelState.IsValid)
            {
                _context.Update(entryModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Title", entryModel.PageId);

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", entryModel.UserId);

            return View(entryModel);
        }

        // GET: Entry/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entryModel = await _context.EntryModel
                .Include(e => e.Page)
                .Include(e => e.User)
                .ThenInclude(u => u.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entryModel == null)
            {
                return NotFound();
            }

            return View(entryModel);
        }

        // POST: Entry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entryModel = await _context.EntryModel.FindAsync(id);
            if (entryModel != null)
            {
                _context.EntryModel.Remove(entryModel);
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
