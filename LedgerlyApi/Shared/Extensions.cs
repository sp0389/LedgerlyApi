using LedgerlyApi.Domain.Interfaces;
using LedgerlyApi.Infrastructure.Persistence;
using LedgerlyApi.Application.Interfaces;
using LedgerlyApi.Application.Services;

namespace LedgerlyApi.Shared;

public static class Extensions
{
    public static IServiceCollection AddServiceGroup(this IServiceCollection services)
    {
        services.AddScoped<IBudgetCategoryRepository, BudgetCategoryRepository>();
        services.AddScoped<IBudgetCategoryService, BudgetCategoryService>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}