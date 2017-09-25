CREATE PROCEDURE [Finance].[CreateAccount]
	@accountName NVARCHAR(100),
	@isActive BIT,
	@accountTypeId INT,
	@parentAccountId INT,
	@returnValue INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

	IF EXISTS (
		SELECT * FROM [Finance].[Account] WITH (NOLOCK)
		WHERE [AccountName] = @accountName
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
		VALUES (@accountName,
				@isActive,
				@accountTypeId,
				@parentAccountId);

		SET @returnValue = SCOPE_IDENTITY();
	END
END

