CREATE PROCEDURE [Finance].[DeleteAccount]
	@accountId INT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM [Finance].[Account]
	WHERE [AccountId] = @accountId
END

