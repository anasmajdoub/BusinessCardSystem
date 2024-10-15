using BizCardSystem.Application.Abstractions.Data;
using BizCardSystem.Domain.Abstractions;
using BizCardSystem.Domain.BusinessCards;
using BizCardSystem.Infrastructure.Data;
using BizCardSystem.Infrastructure.DataBaseConext;
using BizCardSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BizCardSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IBusinessCardsRepository, BusinessCardsRepository>();
        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        return services;
    }
}
