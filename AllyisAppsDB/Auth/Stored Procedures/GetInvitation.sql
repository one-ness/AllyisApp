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
		[Organization].[OrganizationName],
		[EmployeeId],
		[Invitation].[InvitationStatus],
		[Invitation].[ProductRolesJson]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	LEFT JOIN [Auth].[Organization] WITH (NOLOCK) ON [Auth].[Organization].OrganizationId =  [Invitation].[OrganizationId] 
	WHERE [InvitationId] = @inviteId