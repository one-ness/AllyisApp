CREATE TABLE #Vars
(
	[VarName] NVARCHAR(128),
	[VarValue] NVARCHAR(128)
)

INSERT INTO #Vars VALUES('EmailSuffix', @EmailSuffix);
INSERT INTO #Vars VALUES('UserIdSuffix', @UserIdSuffix);
INSERT INTO #Vars VALUES('SubdomainSuffix', @SubdomainSuffix);


PRINT 'Duplicate Organization';

SELECT * INTO #Tmp FROM [Auth].[Organization] WHERE [Subdomain] = 'NotAllyis';

UPDATE #Tmp SET Subdomain = Subdomain + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'SubdomainSuffix'), Name = Name + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'SubdomainSuffix');

INSERT INTO [Auth].[Organization] ([Name], [SiteUrl], [Address], [City], [State], [Country], [PostalCode], [PhoneNumber], [Subdomain])
SELECT [Name], [SiteUrl], [Address], [City], [State], [Country], [PostalCode], [PhoneNumber], [Subdomain] FROM #Tmp;

DROP TABLE #Tmp;

GO

PRINT 'Duplicate OrganizationUser';

SELECT * INTO #Tmp FROM [Auth].[OrganizationUser] WHERE [OrganizationUser].[OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE Subdomain = 'NotAllyis');

UPDATE #Tmp SET [OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Subdomain] = 'NotAllyis' + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'SubdomainSuffix')), [UserId] = [UserId] + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'UserIdSuffix');

INSERT INTO [Auth].[OrganizationUser] SELECT * FROM #Tmp;

DROP TABLE #Tmp;

GO

PRINT 'Duplicate Subscription';

SELECT * INTO #Tmp FROM [Billing].[Subscription] WHERE [Subscription].[OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE Subdomain = 'NotAllyis');

UPDATE #Tmp SET [OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Subdomain] = 'NotAllyis' + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'SubdomainSuffix'));

INSERT INTO [Billing].[Subscription] ([OrganizationId], [SkuId], [NumberOfUsers], [IsActive]) SELECT [OrganizationId], [SkuId], [NumberOfUsers], [IsActive] FROM #Tmp;

DROP TABLE #Tmp;

GO

PRINT 'Duplicate SubscriptionUser';

SELECT * INTO #Tmp FROM [Billing].[SubscriptionUser] WHERE [SubscriptionUser].[SubscriptionId] = (SELECT [SubscriptionId] FROM [Billing].[Subscription] WHERE [Subscription].[OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Organization].[Subdomain] = 'NotAllyis') AND [Billing].[Subscription].[SkuId] = (SELECT [SkuId] FROM [Billing].[Sku] WHERE [Name] = 'TimeTracker - Basic'));

UPDATE #Tmp SET [SubscriptionId] = (SELECT [SubscriptionId] FROM [Billing].[Subscription] WHERE [Subscription].[OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Organization].[Subdomain] = 'NotAllyis' + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'SubdomainSuffix')) AND [Subscription].[SkuId] = (SELECT [SkuId] FROM [Billing].[Sku] WHERE [Name] = 'TimeTracker - Basic')), [UserId] = [UserId] + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'UserIdSuffix');

INSERT INTO [Billing].[SubscriptionUser] SELECT * FROM #Tmp;

DROP TABLE #Tmp;

GO

PRINT 'Duplicate Customer';
SELECT * INTO #Tmp FROM [Crm].[Customer] WHERE [Customer].[OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Organization].[Subdomain] = 'NotAllyis');
UPDATE #Tmp SET [OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Organization].[Subdomain] = 'NotAllyis'  + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'SubdomainSuffix')),
[Name] = [Name] + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'SubdomainSuffix');
INSERT INTO [Crm].[Customer] ([Name], [ContactEmail], [Address], [City], [State], [Country], [PostalCode], [ContactPhoneNumber], [FaxNumber], [Website], [EIN], [OrganizationId], [IsActive])
SELECT [Name], [ContactEmail], [Address], [City], [State], [Country], [PostalCode], [ContactPhoneNumber], [FaxNumber], [Website], [EIN], [OrganizationId], [IsActive] from #Tmp;
DROP TABLE #Tmp;
PRINT 'Go';
GO
PRINT 'Test';
PRINT 'Duplicate Project';
PRINT 'Select';
SELECT * INTO #Tmp FROM [Pjm].[Project] WHERE [Project].[CustomerId] IN (SELECT [CustomerId] FROM [Crm].[Customer] WHERE [OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Organization].[Subdomain] = 'NotAllyis'));
PRINT 'Select 2';
SELECT [C1].[CustomerId] AS [C1Id], [C2].[CustomerId] AS [C2Id] INTO #CustomerCompares
  FROM [Crm].[Customer] AS [C1]
  INNER JOIN [Crm].[Customer] AS [C2] ON [C1].[Name] + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'SubdomainSuffix') = [C2].[Name] AND 	COALESCE([C1].[ContactEmail],'') = COALESCE([C2].[ContactEmail],'') AND 	COALESCE([C1].[Address],'') = COALESCE([C2].[Address],'') AND 	COALESCE([C1].[City],'') = COALESCE([C2].[City],'') AND 	COALESCE([C1].[State],'') = COALESCE([C2].[State],'') AND 	COALESCE([C1].[Country],'') = COALESCE([C2].[Country],'') AND 	COALESCE([C1].[PostalCode],'') = COALESCE([C2].[PostalCode],'') AND 	COALESCE([C1].[ContactPhoneNumber],'') = COALESCE([C2].[ContactPhoneNumber],'') AND 	COALESCE([C1].[FaxNumber],'') = COALESCE([C2].[FaxNumber],'') AND 	COALESCE([C1].[Website],'') = COALESCE([C2].[Website],'') AND 	COALESCE([C1].[EIN],'') = COALESCE([C2].[EIN],'') AND 	COALESCE([C1].[IsActive],'') = COALESCE([C2].[IsActive],'')
