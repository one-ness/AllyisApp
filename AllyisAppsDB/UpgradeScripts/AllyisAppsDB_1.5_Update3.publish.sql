

GO
PRINT N'Dropping [Auth].[DF_OrganizationUser_OrganizationUserCreatedUtc]...';


GO
ALTER TABLE [Auth].[OrganizationUser] DROP CONSTRAINT [DF_OrganizationUser_OrganizationUserCreatedUtc];


GO
PRINT N'Altering [aaUser]...';


GO
ALTER USER [aaUser]
    WITH LOGIN = [aaUser];


GO
PRINT N'Creating [Auth].[DF_OrganizationUser_CreatedUtc]...';


GO
ALTER TABLE [Auth].[OrganizationUser]
    ADD CONSTRAINT [DF_OrganizationUser_CreatedUtc] DEFAULT (getutcdate()) FOR [OrganizationUserCreatedUtc];


GO
PRINT N'Altering [Auth].[CreateInvitation]...';


GO
ALTER PROCEDURE [Auth].[CreateInvitation]
	@email NVARCHAR(384),
	@firstName NVARCHAR(40),
	@lastName NVARCHAR(40),
	@organizationId INT,
	@organizationRole INT,
	@employeeId NVARCHAR(16)
AS

BEGIN
	SET NOCOUNT ON;
	IF EXISTS (
		SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
		INNER JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
		WHERE [Email] = @email AND [OrganizationId] = @organizationId
	)
	BEGIN
		SELECT -1 --Indicates the user is already in the organization
	END
	ELSE
	BEGIN
		-- Check for existing employee id
		IF EXISTS (
			SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
			WHERE [OrganizationId] = @organizationId AND [EmployeeId] = @employeeId
		) OR EXISTS (
			SELECT * FROM [Auth].[Invitation] WITH (NOLOCK)
			WHERE [OrganizationId] = @organizationId AND [IsActive] = 1 AND [EmployeeId] = @employeeId
		)
		BEGIN
			SELECT -2 -- Indicates employee id already taken
		END
		ELSE
		BEGIN
			INSERT INTO [Auth].[Invitation] 
				([Email], 
				[FirstName], 
				[LastName], 
				[OrganizationId],  
				[IsActive], 
				[OrganizationRoleId],
				[EmployeeId]
				)
			VALUES 
				(@email, 
				@firstName, 
				@lastName, 
				@organizationId,  
				1, 
				@organizationRole, 
				@employeeId
				);

			-- Return invitation id
			SELECT SCOPE_IDENTITY()
		END
	END
END
GO
PRINT N'Altering [Auth].[UpdateEmailConfirmed]...';


GO
ALTER PROCEDURE [Auth].[UpdateEmailConfirmed]
	@emailConfirmCode uniqueidentifier
AS
BEGIN
	set nocount on
	UPDATE [Auth].[User]
	SET [IsEmailConfirmed] = 1
	WHERE [EmailConfirmationCode] = @emailConfirmCode
	select @@ROWCOUNT
END
GO
PRINT N'Altering [Billing].[GetAllActiveProductsAndSkus]...';


GO
ALTER PROCEDURE [Billing].[GetAllActiveProductsAndSkus]
AS
	SET NOCOUNT ON;
	SELECT
		[Product].[ProductId],
		[Product].[ProductName],
		[Product].[Description],
		[Product].[AreaUrl]
	FROM [Billing].[Product] WITH (NOLOCK) 
	WHERE [IsActive] = 1
	ORDER BY [Product].[ProductId]

	SELECT
		[Product].[ProductId],
		[Sku].[SkuId],
		[Sku].[SkuName],
		[Sku].[CostPerBlock] AS 'Price',
		[Sku].[UserLimit],
		[Sku].[BillingFrequency],
		[Sku].[BlockBasedOn],
		[Sku].[BlockSize],
		[Sku].[Description],
		[Sku].[PromoCostPerBlock],
		[Sku].[PromoDeadline],
		[Sku].[IconUrl]
	FROM [Billing].[Product] 
	LEFT JOIN [Billing].[Sku]
	WITH (NOLOCK) 
	ON [Product].[ProductId] = [Sku].[ProductId]
	WHERE ([Product].[IsActive] = 1 AND [Sku].[IsActive] = 1)
	ORDER BY [Product].[ProductId]
