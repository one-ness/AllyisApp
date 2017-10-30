CREATE PROCEDURE [Billing].[GetSubscriptions]
    @orgId INT
AS
BEGIN
    SET NOCOUNT ON
    SELECT [s].*,
           [sk].[SkuName],
		   [sk].[IconUrl],
           [p].[ProductId],
           [p].[ProductName],
           [p].[AreaUrl],
           [p].[Description] AS [ProductDescription]
      FROM [Subscription] [s] WITH (NOLOCK)
      JOIN [Sku]         [sk] WITH (NOLOCK) ON [sk].[SkuId] = [s].[SkuId]
      JOIN [Product]      [p] WITH (NOLOCK) ON [p].[ProductId] = [sk].[ProductId]
     WHERE [s].[OrganizationId] = @orgId
       AND [s].[IsActive] = 1
       AND [sk].[IsActive] = 1
       AND [p].[IsActive] = 1
END