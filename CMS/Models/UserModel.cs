using CMS.Enums;

namespace CMS.Models;

public class UserModel
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Identify { get; set; }
    public UserRolesEnum Role { get; set; }

    public virtual ICollection<PageModel> Pages { get; set; } = new List<PageModel>();
    public virtual ICollection<EntryModel> Entries { get; set; } = new List<EntryModel>();
    public virtual ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
    public virtual ICollection<RateModel> Ratings { get; set; } = new List<RateModel>();
}