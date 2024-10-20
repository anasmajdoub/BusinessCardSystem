using BizCardSystem.Domain.Abstractions;

namespace BizCardSystem.Domain.BusinessCards;

public interface IBusinessCardsRepository : IRepository<BusinessCard>
{
    Task<bool> IsEmailUniqueAsync(string email);
}
