CREATE PROCEDURE [Billing].[GetSubscriptionUserList]
AS
	SET NOCOUNT ON;
	SELECT [SubscriptionId],
			[UserId],
			[ProductRoleId],
			[CreatedUTC]
	FROM [Billing].[SubscriptionUser] WITH (NOLOCK) 