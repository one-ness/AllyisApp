CREATE PROCEDURE [Auth].[GetUserInvitations]
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