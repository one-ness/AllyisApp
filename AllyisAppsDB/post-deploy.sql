/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

--Insert Default Org
SET IDENTITY_INSERT [Auth].[Organization] ON
INSERT INTO [Auth].[Organization] (OrganizationId, Name, Subdomain)
VALUES (0, 'Default', 'default')
SET IDENTITY_INSERT [Auth].[Organization] OFF
GO

--Insert Default Customer
SET IDENTITY_INSERT [Crm].[Customer] ON
INSERT INTO [CRM].[Customer] ([CustomerId], [Name], [OrganizationId], [CustomerOrgId], [IsActive])
VALUES (0, 'Default', 0, 0, '0000000000000000')
SET IDENTITY_INSERT [Crm].[Customer] OFF
GO

--Insert Default Project
SET IDENTITY_INSERT [Crm].[Project] ON
INSERT INTO [CRM].[Project] ([ProjectId], [Name], [CustomerId], [ProjectOrgId], [IsActive])
VALUES (0, 'Default', 0, 0, '0000000000000000')
SET IDENTITY_INSERT [Crm].[Project] OFF
GO

:r .\DefaultData\CountryStates.sql
:r .\DefaultData\Products.sql
:r .\DefaultData\Sku.sql
:r .\DefaultData\Role.sql
:r .\DefaultData\PayClasses.sql
:r .\DefaultData\Holidays.sql
:r .\DefaultData\Languages.sql
--:r .\TestData\TestData.sql
--:r .\TestData\DuplicateTestData.sql

DBCC CHECKIdENT('TimeTracker.TimeEntry');
DBCC CHECKIdENT('Auth.Invitation');
DBCC CHECKIdENT('TimeTracker.Holiday');
DBCC CHECKIdENT('TimeTracker.PayClass');
DBCC CHECKIdENT('Crm.Project');
DBCC CHECKIdENT('Crm.Customer');
DBCC CHECKIdENT('Billing.Subscription');
DBCC CHECKIdENT('Lookup.State');
DBCC CHECKIdENT('Lookup.Country');
