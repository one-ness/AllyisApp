CREATE PROCEDURE [TimeTracker].[CreateBulkTimeEntry]
	@Date DATETIME2(0),
	@Duration FLOAT,
	@Description NVARCHAR(120),
	@PayClassId INT,
	@OrganizationId INT,
	@Overwrite BIT = 0
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TTProductId INT = (SELECT [ProductId] FROM [Billing].[Product] WHERE [Product].[Name] = 'TimeTracker');
	SELECT [SkuId] INTO #SKUIDs FROM [Billing].[Sku] WHERE [ProductId] = @TTProductId;

	IF (@OrganizationId = 0) --Every time tracker user
		BEGIN
			SELECT DISTINCT [UserId], 
				@Date AS 'Date', 
				@Duration AS 'Duration', 
				@Description AS 'Description', 
				1 AS 'IsActive', 
				@PayClassId AS 'PayClassId', 
				NULL AS 'FirstProject' 
			INTO #Tmp 
			FROM [Billing].[SubscriptionUser] WITH (NOLOCK)  
			WHERE [SubscriptionId] IN 
				(SELECT [SubscriptionId] 
				FROM [Billing].[Subscription] WITH (NOLOCK)  
				WHERE [Subscription].[SkuId] IN (SELECT [SkuId] FROM #SKUIDs))
			
			UPDATE #Tmp SET [FirstProject] = 
				(SELECT TOP 1 [ProjectId] 
				FROM [Crm].[ProjectUser] 
				WITH (NOLOCK) 
				WHERE [UserId] = [#Tmp].[UserId] 
					AND [IsActive] = 1);

			IF(@Overwrite = 1)
				BEGIN
					UPDATE [TimeTracker].[TimeEntry] 
					SET [Duration] = @Duration, 
						[Description] = @Description, 
						[PayClassId] = @PayClassId 
					WHERE [Date] = @Date 
						AND [UserId] IN (SELECT [UserId] FROM #Tmp);
					DELETE FROM #Tmp 
					WHERE [UserId] IN 
						(SELECT [UserId] FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) WHERE [Date] = @Date);
				END
			ELSE
				BEGIN
					DELETE FROM #Tmp 
					WHERE [UserId] IN 
						(SELECT [UserId] FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) WHERE [Date] = @Date);
				END
				INSERT INTO [TimeTracker].[TimeEntry] 
					([UserId], 
					[Date], 
					[Duration], 
					[Description],
					[PayClassId], 
					[ProjectId]) 
				SELECT [UserId], 
					[Date], 
					[Duration], 
					[Description],
					[PayClassId], 
					[FirstProject] AS 'ProjectId' 
				FROM #Tmp;
				DROP TABLE #Tmp;
		END
	ELSE
		BEGIN --Specific organization's users
			DECLARE @SubscriptionID INT = 
				(SELECT TOP 1 [SubscriptionId] 
				FROM [Billing].[Subscription] WITH (NOLOCK) 
				WHERE [OrganizationId] = @OrganizationId 
					AND [SkuId] IN (SELECT [SkuId] FROM #SKUIDs));
			SELECT [UserId], 
				@Date AS [Date],
				 @Duration AS [Duration], 
				 @Description AS [Description], 
				 1 AS [IsActive], 
				 @PayClassId AS [PayClassId], 
				 NULL AS 'FirstProject' 
			INTO #OrgTmp 
			FROM [Billing].[SubscriptionUser] WITH (NOLOCK) WHERE [SubscriptionId] = @SubscriptionID;

			UPDATE #OrgTmp SET [FirstProject] = 
				(SELECT TOP 1 [ProjectId] 
				FROM [Crm].[ProjectUser] WITH (NOLOCK) 
				WHERE [UserId] = [#OrgTmp].[UserId] AND [IsActive] = 1 
					AND [ProjectId] IN 
						(SELECT [ProjectId] 
						FROM [Crm].[Project] WITH (NOLOCK) 
						JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId] WHERE [Customer].[OrganizationId] = @OrganizationId));
			--^^^ Sets the column that contains the first project id for each user for the specified org
			IF (@Overwrite = 1)
				BEGIN
					UPDATE [TimeTracker].[TimeEntry] SET [Duration] = @Duration, [Description] = @Description, [PayClassId] = @PayClassId WHERE [Date] = @Date AND [UserId] IN (SELECT [UserId] FROM #OrgTmp);
					DELETE FROM #OrgTmp WHERE [UserId] IN (SELECT [UserId] FROM [TimeTracker].[TimeEntry] WHERE [Date] = @Date);
				END
			ELSE
				BEGIN
					DELETE FROM #OrgTmp WHERE [UserId] IN (SELECT [UserId] FROM [TimeTracker].[TimeEntry] WHERE [Date] = @Date);
				END
				INSERT INTO [TimeTracker].[TimeEntry] ([UserId], [Date], [Duration], [Description], [PayClassId], [ProjectId])SELECT [UserId], [Date], [Duration], [Description], [PayClassId], [FirstProject] AS 'ProjectId' FROM #OrgTmp;
				DROP TABLE #OrgTmp;
		END
		DROP TABLE #SKUIDs;
END

