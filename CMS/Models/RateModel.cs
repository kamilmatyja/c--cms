using System.ComponentModel.DataAnnotations;
using CMS.Enums;

namespace CMS.Models;

public class RateModel
{
    [Display(Name="Ocena")]
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

    [Display(Name="Wartość")]
    public RatingsEnum Rating { get; set; }
}