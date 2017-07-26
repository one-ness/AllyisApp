CREATE PROCEDURE [Finance].[DeleteAccount]
	@AccountId INT
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS( SELECT * FROM [Finance].[Account] WHERE [AccountId] = @AccountId AND [IsActive] = 1)
	BEGIN
		UPDATE [Finance].[Account] 
		SET [IsActive] = 0
		WHERE [AccountId] = @AccountId;

		SELECT 1;
	END
	ELSE
	BEGIN
		SELECT 0;
	END
END

