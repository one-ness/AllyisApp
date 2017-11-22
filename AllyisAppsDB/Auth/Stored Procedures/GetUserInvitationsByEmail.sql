CREATE PROCEDURE [Auth].[GetUserInvitationsByEmail]
	@email NVARCHAR(384)
	
AS
	SET NOCOUNT ON;
SELECT 
	[InvitationId], 
	[Email], 
	[FirstName], 
	[LastName], 
	[INV].[OrganizationId],
	[ORG].[OrganizationName],
	[EmployeeId],
	[ProductRolesJson]
FROM 
([Auth].[Invitation] AS [INV] WITH (NOLOCK)
JOIN [Auth].[Organization] AS [ORG] ON [ORG].[OrganizationId] = [INV].[OrganizationId]) 

WHERE [Email] = @email