CREATE PROCEDURE [Auth].[GetInvitations]
	@organizationId INT,
	@statusMask INT
AS

	SET NOCOUNT ON;

	SELECT * FROM [Invitation] WITH (NOLOCK)
	WHERE [OrganizationId] = @organizationId
	AND ([InvitationStatus] & @statusMask) > 0