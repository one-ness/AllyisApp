--DECLARE @IserId VARCHAR(40) = '105b34f5-8b59-4f01-b42a-ae9de49ed71f';
CREATE PROCEDURE [Billing].[GetSubscriptionDetailsByUser]
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
	[Organization].[OrganizationId] AS [OrganizationId],
	[ProductId]						AS [ProductId],
	[Organization].[Name]			AS [OrganizationName],
	[Sku].[Name]					AS [SkuName],
	[Subscription].[SubscriptionId] AS [SubscriptionId],
	[Sku].[SkuId]					AS [SkuID],
	[Sku].[Name]					AS [SkuName]
	FROM
	(
		(SELECT [UserId], [OrganizationId], [EmployeeId], [OrgRoleId], [TTLockDate], [CreatedUTC], [ModifiedUTC]
		 FROM [Auth].[OrganizationUser] WITH (NOLOCK) 
		WHERE [OrganizationUser].[UserId] = @UserId) AS [UserOrg]
		JOIN [Auth].[Organization]		WITH (NOLOCK) ON [Organization].[OrganizationId] = [UserOrg].[OrganizationId]
		JOIN [Billing].[Subscription]	WITH (NOLOCK) ON [Subscription].[OrganizationId] = [Organization].[OrganizationId]
		JOIN [Billing].[Sku]			WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
		JOIN [Auth].[User]				WITH (NOLOCK) ON [User].[UserId] = @UserId
	)
	WHERE [Subscription].[IsActive] = 1
	ORDER BY [OrgRoleId] DESC, [Organization].[Name] ASC, [BillingFrequency] DESC
END