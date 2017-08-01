CREATE PROCEDURE [Finance].[DeleteAccount]
	@accountId INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Finance].[Account] 
	SET [IsActive] = 0
	WHERE [AccountId] = @accountId;
END

