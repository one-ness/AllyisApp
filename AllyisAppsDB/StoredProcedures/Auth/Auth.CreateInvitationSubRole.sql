CREATE PROCEDURE [Auth].[CreateInvitationSubRole]
	@InvitationId INT,
	@SubscriptionId INT,
	@ProductRoleId INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Auth].[InvitationSubRole] ([InvitationId], [SubscriptionId], [ProductRoleId])
	VALUES (@InvitationId, @SubscriptionId, @ProductRoleId);

	SELECT SCOPE_IDENTITY();
END