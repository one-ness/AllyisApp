
SET IDENTITY_INSERT [Auth].[Organization] ON

INSERT INTO [Auth].[Organization] (OrganizationId, Name, Subdomain)
VALUES (0, 'Default', 'default')

SET IDENTITY_INSERT [Auth].[Organization] OFF

:r .\ReferenceData\CountryStates.sql
:r .\ReferenceData\Products.sql
:r .\ReferenceData\Sku.sql
:r .\ReferenceData\Role.sql
:r .\ReferenceData\PayClasses.sql
:r .\ReferenceData\Holidays.sql
:r .\ReferenceData\Languages.sql
--:r .\TestData\TestData.sql
--:r .\TestData\DuplicateTestData.sql

DBCC CHECKIDENT('TimeTracker.TimeEntry');
DBCC CHECKIDENT('Auth.Invitation');
DBCC CHECKIDENT('TimeTracker.Holiday');
DBCC CHECKIDENT('TimeTracker.PayClass');
DBCC CHECKIDENT('Crm.Project');
DBCC CHECKIDENT('Crm.Customer');
DBCC CHECKIDENT('Billing.Subscription');
DBCC CHECKIDENT('Lookup.State');
DBCC CHECKIDENT('Lookup.Country');

:r .\Logins\aaUser.sql	--Create Login for aaUser
:r .\Users\aaUser.sql   --Add aaUser Login ability to access database