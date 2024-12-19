using System.ComponentModel.DataAnnotations;

namespace CMS.Models;

public class PageModel
{
    [Display(Name="Podstrona")]
    public int Id { get; set; }

    [Display(Name="Użytkownik")]
    public int UserId { get; set; }
    public virtual UserModel User { get; set; }

    [Display(Name="Kategoria")]
    public int CategoryId { get; set; }
    public virtual CategoryModel Category { get; set; }

    [DataType(DataType.Date)]
    [Display(Name="Data stworzenia")]
    public DateTime CreatedAt { get; set; }

    [Display(Name="Link")]
    public string Link { get; set; }
    [Display(Name="Tytuł")]
    public string Title { get; set; }
    [Display(Name="Treść")]
    public string Description { get; set; }
    [Display(Name="Słowa kluczowe")]
    public string Keywords { get; set; }
    [Display(Name="Zdjęcie")]
    public string Image { get; set; }

    public virtual ICollection<EntryModel> Entries { get; set; } = new List<EntryModel>();
    public virtual ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
    public virtual ICollection<RateModel> Ratings { get; set; } = new List<RateModel>();
    public virtual ICollection<PageContentModel> Contents { get; set; } = new List<PageContentModel>();
}