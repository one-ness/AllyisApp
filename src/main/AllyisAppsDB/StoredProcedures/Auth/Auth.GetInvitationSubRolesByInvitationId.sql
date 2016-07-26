CREATE PROCEDURE [Auth].[GetInvitationSubRolesByInvitationId]
	@InvitationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [InvitationSubRole].[InvitationId],
			[SubscriptionId],
			[ProductRoleId]
	FROM [Auth].[InvitationSubRole] WITH (NOLOCK)
	JOIN [Auth].[Invitation] WITH (NOLOCK) ON [Invitation].[InvitationId] = [InvitationSubRole].[InvitationId]
	WHERE [InvitationSubRole].[InvitationId] = @InvitationId
	AND [Invitation].[IsActive] = 1;
END