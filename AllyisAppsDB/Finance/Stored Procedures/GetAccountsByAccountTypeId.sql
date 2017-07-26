CREATE PROCEDURE [Finance].[GetAccountsByAccountTypeId]
	@AccountTypeId INT,
	@IsActive BIT
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
		LEFT JOIN (SELECT * FROM [Finance].[AccountType] WHERE [AccountTypeId] = @AccountTypeId) AS [T]
			ON [T].[AccountTypeId] = [A].[AccountTypeId]
	WHERE [A].[IsActive] = @IsActive
END

