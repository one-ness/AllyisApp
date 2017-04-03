CREATE PROCEDURE [Auth].[UpdateEmailConfirmed]
	@UserId nvarchar (40) ,
	@confirmCode nvarchar (MAX)
AS
BEGIN
	UPDATE [Auth].[User]
	SET [EmailConfirmed] = 1
	WHERE [UserId] = @UserId AND [EmailConfirmationCode] = @confirmCode

	SELECT [UserId]
	FROM [Auth].[User] WITH (NOLOCK)
	WHERE [UserId] = @UserId AND [EmailConfirmationCode] = @confirmCode
END