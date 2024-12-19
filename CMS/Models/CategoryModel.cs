namespace CMS.Models;

public class CategoryModel
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public virtual UserModel User { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get; set; }

    public virtual ICollection<PageModel> Pages { get; set; } = new List<PageModel>();
}