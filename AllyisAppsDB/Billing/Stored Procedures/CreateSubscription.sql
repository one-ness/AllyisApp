CREATE PROCEDURE [Billing].[CreateSubscription]
	@organizationId INT,
	@skuId INT,
	@subscriptionName NVARCHAR(50),
	@userId INT,
	@productRoleId int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @subscriptionId INT

	--Create the new subscription
	INSERT INTO [Billing].[Subscription]
		([OrganizationId],
		[SkuId],
		[SubscriptionName])
	VALUES
		(@organizationId,
		@skuId,
		@subscriptionName);

	select @subscriptionId = SCOPE_IDENTITY()
	
	-- create a new subscription user
	INSERT INTO [Billing].[SubscriptionUser] ([UserId], [SubscriptionId], [ProductRoleId])
		values(@userId, @subscriptionId, @productRoleId)

	select @subscriptionId

END
