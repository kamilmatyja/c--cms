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
            var applicationDbContext = _context.EntryModel.Include(e => e.Page).Include(e => e.User);
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
                .Include(e => e.User)
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
            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Set<UserModel>(), "Id", "Id");
            return View();
        }

        // POST: Entry/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PageId,UserId,CreatedAt")] EntryModel entryModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entryModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Id", entryModel.PageId);
            ViewData["UserId"] = new SelectList(_context.Set<UserModel>(), "Id", "Id", entryModel.UserId);
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
            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Id", entryModel.PageId);
            ViewData["UserId"] = new SelectList(_context.Set<UserModel>(), "Id", "Id", entryModel.UserId);
            return View(entryModel);
        }

        // POST: Entry/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PageId,UserId,CreatedAt")] EntryModel entryModel)
        {
            if (id != entryModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entryModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryModelExists(entryModel.Id))
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
            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Id", entryModel.PageId);
            ViewData["UserId"] = new SelectList(_context.Set<UserModel>(), "Id", "Id", entryModel.UserId);
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

        private bool EntryModelExists(int id)
        {
            return _context.EntryModel.Any(e => e.Id == id);
        }
    }
}
