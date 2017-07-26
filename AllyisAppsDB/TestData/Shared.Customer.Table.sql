PRINT 'Customer'
SET IDENTITY_INSERT [Crm].[Customer] ON 

INSERT [Crm].[Customer] ([CustomerId], [Name], [ContactEmail], [Address], [City], [State], [Country], [PostalCode], [ContactPhoneNumber], [FaxNumber], [Website], [EIN], [OrganizationId], [IsActive]) VALUES (1, N'NotAllyis', N'contact@notallyis.com', N'10210 NE Points Dr.', N'Kirkland', (SELECT [StateId] FROM [Lookup].[State] WHERE [Name] = 'Washington'), (SELECT [CountryId] FROM [Lookup].[Country] WHERE [Name] = 'United States'), N'98033', N'1234567890', N'9876543210', N'www.notallyis.com', NULL, 1, 1)
INSERT [Crm].[Customer] ([CustomerId], [Name], [ContactEmail], [Address], [City], [State], [Country], [PostalCode], [ContactPhoneNumber], [FaxNumber], [Website], [EIN], [OrganizationId], [IsActive]) VALUES (10, N'4C Insights', N'contact@4cinsights.com', N'12345 Somewhere St.', N'Bellevue', (SELECT [StateId] FROM [Lookup].[State] WHERE [Name] = 'Washington'), (SELECT [CountryId] FROM [Lookup].[Country] WHERE [Name] = 'United States'), N'98007', N'1234567890', N'9876543210', N'www.4cinsights.com', N'', 1, 1)
INSERT [Crm].[Customer] ([CustomerId], [Name], [ContactEmail], [Address], [City], [State], [Country], [PostalCode], [ContactPhoneNumber], [FaxNumber], [Website], [EIN], [OrganizationId], [IsActive]) VALUES (11, N'Acumatica', N'contact@acumatica.com', N'12346 Somewhere St.', N'Bellevue', (SELECT [StateId] FROM [Lookup].[State] WHERE [Name] = 'Washington'), (SELECT [CountryId] FROM [Lookup].[Country] WHERE [Name] = 'United States'), N'98007', N'1234567890', N'9876543210', N'www.acumatica.com', N'', 1, 1)
INSERT [Crm].[Customer] ([CustomerId], [Name], [ContactEmail], [Address], [City], [State], [Country], [PostalCode], [ContactPhoneNumber], [FaxNumber], [Website], [EIN], [OrganizationId], [IsActive]) VALUES (12, N'Adobe', N'contact@adobe.com', N'12347 Somewhere St.', N'Bellevue', (SELECT [StateId] FROM [Lookup].[State] WHERE [Name] = 'Washington'), (SELECT [CountryId] FROM [Lookup].[Country] WHERE [Name] = 'United States'), N'98007', N'1234567890', N'9876543210', N'www.adobe.com', N'', 1, 1)
INSERT [Crm].[Customer] ([CustomerId], [Name], [ContactEmail], [Address], [City], [State], [Country], [PostalCode], [ContactPhoneNumber], [FaxNumber], [Website], [EIN], [OrganizationId], [IsActive]) VALUES (13, N'Beeline', N'contact@beeline.com', N'12349 Somewhere St.', N'Bellevue', (SELECT [StateId] FROM [Lookup].[State] WHERE [Name] = 'Washington'), (SELECT [CountryId] FROM [Lookup].[Country] WHERE [Name] = 'United States'), N'98007', N'1234567890', N'9876543210', N'www.beeline.com', N'', 1, 1)
INSERT [Crm].[Customer] ([CustomerId], [Name], [ContactEmail], [Address], [City], [State], [Country], [PostalCode], [ContactPhoneNumber], [FaxNumber], [Website], [EIN], [OrganizationId], [IsActive]) VALUES (14, N'Microsoft', N'contact@microsoft.com', N'12350 Somewhere St.', N'Bellevue', (SELECT [StateId] FROM [Lookup].[State] WHERE [Name] = 'Washington'), (SELECT [CountryId] FROM [Lookup].[Country] WHERE [Name] = 'United States'), N'98007', N'1234567890', N'9876543210', N'www.microsoft.com', N'',1, 1)
INSERT [Crm].[Customer] ([CustomerId], [Name], [ContactEmail], [Address], [City], [State], [Country], [PostalCode], [ContactPhoneNumber], [FaxNumber], [Website], [EIN], [OrganizationId], [IsActive]) VALUES (15, N'Caradigm', N'contact@caradigm.com', N'12351 Somewhere St.', N'Bellevue', (SELECT [StateId] FROM [Lookup].[State] WHERE [Name] = 'Washington'), (SELECT [CountryId] FROM [Lookup].[Country] WHERE [Name] = 'United States'), N'98007', N'1234567890', N'9876543210', N'www.caradigm.com', N'',1, 1)
INSERT [Crm].[Customer] ([CustomerId], [Name], [ContactEmail], [Address], [City], [State], [Country], [PostalCode], [ContactPhoneNumber], [FaxNumber], [Website], [EIN], [OrganizationId], [IsActive]) VALUES (16, N'Group Health', N'contact@grouphealth.com', N'12352 Somewhere St.', N'Bellevue', (SELECT [StateId] FROM [Lookup].[State] WHERE [Name] = 'Washington'), (SELECT [CountryId] FROM [Lookup].[Country] WHERE [Name] = 'United States'), N'98007', N'1234567890', N'9876543210', N'www.grouphealth.com', N'', 1, 1)
INSERT [Crm].[Customer] ([CustomerId], [Name], [ContactEmail], [Address], [City], [State], [Country], [PostalCode], [ContactPhoneNumber], [FaxNumber], [Website], [EIN], [OrganizationId], [IsActive]) VALUES (17, N'Savers', N'contact@savers.com', N'12353 Somewhere St.', N'Bellevue', (SELECT [StateId] FROM [Lookup].[State] WHERE [Name] = 'Washington'), (SELECT [CountryId] FROM [Lookup].[Country] WHERE [Name] = 'United States'), N'98007', N'1234567890', N'9876543210', N'www.caradigm.com', N'', 1, 1)

SET IDENTITY_INSERT [Crm].[Customer] OFF