WHERE [C1].[OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Organization].[Subdomain] = 'NotAllyis') AND [C2].[OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Organization].[Subdomain] = 'NotAllyis'  + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'SubdomainSuffix'));
PRINT 'Update'
UPDATE #Tmp SET [CustomerId] = (SELECT [C2Id] FROM #CustomerCompares WHERE [C1Id] = [CustomerId]), [Name] = [Name] + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'SubdomainSuffix');
PRINT 'Insert'
INSERT INTO [Pjm].[Project] ([CustomerId], [Name], [IsActive])
SELECT [CustomerId], [Name], [IsActive] FROM #Tmp;

DROP TABLE #Tmp;

GO

PRINT 'Duplicate ProjectUser';

SELECT * INTO #Tmp FROM [Pjm].[ProjectUser] WHERE [ProjectUser].[ProjectId] IN (SELECT [ProjectId] FROM [Pjm].[Project] WHERE [CustomerId] IN (SELECT [CustomerId] FROM [Crm].[Customer] WHERE [OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Organization].[Subdomain] = 'NotAllyis')))

UPDATE #Tmp SET [UserId] = [UserId] + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'UserIdSuffix'),
[ProjectId] = (SELECT [P1].[ProjectId] FROM [Pjm].[Project] AS [P1] INNER JOIN [Pjm].[Project] AS [P2] ON [P1].[Name] = [P2].Name + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'SubdomainSuffix') WHERE [P2].[ProjectId] = [#Tmp].[ProjectId]);

--SELECT * FROM #Tmp WHERE [UserId] NOT IN (SELECT [UserId] FROM [Auth].[User]);

INSERT INTO [Pjm].[ProjectUser] SELECT * FROM #Tmp;

DROP TABLE #Tmp;


GO

PRINT 'Duplicate Settings';

SELECT * INTO #Tmp FROM [TimeTracker].[Setting] WHERE [Setting].[OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Organization].[Subdomain] = 'NotAllyis');

UPDATE #Tmp SET [OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Organization].[Subdomain] = 'NotAllyis'  + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'SubdomainSuffix'));

INSERT INTO [TimeTracker].[Setting] SELECT * FROM #Tmp;

DROP TABLE #Tmp;

GO

PRINT 'Duplicate TimeEntry';

SELECT * INTO #Tmp FROM [TimeTracker].[TimeEntry] WHERE [UserId] IN (SELECT [UserId] FROM [Auth].[OrganizationUser] WHERE [OrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Organization].[Subdomain] = 'NotAllyis'));

SELECT [P1].[ProjectId] AS [P1Id], [P2].[ProjectId] AS [P2Id] INTO #ProjectCompares FROM [Pjm].[Project] AS [P1] INNER JOIN [Pjm].[Project] AS [P2] ON COALESCE([P1].[IsActive], '') = COALESCE([P2].[IsActive], '') AND [P1].[Name] + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'SubdomainSuffix') = [P2].[Name];

UPDATE #Tmp SET [UserId] = [UserId] + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'UserIdSuffix');

UPDATE #Tmp set [ProjectId] = (SELECT [P2Id] FROM #ProjectCompares WHERE [P1Id] = [#Tmp].[ProjectId]);

INSERT INTO [TimeTracker].[TimeEntry] ([UserId], [ProjectId], [Date], [Duration], [Description], [PayClassId]) SELECT [UserId], [ProjectId], [Date], [Duration], [Description], [PayClassId] FROM #Tmp;


DROP TABLE #Tmp;
DROP TABLE #CustomerCompares;
DROP TABLE #ProjectCompares;

GO

PRINT 'Update Users';

UPDATE [Auth].[User] SET [LastUsedOrganizationId] = (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Name] = (SELECT [Name] FROM [Auth].[Organization] WHERE [OrganizationId] = [User].[LastUsedOrganizationId]) + (SELECT TOP 1 VarValue FROM #Vars WHERE [VarName] = 'SubdomainSuffix'))
UPDATE [Auth].[User] SET [LastUsedSubscriptionId] = (SELECT [SubscriptionId] FROM [Billing].[Subscription] WHERE [SkuId] = (SELECT [SkuId] FROM [Billing].[Subscription] WHERE [SubscriptionId] = [LastUsedSubscriptionId]) AND [OrganizationId] = [LastUsedOrganizationId]);