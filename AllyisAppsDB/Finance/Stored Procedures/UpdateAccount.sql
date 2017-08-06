CREATE PROCEDURE [Finance].[UpdateAccount]
	@accountId INT,
	@accountName NVARCHAR(100),
	@isActive BIT,
	@accountTypeId INT,
	@parentAccountId INT,
	@returnValue INT
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
		UPDATE [Finance].[Account]
		SET [AccountName]		= @accountName, 
			[IsActive]			= @isActive, 
			[AccountTypeId]		= @accountTypeId, 
			[ParentAccountId]	= @parentAccountId
		WHERE [AccountId]		= @accountId

		SET @returnValue = SCOPE_IDENTITY();
	END
END

