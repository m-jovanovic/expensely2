CREATE INDEX "IDX_Expense_UserId_Date_CreatedOnUtc" ON "Expense" ("UserId" ASC, "Date" DESC, "CreatedOnUtc" DESC)
WHERE NOT "Deleted";