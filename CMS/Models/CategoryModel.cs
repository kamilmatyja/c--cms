using System.ComponentModel.DataAnnotations;

namespace CMS.Models;

public class CategoryModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name="Użytkownik")]
    public int UserId { get; set; }
    public virtual UserModel User { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name="Data stworzenia")]
    public DateTime CreatedAt { get; set; }

    [Required]
    [Display(Name="Nazwa")]
    public string Name { get; set; }

    public virtual ICollection<PageModel> Pages { get; set; } = new List<PageModel>();
}