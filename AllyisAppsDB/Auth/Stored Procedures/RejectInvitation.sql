CREATE PROCEDURE [Auth].[RejectInvitation]
	@invitationId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Auth].[Invitation] 
	SET    [InvitationStatus] = 2, DecisionDateUtc = GETUTCDATE()
	WHERE [InvitationId] = @invitationId;

	SELECT @@ROWCOUNT;
END