using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CMS.Models;

namespace CMS.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<CMS.Models.CategoryModel> CategoryModel { get; set; } = default!;

    public DbSet<CMS.Models.CommentModel> CommentModel { get; set; } = default!;

    public DbSet<CMS.Models.PageModel> PageModel { get; set; } = default!;

    public DbSet<CMS.Models.EntryModel> EntryModel { get; set; } = default!;

    public DbSet<CMS.Models.RateModel> RateModel { get; set; } = default!;

    public DbSet<CMS.Models.UserModel> UserModel { get; set; } = default!;
}