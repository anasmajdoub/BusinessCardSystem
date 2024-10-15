using BizCardSystem.Domain.Abstractions;

namespace BizCardSystem.Application.BusinessCards.Dtos.Get;

public class GetBizResponse : Dto
{
    public string Name { get; set; }
    public string Gender { get; set; }
    public string DateofBirth { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Photo { get; set; }
    public string Address { get; set; }
}
