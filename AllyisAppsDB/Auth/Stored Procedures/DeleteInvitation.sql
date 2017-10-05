CREATE PROCEDURE [Auth].[DeleteInvitation]
	@invitationId INT
AS
BEGIN
	BEGIN TRANSACTION
			DELETE FROM [Auth].[Invitation]
			WHERE [InvitationId] = @invitationId
			
			SELECT @@ROWCOUNT
	COMMIT

	
END