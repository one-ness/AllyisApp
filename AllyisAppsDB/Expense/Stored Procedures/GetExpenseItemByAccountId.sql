﻿CREATE PROCEDURE [Expense].[GetExpenseItemsByAccountId]
	@accountId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[ExpenseItemId],
		[ExpenseItemName],
		[ItemDescription],
		[TransactionDate],
		[Amount],
		[ExpenseReportId],
		[AccountId],
		[AccountTypeId],
		[IsBillableToCustomer],
		[ExpenseItemCreatedUtc],
		[ExpenseItemModifiedUtc]
	FROM [Expense].[ExpenseItem] WITH (NOLOCK)
	WHERE AccountId = @accountId
END