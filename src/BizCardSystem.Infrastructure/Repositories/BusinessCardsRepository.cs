using BizCardSystem.Domain.BusinessCards;
using BizCardSystem.Infrastructure.DataBaseConext;
using Microsoft.EntityFrameworkCore;

namespace BizCardSystem.Infrastructure.Repositories;

public class BusinessCardsRepository(ApplicationDbContext context) : Repository<BusinessCard>(context), IBusinessCardsRepository
{
    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        return !await Query.AnyAsync(x => x.Email == email);
    }
}
