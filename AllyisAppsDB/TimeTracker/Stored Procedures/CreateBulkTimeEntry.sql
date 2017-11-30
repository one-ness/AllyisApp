CREATE PROCEDURE [TimeTracker].[CreateBulkTimeEntry]
	@date DATETIME2(0),
	@duration FLOAT,
	@description NVARCHAR(120),
	@payClassId INT,
	@organizationId INT,
	@overwrite BIT = 0
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @tTProductId INT = (SELECT [ProductId] FROM [Billing].[Product] WHERE [Product].[ProductName] = 'TimeTracker');
	SELECT [SkuId] INTO #SKUIDs FROM [Billing].[Sku] WHERE [ProductId] = @tTProductId;

	DECLARE @subscriptionId INT = 
		(SELECT TOP 1 [SubscriptionId] 
		FROM [Billing].[Subscription] WITH (NOLOCK) 
		WHERE [OrganizationId] = @organizationId 
			AND [SkuId] IN (SELECT [SkuId] FROM #SKUIDs));
	SELECT [UserId], 
		@date AS [Date],
			@duration AS [Duration], 
			@description AS [Description], 
			1 AS [IsActive], 
			@payClassId AS [PayClassId], 
			NULL AS 'FirstProject' 
	INTO #OrgTmp 
	FROM [Billing].[SubscriptionUser] WITH (NOLOCK) WHERE [SubscriptionId] = @subscriptionId;

	Declare @firstProject as int
	set @firstProject = (SELECT TOP 1 [ProjectId] 
		FROM [Pjm].[ProjectUser], #OrgTmp WITH (NOLOCK) 
		WHERE [ProjectUser].[UserId] = [#OrgTmp].[UserId] AND [ProjectUser].[IsActive] = 1 
			AND [ProjectId] IN 
				(SELECT [ProjectId] 
				FROM [Pjm].[Project] WITH (NOLOCK) 
				JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId] WHERE [Customer].[OrganizationId] = @organizationId));
	--^^^ Sets the column that contains the first project id for each user for the specified org

	if (@firstProject is null)
	begin;
		throw 50001,'The organizaion has no projects no body to take holiday time from',1;
	end

	UPDATE #OrgTmp SET [FirstProject] = @firstProject
				
	IF (@overwrite = 1)
	BEGIN
		UPDATE [TimeTracker].[TimeEntry]
		SET	[Duration] = @duration,
			[Description] = @description,
			[PayClassId] = @payClassId
		WHERE [Date] = @date
		AND [UserId] IN (SELECT [UserId] FROM #OrgTmp)
	END

	

	INSERT INTO [TimeTracker].[TimeEntry] ([UserId], [Date], [Duration], [Description], [PayClassId], [ProjectId])SELECT [UserId], [Date], [Duration], [Description], [PayClassId], [FirstProject] AS 'ProjectId' FROM #OrgTmp;
	DROP TABLE #OrgTmp;
	DROP TABLE #SKUIDs;
END