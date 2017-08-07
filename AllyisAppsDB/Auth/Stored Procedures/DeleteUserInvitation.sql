CREATE PROCEDURE [Auth].[DeleteUserInvitation]
	@invitationId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Auth].[Invitation] 
	SET [IsActive] = 0
	WHERE [InvitationId] = @invitationId;
END