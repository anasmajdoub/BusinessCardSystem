using BizCardSystem.Domain.Shared.Filter;

namespace BizCardSystem.Domain.BusinessCards.Filters;

public class BusinessCardParameters : Filter
{
    public string? Name { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateofBirth { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}