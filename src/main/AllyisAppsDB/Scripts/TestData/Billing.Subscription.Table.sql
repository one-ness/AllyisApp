PRINT 'Subscription'
SET IDENTITY_INSERT [Billing].[Subscription] ON 

INSERT [Billing].[Subscription] ([SubscriptionId], [OrganizationId], [SkuId], [NumberOfUsers], [IsActive]) VALUES (1, 1, 2, 285, 1)
SET IDENTITY_INSERT [Billing].[Subscription] OFF
