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
            var applicationDbContext = _context.PageModel
                .Include(p => p.Category)
                .Include(p => p.ParentPage)
                .Include(c => c.User)
                .ThenInclude(u => u.IdentityUser);
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
                .Include(p => p.ParentPage)
                .Include(p => p.User)
                .ThenInclude(u => u.IdentityUser)
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
            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", GetUserId());

            ViewData["ParentPageId"] = new SelectList(_context.PageModel, "Id", "Title");

            ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Name");

            ViewData["CreatedAt"] = DateTime.Now.ToString("yyyy-MM-dd");

            return View();
        }

        // POST: Page/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] int userId, [FromForm] int? parentPageId, [FromForm] int categoryId, [FromForm] DateTime createdAt, [FromForm] string link, [FromForm] string title, [FromForm] string description, [FromForm] string keywords, [FromForm] IFormFile? image, [FromForm] List<IFormFile> contents)
        {
            var pageModel = new PageModel
            {
                UserId = userId,
                User = await _context.UserModel.FindAsync(userId),
                CategoryId = categoryId,
                Category = await _context.CategoryModel.FindAsync(categoryId),
                CreatedAt = createdAt,
                Link = link,
                Title = title,
                Description = description,
                Keywords = keywords
            };

            if (parentPageId != null)
            {
                pageModel.ParentPageId = parentPageId;
                pageModel.ParentPage = await _context.PageModel.FindAsync(parentPageId);
            }

            if (image == null || image.Length == 0)
            {
                ModelState.AddModelError("Image", "Please upload an image.");
            }
            else
            {
                var allowedExtensions = new[] { ".webp", ".jpeg", ".jpg", ".png", ".gif" };
                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Image", "This file extension is not allowed.");
                }
                else
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        pageModel.Image = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }

            foreach (var content in contents)
            {
                var pageContent = new PageContentModel();

                if (content.ContentType.StartsWith("image/"))
                {
                    var allowedExtensions = new[] { ".webp", ".jpeg", ".jpg", ".png", ".gif" };
                    var extension = Path.GetExtension(content.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("Contents", "This file extension is not allowed.");
                        return View(pageModel);
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        await content.CopyToAsync(memoryStream);
                        pageContent.PageId = pageModel.Id;
                        pageContent.Page = pageModel;
                        pageContent.Value = Convert.ToBase64String(memoryStream.ToArray());
                        pageContent.Type = ContentTypesEnum.Image;
                    }
                }
                else
                {
                    using (var reader = new StreamReader(content.OpenReadStream()))
                    {
                        pageContent.PageId = pageModel.Id;
                        pageContent.Page = pageModel;
                        pageContent.Value = await reader.ReadToEndAsync();
                        pageContent.Type = ContentTypesEnum.Text;
                    }
                }

                pageModel.Contents.Add(pageContent);
            }

            if (ModelState.IsValid)
            {
                _context.Add(pageModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", pageModel.UserId);

            ViewData["ParentPageId"] = new SelectList(_context.PageModel, "Id", "Title", pageModel.ParentPageId);

            ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Name", pageModel.CategoryId);

            ViewData["CreatedAt"] = DateTime.Now.ToString("yyyy-MM-dd");

            ViewData["Contents"] = pageModel.Contents;

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

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", pageModel.UserId);

            ViewData["ParentPageId"] = new SelectList(
                _context.PageModel.Where(p => p.Id != pageModel.Id),
                "Id",
                "Title",
                pageModel.ParentPageId
            );

            ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Name", pageModel.CategoryId);

            return View(pageModel);
        }

        // POST: Page/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] int userId, [FromForm] int? parentPageId, [FromForm] int categoryId, [FromForm] DateTime createdAt, [FromForm] string link, [FromForm] string title, [FromForm] string description, [FromForm] string keywords, [FromForm] IFormFile? image)
        {
            var pageModel = await _context.PageModel.FindAsync(id);
            if (pageModel == null)
            {
                return NotFound();
            }

            pageModel.UserId = userId;
            pageModel.User = await _context.UserModel.FindAsync(userId);

            if (parentPageId != null)
            {
                pageModel.ParentPageId = parentPageId;
                pageModel.ParentPage = await _context.PageModel.FindAsync(parentPageId);
            }

            pageModel.CategoryId = categoryId;
            pageModel.Category = await _context.CategoryModel.FindAsync(categoryId);
            pageModel.CreatedAt = createdAt;
            pageModel.Link = link;
            pageModel.Title = title;
            pageModel.Description = description;
            pageModel.Keywords = keywords;

            if (image != null && image.Length > 0)
            {
                var allowedExtensions = new[] { ".webp", ".jpeg", ".jpg", ".png", ".gif" };
                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Image", "This file extension is not allowed.");
                }
                else
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        pageModel.Image = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }

            if (ModelState.IsValid)
            {
                _context.Update(pageModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var users = _context.UserModel
                .Include(u => u.IdentityUser)
                .Select(u => new { u.Id, UserName = u.IdentityUser.UserName })
                .ToList();

            ViewData["UserId"] = new SelectList(users, "Id", "UserName", pageModel.UserId);

            ViewData["ParentPageId"] = new SelectList(
                _context.PageModel.Where(p => p.Id != pageModel.Id),
                "Id",
                "Title",
                pageModel.ParentPageId
            );

            ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Name", pageModel.CategoryId);

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
                .Include(p => p.ParentPage)
                .Include(p => p.User)
                .ThenInclude(u => u.IdentityUser)
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
