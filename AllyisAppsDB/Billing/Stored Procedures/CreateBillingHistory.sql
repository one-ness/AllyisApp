CREATE PROCEDURE [Billing].[CreateBillingHistory]
	@description NVARCHAR(MAX),
	@organizationId INT,
	@userId INT,
	@skuId INT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [Billing].[BillingHistory]
		([Date],
		[Description],
		[OrganizationId],
		[UserId],
		[SkuId])
	VALUES (
		SYSDATETIME(),
		@description,
		@organizationId,
		@userId,
		@skuId);
END
