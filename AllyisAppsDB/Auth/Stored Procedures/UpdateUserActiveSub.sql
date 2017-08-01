CREATE PROCEDURE [Auth].[UpdateUserActiveSub]
	@UserId INT,
	@SubscriptionId INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[User]
	SET [LastUsedSubscriptionId] = @SubscriptionId
	WHERE [UserId] = @UserId
END