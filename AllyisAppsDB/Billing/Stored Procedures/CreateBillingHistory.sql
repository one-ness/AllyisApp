CREATE PROCEDURE [Billing].[CreateBillingHistory]
	@Description NVARCHAR(MAX), 
	@OrganizationId INT, 
	@UserId INT, 
	@SkuId INT 
AS
	SET NOCOUNT ON;
	INSERT INTO [Billing].[BillingHistory] ([Date], [Description], [OrganizationId], [UserId], [SkuId])
	VALUES (SYSDATETIME(), @Description, @OrganizationId, @UserId, @SkuId)