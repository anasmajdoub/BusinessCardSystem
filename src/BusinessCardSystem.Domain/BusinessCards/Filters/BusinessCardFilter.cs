using BizCardSystem.Domain.Enums;
using BizCardSystem.Domain.Shared.Filter;

namespace BizCardSystem.Domain.BusinessCards.Filters;

public class BusinessCardFilter : Filter
{
    public string? Name { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? DateofBirth { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
