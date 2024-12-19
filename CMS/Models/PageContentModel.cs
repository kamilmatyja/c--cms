using System.ComponentModel.DataAnnotations;
using CMS.Enums;

namespace CMS.Models;

public class PageContentModel
{
    [Display(Name="Element podstrony")]
    public int Id { get; set; }

    [Display(Name="Podstrona")]
    public int PageId { get; set; }
    public virtual PageModel Page { get; set; }

    [Display(Name="Typ")]
    public ContentTypesEnum Type { get; set; }
    [Display(Name="Zawartość")]
    public string Value { get; set; }
    [Display(Name="Kolejność")]
    public int Order { get; set; }
}