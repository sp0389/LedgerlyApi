using FinanceApi.Domain.Interfaces;
using FinanceApi.Infrastructure.Persistence;
using FinanceApi.Application.Interfaces;
using FinanceApi.Application.Services;

namespace FinanceApi.Shared;

public static class Extensions
{
    public static IServiceCollection AddServiceGroup(this IServiceCollection services)
    {
        services.AddScoped<IBudgetCategoryRepository, BudgetCategoryRepository>();
        services.AddScoped<IBudgetCategoryService, BudgetCategoryService>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ITransactionService, TransactionService>();

        return services;
    }
}