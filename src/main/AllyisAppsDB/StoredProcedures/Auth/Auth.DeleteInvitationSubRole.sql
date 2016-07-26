CREATE PROCEDURE [Auth].[DeleteInvitationSubRole]
	@InvitationId INT,
	@SubscriptionId INT
AS

BEGIN
	SET NOCOUNT ON;

	DELETE FROM [Auth].[InvitationSubRole] 
	WHERE [InvitationId] = @InvitationId 
	AND	[SubscriptionId] = @SubscriptionId;
END