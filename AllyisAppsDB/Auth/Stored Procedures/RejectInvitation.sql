CREATE PROCEDURE [Auth].[RejectInvitation]
	@invitationId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Auth].[Invitation] 
	SET    [StatusId] = -1, DecisionDateUtc = GETUTCDATE()
	WHERE [InvitationId] = @invitationId;
END