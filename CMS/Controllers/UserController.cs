using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Enums;
using CMS.Models;

namespace CMS.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserModel.Include(u => u.IdentityUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel
                .Include(u => u.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            var availableUsers = _context.Users
                .Where(u => !_context.UserModel.Select(um => um.IdentityUserId).Contains(u.Id))
                .Select(u => new { u.Id, u.UserName })
                .ToList();

            ViewData["IdentityUserId"] = new SelectList(availableUsers, "Id", "UserName", GetCurrentUserIdentifierUserId());

            ViewData["CreatedAt"] = DateTime.Now.ToString("yyyy-MM-dd");

            ViewData["Role"] = EnumExtensions.ToSelectList<UserRolesEnum>();

            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] string identityUserId, [FromForm] DateTime createdAt, [FromForm] UserRolesEnum role)
        {
            var userModel = new UserModel
            {
                IdentityUserId = identityUserId,
                IdentityUser = await _context.Users.FindAsync(identityUserId),
                CreatedAt = createdAt,
                Role = role
            };

            if (ModelState.IsValid)
            {
                _context.Add(userModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var availableUsers = _context.Users
                .Where(u => !_context.UserModel.Select(um => um.IdentityUserId).Contains(u.Id))
                .Select(u => new { u.Id, u.UserName })
                .ToList();

            ViewData["IdentityUserId"] = new SelectList(availableUsers, "Id", "UserName", userModel.IdentityUserId);

            ViewData["CreatedAt"] = userModel.CreatedAt.ToString("yyyy-MM-dd");

            ViewData["Role"] = EnumExtensions.ToSelectList<UserRolesEnum>(userModel.Role);

            return View(userModel);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            var availableUsers = _context.Users
                .Where(u => !_context.UserModel.Select(um => um.IdentityUserId).Contains(u.Id) || u.Id == userModel.IdentityUserId)
                .Select(u => new { u.Id, u.UserName })
                .ToList();

            ViewData["IdentityUserId"] = new SelectList(availableUsers, "Id", "UserName", userModel.IdentityUserId);

            ViewData["Role"] = EnumExtensions.ToSelectList<UserRolesEnum>(userModel.Role);

            return View(userModel);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] string identityUserId, [FromForm] DateTime createdAt, [FromForm] UserRolesEnum role)
        {
            var userModel = await _context.UserModel.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            userModel.IdentityUserId = identityUserId;
            userModel.IdentityUser = await _context.Users.FindAsync(identityUserId);
            userModel.CreatedAt = createdAt;
            userModel.Role = role;

            if (ModelState.IsValid)
            {
                _context.Update(userModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var availableUsers = _context.Users
                .Where(u => !_context.UserModel.Select(um => um.IdentityUserId).Contains(u.Id) || u.Id == userModel.IdentityUserId)
                .Select(u => new { u.Id, u.UserName })
                .ToList();

            ViewData["IdentityUserId"] = new SelectList(availableUsers, "Id", "UserName", userModel.IdentityUserId);

            ViewData["Role"] = EnumExtensions.ToSelectList<UserRolesEnum>(userModel.Role);

            return View(userModel);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel
                .Include(u => u.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userModel = await _context.UserModel.FindAsync(id);
            if (userModel != null)
            {
                _context.UserModel.Remove(userModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private string? GetCurrentUserIdentifierUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
