CREATE PROCEDURE [Finance].[GetAccounts]
	@subscriptionId INT
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
		LEFT JOIN [Finance].[AccountType] AS [T] WITH (NOLOCK) ON [T].[AccountTypeId] = [A].[AccountTypeId]
	WHERE [SubscriptionId] = @subscriptionId
END

