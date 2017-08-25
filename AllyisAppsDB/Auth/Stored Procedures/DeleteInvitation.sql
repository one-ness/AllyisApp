CREATE PROCEDURE [Auth].[DeleteInvitation]
	@invitationId INT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
			DELETE FROM [Auth].[Invitation]
			WHERE [InvitationId] = @invitationId
	COMMIT
END