

GO
PRINT N'Dropping [Auth].[GetUserInvitationByInviteId]...';


GO
DROP PROCEDURE [Auth].[GetUserInvitationByInviteId];


GO
PRINT N'Dropping [Auth].[GetUserInvitationsByOrgId]...';


GO
DROP PROCEDURE [Auth].[GetUserInvitationsByOrgId];


GO
PRINT N'Dropping [Auth].[GetUserInvitationsByUserData]...';


GO
DROP PROCEDURE [Auth].[GetUserInvitationsByUserData];


GO
PRINT N'Altering [aaUser]...';


GO
ALTER USER [aaUser]
    WITH LOGIN = [aaUser];


GO
PRINT N'Altering [Auth].[GetOrgUserList]...';


GO
ALTER PROCEDURE [Auth].[GetOrgUserList]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [OU].[OrganizationId],
	       [OU].[UserId],
		   [OU].[OrganizationRoleId],
		   [O].[OrganizationName] AS [OrganizationName],
		   [OU].[EmployeeId],
		   [U].[Email],
		   [U].[FirstName],
		   [U].[LastName],
		   [OU].OrganizationUserCreatedUtc as 'CreatedUtc'
    FROM [Auth].[OrganizationUser]	AS [OU]
	WITH (NOLOCK)
    INNER JOIN [Auth].[User]		AS [U] WITH (NOLOCK) 
		ON [U].[UserId] = [OU].[UserId]
	INNER JOIN [Auth].[Organization] AS [O] WITH (NOLOCK)
		ON [O].[OrganizationId] = [OU].[OrganizationId]
    WHERE [OU].[OrganizationId] = @organizationId
	ORDER BY [U].[LastName]
END
GO
PRINT N'Altering [Billing].[GetSubscriptionDetailsById]...';


GO
ALTER PROCEDURE [Billing].[GetSubscriptionDetailsById]
	@subscriptionId INT
AS
	SET NOCOUNT ON;
SELECT [OrganizationId]
      ,[Subscription].[SkuId]
	  ,[Subscription].[SubscriptionId]
	  ,[NumberOfUsers]
      ,[SubscriptionCreatedUtc]
      ,[Subscription].[IsActive]
	  ,[SubscriptionName] As 'Name'
	  ,[Sku].[ProductId]
FROM [Billing].[Subscription] WITH (NOLOCK) 
JOIN [Billing].[Sku] ON [Sku].SkuId = [Subscription].[SkuId]
WHERE [SubscriptionId] = @subscriptionId AND [Subscription].[IsActive] = 1
GO
PRINT N'Altering [Billing].[GetSubscriptionsDisplayByOrg]...';


GO
ALTER PROCEDURE [Billing].[GetSubscriptionsDisplayByOrg]
	@organizationId INT
AS
	SET NOCOUNT ON;
SELECT	[Product].[ProductId],
		[Product].[ProductName] AS [ProductName],
		[Subscription].[SubscriptionId],
		[Organization].[OrganizationId],
		[Subscription].[SkuId],
		[Subscription].[SubscriptionCreatedUtc] as 'CreatedUtc',
		[Subscription].[NumberOfUsers],
		[Subscription].[SubscriptionName],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Sku].[SkuName] AS [SkuName]
  FROM [Billing].[Subscription] WITH (NOLOCK) 
  LEFT JOIN [Billing].[Sku]			WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
  LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
  LEFT JOIN [Billing].[Product]		WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
  WHERE [Subscription].[OrganizationId] = @organizationId
	AND [Subscription].[IsActive] = 1
ORDER BY [Product].[ProductName]
GO
PRINT N'Creating [Auth].[GetInvitation]...';


GO
CREATE PROCEDURE [Auth].[GetInvitation]
	@inviteId INT
AS
	SET NOCOUNT ON;
	SELECT 
		[InvitationId],
		[Email],
		[FirstName],
		[LastName], 
		[Organization].[OrganizationId], 
		[Invitation].[OrganizationRoleId],
		[Organization].[OrganizationName],
		[Auth].[OrganizationRole].[OrganizationRoleName],
		[EmployeeId],
		[Invitation].[StatusId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	LEFT JOIN [Auth].[OrganizationRole] WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [Invitation].[OrganizationRoleId]
	LEFT JOIN [Auth].[Organization] WITH (NOLOCK) ON [Auth].[Organization].OrganizationId =  [Invitation].[OrganizationId] 
	WHERE [InvitationId] = @inviteId AND [Invitation].[IsActive] = 1
GO
PRINT N'Creating [Auth].[GetInvitations]...';


GO
CREATE PROCEDURE [Auth].[GetInvitations]
	@organizationId INT
AS
	SET NOCOUNT ON;
	SELECT 
		[InvitationId],
		[Email],
		[FirstName],
		[LastName],  
		[OrganizationId], 
		[Invitation].[OrganizationRoleId],
		[OrganizationRoleName] AS [OrganizationRoleName],
		[EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	LEFT JOIN [Auth].[OrganizationRole] WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [Invitation].[OrganizationRoleId]
	WHERE [OrganizationId] = @organizationId AND [IsActive] = 1
GO
PRINT N'Creating [Auth].[GetMaxEmployeeId]...';


GO
CREATE procedure Auth.GetMaxEmployeeId
	@organizationId int
as
begin
	set nocount on
	select top 1 EmployeeId from (select EmployeeId from Invitation where OrganizationId = @organizationId union select EmployeeId from OrganizationUser where OrganizationId = @organizationId) as EmployeeId
	order by EmployeeId COLLATE Latin1_General_BIN desc
end
GO
PRINT N'Creating [Auth].[GetUserInvitationsByEmail]...';


GO
CREATE PROCEDURE [Auth].[GetUserInvitationsByEmail]
	@email NVARCHAR(384)
	
AS
	SET NOCOUNT ON;
SELECT 
	[InvitationId], 
	[Email], 
	[FirstName], 
	[LastName], 
	[OrganizationId],  
	[OrganizationRoleId],
	[EmployeeId] 
FROM [Auth].[Invitation]
WITH (NOLOCK)
WHERE [Email] = @email AND [IsActive] = 1
GO
PRINT N'Creating [Billing].[GetSubscriptionUsersBySubscriptionId]...';


GO
CREATE PROCEDURE [Billing].[GetSubscriptionUsersBySubscriptionId]
	@subscriptionId int
AS
	SELECT 
	[Auth].[User].FirstName,
	[Auth].[User].LastName,
	[Billing].[SubscriptionUser].[ProductRoleId],
	[Auth].[User].UserId,
	[Auth].[User].Email,
	[Billing].[SubscriptionUser].ProductRoleId,
	[Billing].[SubscriptionUser].SubscriptionUserCreatedUtc as 'CreatedUtc',
	[Billing].[SubscriptionUser].[SubscriptionId],
	[Billing].[Sku].ProductId
	FROM 
	[Billing].SubscriptionUser
	JOIN [Auth].[User] ON [Auth].[User].UserId = [SubscriptionUser].UserId
	JOIN [Billing].[Subscription] ON [Billing].[Subscription].SubscriptionId = [Billing].[SubscriptionUser].SubscriptionId
	JOIN [Billing].[Sku] ON [Billing].[Sku].[SkuId] = [Billing].[Subscription].[SkuId]
	WHERE  [SubscriptionUser].SubscriptionId = @subscriptionId;
GO
PRINT N'Update complete.';


GO
