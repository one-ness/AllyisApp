CREATE PROCEDURE [Finance].[GetAccountsByParentId]
	@ParentAccountId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[AccountId],
		[AccountName],
		[IsActive],
		[ACC].[AccountTypeId],
		[AccountTypeName],
		[ParentAccountId]
	FROM [Finance].[Account] AS [ACC] WITH (NOLOCK)
		LEFT JOIN [Finance].[AccountType] WITH (NOLOCK) ON [AccountType].[AccountTypeId] = [ACC].[AccountTypeId]
	WHERE ParentAccountId = @ParentAccountId
END