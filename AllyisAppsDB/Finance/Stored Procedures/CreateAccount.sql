CREATE PROCEDURE [Finance].[CreateAccount]
	@AccountName NVARCHAR(100),
	@IsActive BIT,
	@AccountTypeId INT,
	@ParentAccountId INT,
	@returnValue INT
AS
BEGIN
    SET NOCOUNT ON;

	IF EXISTS (
		SELECT * FROM [Finance].[Account] WITH (NOLOCK)
		WHERE [AccountName] = @AccountName
	)
	BEGIN
		-- Account name is not unique
		SET @returnValue = -1;
	END
	ELSE
	BEGIN
		-- Create account
		INSERT INTO [Finance].[Account]
				([AccountName], 
				[IsActive], 
				[AccountTypeId], 
				[ParentAccountId])
		VALUES (@AccountName,
				@IsActive,
				@AccountTypeId,
				@ParentAccountId);

		SET @returnValue = SCOPE_IDENTITY();
	END
	SELECT @returnValue;
END

