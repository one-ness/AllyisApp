CREATE PROCEDURE [Auth].[GetNumberOfOrgSubscriptions]
	@OrgId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(DISTINCT [SubscriptionId]) 
		FROM [Billing].[Subscription] WITH (NOLOCK)
		LEFT JOIN [Auth].[OrganizationUser] WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Subscription].[OrganizationId]
		JOIN [Auth].[User]					WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
	WHERE 
		[OrganizationUser].[OrganizationId] = @OrgId
		AND [Subscription].[IsActive] = 1;
END