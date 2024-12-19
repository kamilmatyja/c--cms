using System.ComponentModel.DataAnnotations;

namespace CMS.Models;

public class CommentModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name="Podstrona")]
    public int PageId { get; set; }
    public virtual PageModel Page { get; set; }

    [Required]
    [Display(Name="Użytkownik")]
    public int UserId { get; set; }
    public virtual UserModel User { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name="Data stworzenia")]
    public DateTime CreatedAt { get; set; }

    [Required]
    [Display(Name="Treść")]
    public string Description { get; set; }
}