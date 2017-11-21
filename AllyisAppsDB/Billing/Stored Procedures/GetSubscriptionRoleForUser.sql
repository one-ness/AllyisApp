CREATE PROCEDURE [Billing].[GetSubscriptionRoleForUser]
	@subscriptionId int,
	@userId int
AS
	SELECT [ProductRoleId]
	FROM [Billing].[SubscriptionUser]
	WHERE [SubscriptionId] = @subscriptionId
	AND [UserId] = @userId

