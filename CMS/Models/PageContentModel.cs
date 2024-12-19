using System.ComponentModel.DataAnnotations;
using CMS.Enums;

namespace CMS.Models;

public class PageContentModel
{
    [Display(Name="Element podstrony")]
    public int Id { get; set; }

    [Required]
    [Display(Name="Podstrona")]
    public int PageId { get; set; }
    public virtual PageModel Page { get; set; }

    [Required]
    [Display(Name="Typ")]
    public ContentTypesEnum Type { get; set; }
    [Required]
    [Display(Name="Zawartość")]
    public string Value { get; set; }
    [Required]
    [Display(Name="Kolejność")]
    public int Order { get; set; }
}