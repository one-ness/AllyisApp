CREATE PROCEDURE [Billing].[CreateSubscription]
	@organizationId INT,
	@skuId INT,
	@subscriptionName NVARCHAR(50),
	@userId INT
AS
BEGIN
	SET NOCOUNT ON;

	--Create the new subscription
	INSERT INTO [Billing].[Subscription]
		([OrganizationId],
		[SkuId],
		[SubscriptionName])
	VALUES
		(@organizationId,
		@skuId,
		@subscriptionName);

	DECLARE @subscriptionId INT = IDENT_CURRENT('[Billing].[Subscription]');

	-- Insert all members of given org to SubscriptionUser table with User role
	-- ASSUMPTION: 1 is the productRoleId of "User" for all subscriptions
	INSERT INTO [Billing].[SubscriptionUser] ([UserId], [SubscriptionId], [ProductRoleId])
		SELECT [UserId], @subscriptionId, 1
		FROM [Auth].[OrganizationUser]
		WHERE [OrganizationId] = @organizationId;

	-- Update the current user's role to manager
	-- ASSUMPTION: 2 is the productRoleId of "Manager" for all subscriptions
	EXEC [Billing].[UpdateSubscriptionUserProductRole] 2, @subscriptionId, @userId

	SELECT @subscriptionId;
END
