using System.ComponentModel.DataAnnotations;

namespace CMS.Models;

public class CommentModel
{
    [Display(Name="Komentarz")]
    public int Id { get; set; }

    [Display(Name="Podstrona")]
    public int PageId { get; set; }
    public virtual PageModel Page { get; set; }

    [Display(Name="Użytkownik")]
    public int UserId { get; set; }
    public virtual UserModel User { get; set; }

    [DataType(DataType.Date)]
    [Display(Name="Data stworzenia")]
    public DateTime CreatedAt { get; set; }

    [Display(Name="Treść")]
    public string Description { get; set; }
}