namespace CMS.Models;

public class EntryModel
{
    public int Id { get; set; }

    public int PageId { get; set; }
    public virtual PageModel Page { get; set; }

    public int? UserId { get; set; }
    public virtual UserModel User { get; set; }

    public DateTime CreatedAt { get; set; }
}