CREATE PROCEDURE [Finance].[GetAccountsByAccountTypeId]
	@accountTypeId INT,
	@isActive BIT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[A].[AccountId],
		[A].[AccountName],
		[A].[SubscriptionId],
		[A].[IsActive],
		[A].[AccountTypeId],
		[T].[AccountTypeName],
		[A].[ParentAccountId]
	FROM [Finance].[Account] AS [A] WITH (NOLOCK)
		LEFT JOIN (SELECT * FROM [Finance].[AccountType] WHERE [AccountTypeId] = @accountTypeId) AS [T]
			ON [T].[AccountTypeId] = [A].[AccountTypeId]
	WHERE [A].[IsActive] = @isActive
END

