CREATE PROCEDURE [Auth].[GetActiveProductRoleForUser]
	@productName VARCHAR (32),
	@organizationId INT,
	@userId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [ProductRoleName]												
	FROM [Auth].[ProductRole]
	WITH (NOLOCK)								
	WHERE [ProductRoleId] =										
		(SELECT [ProductRoleId]									
		FROM [Billing].[SubscriptionUser]
		WITH (NOLOCK)						
		WHERE [UserId] = @userId								
		AND [SubscriptionId] =									
			(SELECT [SubscriptionId]							
			FROM [Billing].[Subscription]
			WITH (NOLOCK)						
			WHERE [OrganizationId] = @organizationId			
				AND [IsActive] = 1								
				AND [SkuId] IN									
					(SELECT [SkuId]								
					FROM [Billing].[Sku]	
					WITH (NOLOCK)					
					WHERE [ProductId] IN						
						(SELECT [ProductId]						
						FROM [Billing].[Product]
						WITH (NOLOCK)				
						WHERE [Product].[ProductName] = @productName))));
END