CREATE PROCEDURE [Auth].[GetUserInvitationByInviteId]
	@inviteId INT
AS
	SET NOCOUNT ON;
	SELECT 
		[InvitationId],
		[Email],
		[FirstName],
		[LastName], 
		[OrganizationId], 
		[Invitation].[OrganizationRoleId],
		[Auth].[Organization].[OrganizationName],
		[Auth].[OrganizationRole].[OrganizationRoleName],
		[EmployeeId],
		[Invitation].[StatusId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	LEFT JOIN [Auth].[OrganizationRole] WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [Invitation].[OrganizationRoleId]
	LEFT JOIN [Auth].[Organizaion] WITH (NOLOCK) ON [Auth].[Organization].OrganizationId =  [Invitation].[OrganizationId] 
	WHERE [InvitationId] = @inviteId AND [IsActive] = 1