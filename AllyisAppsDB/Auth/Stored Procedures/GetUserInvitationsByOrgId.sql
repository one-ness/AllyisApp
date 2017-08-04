CREATE PROCEDURE [Auth].[GetUserInvitationsByOrgId]
	@OrganizationId INT
AS
	SET NOCOUNT ON;
	SELECT 
		[InvitationId],
		[Email],
		[FirstName],
		[LastName], 
		[DateOfBirth], 
		[OrganizationId], 
		[AccessCode], 
		[Invitation].[OrganizationRoleId],
		[OrganizationRoleName] AS [OrganizationRoleName],
		[EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	LEFT JOIN [Auth].[OrganizationRole] WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [Invitation].[OrganizationRoleId]
	WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1