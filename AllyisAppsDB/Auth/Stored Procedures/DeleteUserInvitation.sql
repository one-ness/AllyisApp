CREATE PROCEDURE [Auth].[DeleteUserInvitation]
	@InvitationId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Auth].[Invitation] 
	SET [IsActive] = 0
	WHERE [InvitationId] = @InvitationId;
END