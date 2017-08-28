CREATE PROCEDURE [Auth].[RejectInvitation]
	@invitationId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Auth].[Invitation] 
	SET    [StatusId] = -1, DecisionDate = GETUTCDATE()
	WHERE [InvitationId] = @invitationId;
END