using CMS.Enums;

namespace CMS.Models;

public class PageContentModel
{
    public int Id { get; set; }

    public int PageId { get; set; }
    public virtual PageModel Page { get; set; }

    public ContentTypesEnum Type { get; set; }
    public string Value { get; set; }
    public int Order { get; set; }
}