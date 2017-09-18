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

	IF (@organizationId = 0) --Every time tracker user
		BEGIN
			SELECT DISTINCT [UserId], 
				@date AS 'Date', 
				@duration AS 'Duration', 
				@description AS 'Description', 
				1 AS 'IsActive', 
				@payClassId AS 'PayClassId', 
				NULL AS 'FirstProject' 
			INTO #Tmp 
			FROM [Billing].[SubscriptionUser] WITH (NOLOCK)  
			WHERE [SubscriptionId] IN 
				(SELECT [SubscriptionId] 
				FROM [Billing].[Subscription] WITH (NOLOCK)  
				WHERE [Subscription].[SkuId] IN (SELECT [SkuId] FROM #SKUIDs))
			
			Declare @firstProject as int
			set @firstProject = (SELECT TOP 1 [ProjectId] 
				FROM [Pjm].[ProjectUser], #Tmp
				WITH (NOLOCK) 
				WHERE [ProjectUser].[UserId] = [#Tmp].[UserId] AND [ProjectUser].[IsActive] = 1);

			if @firstProject is null begin set @firstProject = 0 end
			UPDATE #Tmp SET [FirstProject] = @firstProject

				

			IF(@overwrite = 1)
				BEGIN
					UPDATE [TimeTracker].[TimeEntry] 
					SET [Duration] = @duration, 
						[Description] = @description, 
						[PayClassId] = @payClassId 
					WHERE [Date] = @date 
						AND [UserId] IN (SELECT [UserId] FROM #Tmp);
					DELETE FROM #Tmp 
					WHERE [UserId] IN 
						(SELECT [UserId] FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) WHERE [Date] = @date);
				END
			ELSE
				BEGIN
					DELETE FROM #Tmp 
					WHERE [UserId] IN 
						(SELECT [UserId] FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) WHERE [Date] = @date);
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

			Declare @firstProejct as int
			set @firstProject = (SELECT TOP 1 [ProjectId] 
				FROM [Pjm].[ProjectUser], #OrgTmp WITH (NOLOCK) 
				WHERE [ProjectUser].[UserId] = [#OrgTmp].[UserId] AND [ProjectUser].[IsActive] = 1 
					AND [ProjectId] IN 
						(SELECT [ProjectId] 
						FROM [Pjm].[Project] WITH (NOLOCK) 
						JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId] WHERE [Customer].[OrganizationId] = @organizationId));
			--^^^ Sets the column that contains the first project id for each user for the specified org

			if (@firstProject is null) begin set @firstProject = 0 end
			UPDATE #OrgTmp SET [FirstProject] = @firstProject
				
			IF (@overwrite = 1)
				BEGIN
					UPDATE [TimeTracker].[TimeEntry] SET [Duration] = @duration, [Description] = @description, [PayClassId] = @payClassId WHERE [Date] = @date AND [UserId] IN (SELECT [UserId] FROM #OrgTmp);
					DELETE FROM #OrgTmp WHERE [UserId] IN (SELECT [UserId] FROM [TimeTracker].[TimeEntry] WHERE [Date] = @date);
				END
			ELSE
				BEGIN
					DELETE FROM #OrgTmp WHERE [UserId] IN (SELECT [UserId] FROM [TimeTracker].[TimeEntry] WHERE [Date] = @date);
				END
				INSERT INTO [TimeTracker].[TimeEntry] ([UserId], [Date], [Duration], [Description], [PayClassId], [ProjectId])SELECT [UserId], [Date], [Duration], [Description], [PayClassId], [FirstProject] AS 'ProjectId' FROM #OrgTmp;
				DROP TABLE #OrgTmp;
		END
		DROP TABLE #SKUIDs;
END