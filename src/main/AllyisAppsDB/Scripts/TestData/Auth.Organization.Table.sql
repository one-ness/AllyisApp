PRINT 'Organization'
SET IDENTITY_INSERT [Auth].[Organization] OFF 
SET IDENTITY_INSERT [Auth].[Organization] ON 

INSERT [Auth].[Organization] ([OrganizationId], [Name], [SiteUrl], [Address], [City], [State], [Country], [PostalCode], [PhoneNumber], [Subdomain]) VALUES (1, N'NotAllyis', N'www.notallyis.com', N'10210 NE Points Dr.', N'Kirkland', (SELECT [StateId] FROM [Lookup].[State] WHERE [Name] = 'Washington'), (SELECT [CountryId] FROM [Lookup].[Country] WHERE [Name] = 'United States'), N'98033', N'1234567890', N'notallyis')
SET IDENTITY_INSERT [Auth].[Organization] OFF
