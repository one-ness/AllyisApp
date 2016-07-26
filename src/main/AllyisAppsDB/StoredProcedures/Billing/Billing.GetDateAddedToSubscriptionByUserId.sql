CREATE PROCEDURE [Billing].[GetDateAddedToSubscriptionByUserId]
	@UserId INT,
	@SubscriptionId INT
AS
	SET NOCOUNT ON;
	SELECT [CreatedUTC]
	FROM [Billing].[SubscriptionUser] WITH (NOLOCK)  
	WHERE [SubscriptionUser].[UserId] = @UserId 
	AND [SubscriptionUser].[SubscriptionId] = @SubscriptionId