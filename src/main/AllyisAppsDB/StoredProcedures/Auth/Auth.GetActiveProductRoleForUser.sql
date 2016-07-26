CREATE PROCEDURE [Auth].[GetActiveProductRoleForUser]
	@ProductName VARCHAR (32),
	@OrganizationId INT,
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Name]												
	FROM [Auth].[ProductRole]									
	WHERE [ProductRoleId] =										
		(SELECT [ProductRoleId]									
		FROM [Billing].[SubscriptionUser]
		WITH (NOLOCK)						
		WHERE [UserId] = @UserId								
		AND [SubscriptionId] =									
			(SELECT [SubscriptionId]							
			FROM [Billing].[Subscription]
			WITH (NOLOCK)						
			WHERE [OrganizationId] = @OrganizationId			
				AND [IsActive] = 1								
				AND [SkuId] IN									
					(SELECT [SkuId]								
					FROM [Billing].[Sku]	
					WITH (NOLOCK)					
					WHERE [ProductId] IN						
						(SELECT [ProductId]						
						FROM [Billing].[Product]
						WITH (NOLOCK)				
						WHERE [Product].[Name] = @ProductName))));
END