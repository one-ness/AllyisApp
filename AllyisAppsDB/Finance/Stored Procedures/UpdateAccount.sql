CREATE PROCEDURE [Finance].[UpdateAccount]
	@AccountId INT,
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
		UPDATE [Finance].[Account]
		SET [AccountName] = @AccountName, 
			[IsActive] = @IsActive, 
			[AccountTypeId] = @AccountTypeId, 
			[ParentAccountId] = @ParentAccountId
		WHERE [AccountId] = @AccountId

		SET @returnValue = SCOPE_IDENTITY();
	END
	SELECT @returnValue;
END

