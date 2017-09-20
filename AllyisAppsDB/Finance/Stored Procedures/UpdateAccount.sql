CREATE PROCEDURE [Finance].[UpdateAccount]
	@accountId INT,
	@accountName NVARCHAR(100),
	@isActive BIT,
	@accountTypeId INT,
	@parentAccountId INT,
	@returnValue INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN
		-- Create account
		UPDATE [Finance].[Account]
		SET [AccountName]		= @accountName, 
			[IsActive]			= @isActive, 
			[AccountTypeId]		= @accountTypeId, 
			[ParentAccountId]	= @parentAccountId
		WHERE [AccountId]		= @accountId

		SET @returnValue = 1;
	END
END

