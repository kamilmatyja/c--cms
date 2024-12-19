using System.ComponentModel.DataAnnotations;
using CMS.Enums;

namespace CMS.Models;

public class RateModel
{
    [Display(Name="Ocena")]
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
    [Display(Name="Wartość")]
    public RatingsEnum Rating { get; set; }
}