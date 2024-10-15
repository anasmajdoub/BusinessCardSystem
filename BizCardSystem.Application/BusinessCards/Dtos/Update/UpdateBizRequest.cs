using BizCardSystem.Domain.Abstractions;
using BizCardSystem.Domain.Enums;
using BizCardSystem.Domain.Shared;

namespace BizCardSystem.Application.BusinessCards.Dtos.Update
{
    public class UpdateBizRequest : Dto
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public Address Address { get; set; }
    }
}
