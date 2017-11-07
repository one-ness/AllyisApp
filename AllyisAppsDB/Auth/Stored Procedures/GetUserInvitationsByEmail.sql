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
	[OrganizationName]
	[EmployeeId],
	[ProductRolesJson]
FROM [Auth].[Invitation]
WITH (NOLOCK)
WHERE [Email] = @email