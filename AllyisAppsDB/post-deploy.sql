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
--SET IDENTITY_INSERT [Auth].[Organization] ON
--INSERT INTO [Auth].[Organization] (OrganizationId, [OrganizationName], Subdomain)
--VALUES (0, 'Default', 'default')
--SET IDENTITY_INSERT [Auth].[Organization] OFF
GO

:r .\DefaultData\CountryStates.sql
:r .\DefaultData\Products.sql
:r .\DefaultData\Sku.sql
:r .\DefaultData\Role.sql
:r .\DefaultData\Languages.sql
--:r .\TestData\TestData.sql
--:r .\TestData\DuplicateTestData.sql

DBCC CHECKIdENT('TimeTracker.TimeEntry');
DBCC CHECKIdENT('Auth.Invitation');
DBCC CHECKIdENT('Hrm.Holiday');
DBCC CHECKIdENT('Hrm.PayClass');
DBCC CHECKIdENT('Pjm.Project');
DBCC CHECKIdENT('Crm.Customer');
DBCC CHECKIdENT('Billing.Subscription');
DBCC CHECKIdENT('Lookup.State');
DBCC CHECKIdENT('Lookup.Country');
