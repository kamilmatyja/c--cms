namespace CMS.Models;

public class PageModel
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public virtual UserModel User { get; set; }

    public int CategoryId { get; set; }
    public virtual CategoryModel Category { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Link { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Keywords { get; set; }
    public string Image { get; set; }

    public virtual ICollection<EntryModel> Entries { get; set; } = new List<EntryModel>();
    public virtual ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
    public virtual ICollection<RateModel> Ratings { get; set; } = new List<RateModel>();
    public virtual ICollection<PageContentModel> Contents { get; set; } = new List<PageContentModel>();
}