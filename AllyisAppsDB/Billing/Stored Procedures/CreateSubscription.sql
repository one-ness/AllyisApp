CREATE PROCEDURE [Billing].[CreateSubscription]
	@organizationId INT,
	@productId INT,
	@subscriptionName NVARCHAR(50),
	@userId INT,
	@managerProductRoleId INT,
	@unassignedProductRoleId INT
AS
BEGIN
	SET NOCOUNT ON;
    
	--Create the new subscription
	INSERT INTO [Billing].[Subscription]
		([OrganizationId],
		[ProductId],
		[SubscriptionName])
	VALUES
		(@organizationId,
		@productId,
		@subscriptionName);

	DECLARE @subscriptionId INT = SCOPE_IDENTITY();
	
	-- Add all org users to the subscription.  Current user is manager, other users are unassigned
	/*WITH #organizationUsers AS (
		SELECT  [UserId],
				[SubscriptionId] = @subscriptionId,
				[ProductRoleId] = @unassignedProductRoleId
		FROM [Auth].[OrganizationUser]
		WHERE [OrganizationId] = @organizationId
		AND [UserId] != @userId
		UNION ALL
		SELECT  [UserId] = @userId,
				[SubscriptionId] = @subscriptionId,
				[ProductRoleId] = @managerProductRoleId
	)*/
	SELECT @subscriptionId
END
GO