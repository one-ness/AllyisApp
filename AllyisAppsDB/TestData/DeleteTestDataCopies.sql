DELETE FROM [TimeTracker].[Setting] WHERE [OrganizationId] IN (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Name] like '%travis');
DELETE FROM [Pjm].[ProjectUser] WHERE [ProjectId] IN (SELECT [ProjectId] FROM [Pjm].[Project] WHERE [CustomerId] IN (SELECT [CustomerId] FROM [Crm].[Customer] WHERE [OrganizationId] IN (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Name] like '%travis')));
DELETE FROM [Pjm].[Project] WHERE [CustomerId] IN (SELECT [CustomerId] FROM [Crm].[Customer] WHERE [OrganizationId] IN (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Name] like '%travis'));
DELETE FROM [Crm].[Customer] WHERE [OrganizationId] IN (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Name] like '%travis');
DELETE FROM [Billing].[SubscriptionUser] WHERE [SubscriptionId] IN (SELECT [SubscriptionId] FROM [Billing].[Subscription] WHERE [OrganizationId] IN (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Name] like '%travis'));
DELETE FROM [Billing].[Subscription] WHERE [OrganizationId] IN (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Name] like '%travis');
DELETE FROM [Auth].[OrganizationUser] WHERE [OrganizationId] IN (SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Name] like '%travis');
DELETE FROM [Auth].[Organization] WHERE [Name] like '%travis';
DELETE FROM [Auth].[User] WHERE [Email] LIKE '%.Travis.com';
