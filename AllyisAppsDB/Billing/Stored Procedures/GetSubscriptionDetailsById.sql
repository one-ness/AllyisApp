CREATE PROCEDURE [Billing].[GetSubscriptionDetailsById]
	@subscriptionId INT
AS
begin
	SET NOCOUNT ON;
	SELECT * FROM [Billing].[Subscription] WHERE @subscriptionId = [SubscriptionId]
	/*
	select s.*, p.ProductId, p.AreaUrl, p.[Description] as 'ProductDescription', p.ProductName FROM Subscription s WITH (NOLOCK)
	INNER JOIN Product p WITH (NOLOCK) ON p.ProductId = s.ProductId
	where s.SubscriptionId = @subscriptionId
	*/
end