GO
PRINT N'Altering [Billing].[GetProductSubscriptionInfo]...';


GO
ALTER PROCEDURE [Billing].[GetProductSubscriptionInfo]
	@skuId INT,
	@orgId INT
AS
	SET NOCOUNT ON;
	DECLARE @productId INT;
	DECLARE @subscriptionId INT;

SELECT @productId = [Product].[ProductId]
FROM [Billing].[Product] 
	  LEFT JOIN [Billing].[Sku] WITH (NOLOCK) 
	  ON [Product].ProductId = [Sku].ProductId	  
	  WHERE [Sku].SkuId = @skuId

SELECT 
	[Product].[ProductName], 
	[Product].[ProductId], 
	[Product].[Description], 
	[Product].[AreaUrl]
	FROM [Billing].[Product]   
	WHERE [Product].ProductId = @productId

	SELECT
		@subscriptionId = [SubscriptionId]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	WHERE [OrganizationId] = @orgId AND [Subscription].[SkuId] = @skuId AND [Subscription].[IsActive] = 1

	SELECT
		[SubscriptionId],
		[SkuId],
		[NumberOfUsers],
		[SubscriptionCreatedUtc],
		[OrganizationId]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	WHERE [SubscriptionId] = @subscriptionId

	SELECT [SkuId],
		[ProductId],
		[SkuName],
		[CostPerBlock],
		[UserLimit],
		[BillingFrequency],
		[BlockBasedOn],
		[BlockSize],
		[PromoCostPerBlock],
		[PromoDeadline],
		[IsActive],
		[Description],
		[IconUrl]
	FROM [Billing].[Sku] WITH (NOLOCK) 
	WHERE [Billing].[Sku].[ProductId] = @productId

	SELECT [StripeTokenCustId]
	FROM [Billing].[StripeOrganizationCustomer] WITH (NOLOCK) 
	WHERE [OrganizationId] = @orgId AND [IsActive] = 1

	SELECT COUNT([UserId])
	FROM (
		SELECT [SubscriptionUser].[UserId]
		FROM [Billing].[SubscriptionUser] WITH (NOLOCK)
		LEFT JOIN [Billing].[Subscription] WITH (NOLOCK) ON [Subscription].[SubscriptionId] = [SubscriptionUser].[SubscriptionId]
		WHERE 
			[Subscription].[SubscriptionId] = @subscriptionId
	) src
GO
PRINT N'Altering [Billing].[GetSkuById]...';


GO
ALTER PROCEDURE [Billing].[GetSkuById]
	@skuId INT
AS
	SET NOCOUNT ON;
	SELECT [SkuId]
      ,[ProductId]
      ,[SkuName]
      ,[CostPerBlock]
      ,[UserLimit]
      ,[BillingFrequency]
	  ,[BlockBasedOn]
      ,[BlockSize]
	  ,[Description]
      ,[PromoCostPerBlock]
      ,[PromoDeadline]
      ,[IsActive]
	  ,[IconUrl]
       FROM [Billing].[Sku] WITH (NOLOCK) WHERE [SkuId] = @skuId
GO
PRINT N'Altering [Billing].[UpdateSubscription]...';


GO
-- TODO: pass in subscriptionId as a parameter to simplify logic

ALTER PROCEDURE [Billing].[UpdateSubscription]
	@organizationId INT,
	@skuId INT,
	@subscriptionId INT,
	@subscriptionName NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	--Find existing subscription that has the same ProductId but different SkuId, update it to the new SkuId
	--Because the productRoleId and subscriptionId don't change, no need to update SubscriptionUser table
	UPDATE [Billing].[Subscription] SET [SkuId] = @skuId, [SubscriptionName] = @subscriptionName
		WHERE [OrganizationId] = @organizationId
		AND [Subscription].[IsActive] = 1
		AND [Subscription].SubscriptionId = @subscriptionId;
END
GO
PRINT N'Update complete.';


GO
