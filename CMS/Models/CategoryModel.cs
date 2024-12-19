using System.ComponentModel.DataAnnotations;

namespace CMS.Models;

public class CategoryModel
{
    [Display(Name="Kategoria")]
    public int Id { get; set; }

    [Display(Name="Użytkownik")]
    public int UserId { get; set; }
    public virtual UserModel User { get; set; }

    [DataType(DataType.Date)]
    [Display(Name="Data stworzenia")]
    public DateTime CreatedAt { get; set; }

    [Display(Name="Nazwa")]
    public string Name { get; set; }

    public virtual ICollection<PageModel> Pages { get; set; } = new List<PageModel>();
}