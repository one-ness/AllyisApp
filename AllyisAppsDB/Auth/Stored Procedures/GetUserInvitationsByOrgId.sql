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
		[Invitation].[OrgRoleId],
		[Name] AS [OrgRoleName],
		[EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	LEFT JOIN [Auth].[OrgRole] WITH (NOLOCK) ON [OrgRole].[OrgRoleId] = [Invitation].[OrgRoleId]
	WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1