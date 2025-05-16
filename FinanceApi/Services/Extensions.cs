using System;
using FinanceApi.DAL.Interface;
using FinanceApi.DAL.Repository;

namespace FinanceApi.Services;

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