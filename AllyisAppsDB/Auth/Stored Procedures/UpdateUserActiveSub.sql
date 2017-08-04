CREATE PROCEDURE [Auth].[UpdateUserActiveSub]
	@userId INT,
	@subscriptionId INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[User]
	SET [LastUsedSubscriptionId] = @subscriptionId
	WHERE [UserId] = @userId
END
