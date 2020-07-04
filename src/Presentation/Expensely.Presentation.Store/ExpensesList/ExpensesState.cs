﻿using System.Collections.Generic;
using Expensely.Contracts.Expenses;

namespace Expensely.Presentation.Store.ExpensesList
{
    public class ExpensesState
    {
        public ExpensesState(bool isLoading, IReadOnlyCollection<ExpenseDto>? expenses)
        {
            IsLoading = isLoading;
            Expenses = expenses;
        }

        public bool IsLoading { get; }

        public IReadOnlyCollection<ExpenseDto>? Expenses { get; }
    }
}