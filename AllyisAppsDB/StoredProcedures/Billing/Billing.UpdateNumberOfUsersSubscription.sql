/*This query will change the Sku for an organization's subscription and will prevent multiple Skus of the same product*/
CREATE PROCEDURE [Billing].[UpdateNumberOfUsersSubscription]
	@OrganizationId INT,
	@SkuId INT,
	@NumberOfUsers INT
AS
	SET NOCOUNT ON;
	UPDATE [Billing].[Subscription] SET [NumberOfUsers] = @NumberOfUsers
	WHERE [OrganizationId] = @OrganizationId AND [Subscription].[IsActive] = 1 AND [SkuId] = @SkuId
