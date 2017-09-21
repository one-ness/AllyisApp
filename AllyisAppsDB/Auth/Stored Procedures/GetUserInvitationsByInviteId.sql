CREATE PROCEDURE [Auth].[GetUserInvitationByInviteId]
	@inviteId INT
AS
	SET NOCOUNT ON;
	SELECT 
		[InvitationId],
		[Email],
		[FirstName],
		[LastName], 
		[Invitation].[OrganizationId], 
		[Invitation].[OrganizationRoleId],
		[Organization].[OrganizationName],
		[Auth].[OrganizationRole].[OrganizationRoleName],
		[EmployeeId],
		[Invitation].[StatusId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	LEFT JOIN [Auth].[OrganizationRole] WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [Invitation].[OrganizationRoleId]
	LEFT JOIN [Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] =  [Invitation].[OrganizationId] 
	WHERE [InvitationId] = @inviteId AND [Invitation].IsActive = 1