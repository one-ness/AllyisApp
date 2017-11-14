CREATE PROCEDURE [Crm].[GetNextCustId]
	@orgId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 1
		[CustomerCode]
	FROM [Crm].[Customer] WITH (NOLOCK)
	WHERE [Customer].[OrganizationId] = @orgId
	ORDER BY [CustomerCode] DESC;
END