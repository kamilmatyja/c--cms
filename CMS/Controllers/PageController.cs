using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Models;

namespace CMS.Controllers
{
    public class PageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Page
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PageModel.Include(p => p.Category).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Page/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageModel = await _context.PageModel
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pageModel == null)
            {
                return NotFound();
            }

            return View(pageModel);
        }

        // GET: Page/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Set<UserModel>(), "Id", "Id");
            return View();
        }

        // POST: Page/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,CategoryId,CreatedAt,Link,Title,Description,Keywords,Image")] PageModel pageModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pageModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Id", pageModel.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Set<UserModel>(), "Id", "Id", pageModel.UserId);
            return View(pageModel);
        }

        // GET: Page/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageModel = await _context.PageModel.FindAsync(id);
            if (pageModel == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Id", pageModel.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Set<UserModel>(), "Id", "Id", pageModel.UserId);
            return View(pageModel);
        }

        // POST: Page/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,CategoryId,CreatedAt,Link,Title,Description,Keywords,Image")] PageModel pageModel)
        {
            if (id != pageModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pageModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageModelExists(pageModel.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Id", pageModel.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Set<UserModel>(), "Id", "Id", pageModel.UserId);
            return View(pageModel);
        }

        // GET: Page/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageModel = await _context.PageModel
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pageModel == null)
            {
                return NotFound();
            }

            return View(pageModel);
        }

        // POST: Page/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pageModel = await _context.PageModel.FindAsync(id);
            if (pageModel != null)
            {
                _context.PageModel.Remove(pageModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PageModelExists(int id)
        {
            return _context.PageModel.Any(e => e.Id == id);
        }
    }
}
