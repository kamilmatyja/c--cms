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
            var applicationDbContext = _context.RateModel.Include(r => r.Page).Include(r => r.User);
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
            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Set<UserModel>(), "Id", "Id");
            return View();
        }

        // POST: Rate/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PageId,UserId,CreatedAt,Rating")] RateModel rateModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rateModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Id", rateModel.PageId);
            ViewData["UserId"] = new SelectList(_context.Set<UserModel>(), "Id", "Id", rateModel.UserId);
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
            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Id", rateModel.PageId);
            ViewData["UserId"] = new SelectList(_context.Set<UserModel>(), "Id", "Id", rateModel.UserId);
            return View(rateModel);
        }

        // POST: Rate/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PageId,UserId,CreatedAt,Rating")] RateModel rateModel)
        {
            if (id != rateModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rateModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RateModelExists(rateModel.Id))
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
            ViewData["PageId"] = new SelectList(_context.PageModel, "Id", "Id", rateModel.PageId);
            ViewData["UserId"] = new SelectList(_context.Set<UserModel>(), "Id", "Id", rateModel.UserId);
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

        private bool RateModelExists(int id)
        {
            return _context.RateModel.Any(e => e.Id == id);
        }
    }
}
