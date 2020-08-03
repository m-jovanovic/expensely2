﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Contracts.Expenses;
using Expensely.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Expenses.Queries.GetExpenses
{
    /// <summary>
    /// Represents the <see cref="GetExpensesQuery"/> handler.
    /// </summary>
    internal sealed class GetExpensesQueryHandler : IQueryHandler<GetExpensesQuery, IReadOnlyCollection<ExpenseResponse>>
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpensesQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GetExpensesQueryHandler(IDbContext dbContext) => _dbContext = dbContext;

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<ExpenseResponse>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<ExpenseResponse> expenses = await _dbContext.Set<Expense>()
                .AsNoTracking()
                .Select(e => new ExpenseResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    Amount = e.Money.Amount,
                    CurrencyId = e.Money.Currency.Id,
                    CurrencyCode = e.Money.Currency.Code,
                    Date = e.Date,
                    CreatedOnUtc = e.CreatedOnUtc,
                    ModifiedOnUtc = e.ModifiedOnUtc,
                    Deleted = e.Deleted
                }).ToListAsync(cancellationToken);

            return expenses;
        }
    }
}