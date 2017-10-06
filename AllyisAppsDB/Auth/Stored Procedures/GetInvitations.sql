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
		[EmployeeId],
		[Invitation].[RoleJson]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	LEFT JOIN [Auth].[OrganizationRole] WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [Invitation].[OrganizationRoleId]
	WHERE [OrganizationId] = @organizationId AND [IsActive] = 1