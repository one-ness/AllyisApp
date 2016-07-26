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
		[OrgRole],
		[ProjectId] 
	FROM [Auth].[Invitation]
	WITH (NOLOCK)
	WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1