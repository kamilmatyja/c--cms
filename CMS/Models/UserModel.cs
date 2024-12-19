using CMS.Enums;
using Microsoft.AspNetCore.Identity;

namespace CMS.Models;

public class UserModel
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public IdentityUser User { get; set; }

    public DateTime CreatedAt { get; set; }

    public UserRolesEnum Role { get; set; }

    public virtual ICollection<PageModel> Pages { get; set; } = new List<PageModel>();
    public virtual ICollection<EntryModel> Entries { get; set; } = new List<EntryModel>();
    public virtual ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
    public virtual ICollection<RateModel> Ratings { get; set; } = new List<RateModel>();
}