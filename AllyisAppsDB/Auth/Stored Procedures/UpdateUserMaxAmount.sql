CREATE PROCEDURE [Auth].[UpdateUserMaxAmount]
	@userId int,
	@maxAmount nvarchar(512)
AS
BEGIN
	SET NOCOUNT ON
	UPDATE [Auth].[User]
	SET [MaxAmount] = @maxAmount
	WHERE [UserId] = @userId
END
