CREATE PROCEDURE [Finance].[GetAccountByAccountId]
	@AccountId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[A].[AccountId],
		[A].[AccountName],
		[A].[IsActive],
		[A].[AccountTypeId],
		[T].[AccountTypeName],
		[A].[ParentAccountId]
	FROM [Finance].[Account] AS [A] WITH (NOLOCK)
		LEFT JOIN [Finance].[AccountType] AS [T] WITH (NOLOCK) ON [T].[AccountTypeId] = [A].[AccountTypeId]
	WHERE [A].[AccountId] = @AccountId
END

