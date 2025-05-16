using System;
using FinanceApi.DAL.Interface;
using FinanceApi.Models;
using FinanceApi.Models.DTO;

namespace FinanceApi.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactions()
    {
        return await _transactionRepository.GetAllTransactionsAsync();
    }

    public async Task<Transaction> GetTransactionsById(int transactionId)
    {
        return await _transactionRepository.GetTransactionByIdAsync(transactionId);
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByCategoryId(int categoryId)
    {
        return await _transactionRepository.GetTransactionsByCategoryIdAsync(categoryId);
    }

    public async Task<bool> AddTransaction(TransactionDTO transaction)
    {
        return await _transactionRepository.AddTransactionAsync(transaction);
    }

    public async Task<Transaction> UpdateTransaction(TransactionDTO transaction)
    {
        return await _transactionRepository.UpdateTransactionAsync(transaction);
    }

    public async Task<bool> RemoveTransactionById(int transactionId)
    {
        return await _transactionRepository.RemoveTransactionByIdAsync(transactionId);
    }
}
