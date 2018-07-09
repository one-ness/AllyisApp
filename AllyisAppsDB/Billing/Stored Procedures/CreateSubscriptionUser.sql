CREATE PROCEDURE [Billing].[CreateSubscriptionUser]
	@subscriptionId INT,
	@userId INT,
	@prodRoleId INT
AS
BEGIN
	SET NOCOUNT ON;
    
	--Create the new subscription user
	INSERT INTO [Billing].[SubscriptionUser]
		([SubscriptionId],
		[UserId],
		[ProductRoleId],
		[SubscriptionUserCreatedUTC])
	VALUES
		(@subscriptionId,
		@userId,
		@prodRoleId,
		SYSDATETIME());
END
GO