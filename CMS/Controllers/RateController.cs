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
    public class RateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rate
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RateModel
                .Include(r => r.Page)
                .Include(r => r.User)
                .ThenInclude(u => u.IdentityUser);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Rate/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rateModel = await _context.RateModel
                .Include(r => r.Page)
                .Include(r => r.User)
                .ThenInclude(u => u.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rateModel == null)
            {
                return NotFound();
            }

            return View(rateModel);
        }

        // GET: Rate/Create
        public IActionResult Create()
        {
            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Title");

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", GetUserId());

            ViewData["CreatedAt"] = DateTime.Now.ToString("yyyy-MM-dd");

            ViewData["Rating"] = EnumExtensions.ToSelectList<RatingsEnum>();

            ViewData["Status"] = EnumExtensions.ToSelectList<InteractionStatusesEnum>();

            return View();
        }

        // POST: Rate/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] int pageId, [FromForm] int userId, [FromForm] DateTime createdAt, [FromForm] RatingsEnum rating, [FromForm] InteractionStatusesEnum status)
        {
            var rateModel = new RateModel
            {
                PageId = pageId,
                Page = await _context.PageModel.FindAsync(pageId),
                UserId = userId,
                User = await _context.UserModel.FindAsync(userId),
                CreatedAt = createdAt,
                Rating = rating,
                Status = status
            };

            if (ModelState.IsValid)
            {
                _context.Add(rateModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Title", rateModel.PageId);

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", rateModel.UserId);

            ViewData["CreatedAt"] = rateModel.CreatedAt.ToString("yyyy-MM-dd");

            ViewData["Rating"] = EnumExtensions.ToSelectList<RatingsEnum>(rateModel.Rating);

            ViewData["Status"] = EnumExtensions.ToSelectList<InteractionStatusesEnum>(rateModel.Status);

            return View(rateModel);
        }

        // GET: Rate/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rateModel = await _context.RateModel.FindAsync(id);
            if (rateModel == null)
            {
                return NotFound();
            }

            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Title", rateModel.PageId);

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", rateModel.UserId);

            ViewData["Rating"] = EnumExtensions.ToSelectList<RatingsEnum>(rateModel.Rating);

            ViewData["Status"] = EnumExtensions.ToSelectList<InteractionStatusesEnum>(rateModel.Status);

            return View(rateModel);
        }

        // POST: Rate/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] int pageId, [FromForm] int userId, [FromForm] DateTime createdAt, [FromForm] RatingsEnum rating, [FromForm] InteractionStatusesEnum status)
        {
            var rateModel = await _context.RateModel.FindAsync(id);
            if (rateModel == null)
            {
                return NotFound();
            }

            rateModel.PageId = pageId;
            rateModel.Page = await _context.PageModel.FindAsync(pageId);
            rateModel.UserId = userId;
            rateModel.User = await _context.UserModel.FindAsync(userId);
            rateModel.CreatedAt = createdAt;
            rateModel.Rating = rating;
            rateModel.Status = status;

            if (ModelState.IsValid)
            {
                _context.Update(rateModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Title", rateModel.PageId);

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", rateModel.UserId);

            ViewData["Rating"] = EnumExtensions.ToSelectList<RatingsEnum>(rateModel.Rating);

            ViewData["Status"] = EnumExtensions.ToSelectList<InteractionStatusesEnum>(rateModel.Status);

            return View(rateModel);
        }

        // GET: Rate/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rateModel = await _context.RateModel
                .Include(r => r.Page)
                .Include(r => r.User)
                .ThenInclude(u => u.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rateModel == null)
            {
                return NotFound();
            }

            return View(rateModel);
        }

        // POST: Rate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rateModel = await _context.RateModel.FindAsync(id);
            if (rateModel != null)
            {
                _context.RateModel.Remove(rateModel);
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
