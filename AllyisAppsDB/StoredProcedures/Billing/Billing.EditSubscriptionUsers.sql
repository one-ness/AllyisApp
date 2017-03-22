CREATE PROCEDURE [dbo].[EditSubscriptionUsers]
	@OrganizationId	INT,
	@UserIDs [Auth].[UserTable] READONLY,
	@TimeTrackerRole INT
AS
BEGIN TRANSACTION
	DECLARE @SubId INT;
	DECLARE @NumberOfUsers INT;
	SELECT
		@SubId = [SubscriptionId],
		@NumberOfUsers = [NumberOfUsers]
	FROM [Billing].[Subscription] WITH (NOLOCK)
	JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	JOIN [Billing].[Product] WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
	WHERE [Subscription].[OrganizationId] = @OrganizationId AND [Product].[Name] = 'TimeTracker' AND [Subscription].[IsActive] = 1

	IF @SubId IS NOT NULL
	BEGIN
		IF @TimeTrackerRole = -1 -- Removing users from subscription
		BEGIN

		END
		IF @TimeTrackerRole > 0 -- Adding/updating subscription users
		BEGIN
		-- TODO: Check for users being added vs. users just changing roles
			DECLARE @ExistingUsers INT

			SELECT @ExistingUsers = COUNT(*)
			FROM [Billing].[SubscriptionUser] WITH (NOLOCK)
			WHERE [SubscriptionId] = @SubId
		END
	END
	ELSE
	BEGIN	-- No subscription to time tracker
		SELECT -1
	END
COMMIT