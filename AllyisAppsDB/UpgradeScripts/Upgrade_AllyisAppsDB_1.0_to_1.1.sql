
GO
PRINT N'Rename refactoring operation with key d901f09c-0c58-4748-aecf-4ad1c229cec3 is skipped, element [Billing].[Subscription].[CreatedUtc] (SqlSimpleColumn) will not be renamed to CreatedUtc';


GO
PRINT N'Rename refactoring operation with key ce51f5cb-9d8b-4a7b-853c-8fc8b2053742 is skipped, element [Billing].[Subscription].[ModifiedUtc] (SqlSimpleColumn) will not be renamed to ModifiedUtc';


GO
PRINT N'Rename refactoring operation with key e279e838-1485-4ffd-a449-403eddfd0075 is skipped, element [Billing].[Sku].[EntityName] (SqlSimpleColumn) will not be renamed to BlockBasedOn';


GO
PRINT N'Rename refactoring operation with key d56982a6-c07f-4906-8384-9418fed309e1 is skipped, element [Auth].[OrganizationRole].[OrganizationRoleId] (SqlSimpleColumn) will not be renamed to [OrganizationRoleId]';


GO
PRINT N'Rename refactoring operation with key fc38992e-deca-44cf-8bea-6d1e6d5ef351 is skipped, element [Auth].[OrganizationUser].[OrganizationRoleId] (SqlSimpleColumn) will not be renamed to [OrganizationRoleId]';


GO
PRINT N'Rename refactoring operation with key 3ad9644e-6ddb-42f8-bba9-4f37e5ead6eb is skipped, element [Auth].[Invitation].[OrganizationRoleId] (SqlSimpleColumn) will not be renamed to [OrganizationRoleId]';


GO
PRINT N'Rename refactoring operation with key e2d5f599-b8f0-487b-bfa7-d147f7a7ae21 is skipped, element [Auth].[User].[ActiveOrganizationId] (SqlSimpleColumn) will not be renamed to [LastUsedOrganizationId]';


GO
PRINT N'Rename refactoring operation with key 9e6a781f-e569-4948-9876-383c50bc3183 is skipped, element [Auth].[User].[LastSubscriptionId] (SqlSimpleColumn) will not be renamed to [LastUsedSubscriptionId]';


GO
PRINT N'Rename refactoring operation with key a83ff01f-54e5-4ef4-9ae0-3530353217de is skipped, element [Auth].[User].[LanguagePreference] (SqlSimpleColumn) will not be renamed to [PreferredLanguageId]';


GO
PRINT N'The following operation was generated from a refactoring log file a63a65fd-25c4-4564-9739-f4e8eed1b07c';

PRINT N'Rename [Auth].[Organization].[Name] to OrganizationName';


GO
EXECUTE sp_rename @objname = N'[Auth].[Organization].[Name]', @newname = N'OrganizationName', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 19adb6b7-0b7b-4b3e-8b45-47068691e044';

PRINT N'Rename [Auth].[OrganizationRole].[Name] to OrganizationRoleName';


GO
EXECUTE sp_rename @objname = N'[Auth].[OrganizationRole].[Name]', @newname = N'OrganizationRoleName', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 6f07e028-5275-4b73-ac78-8eb031df5900';

PRINT N'Rename [Auth].[ProductRole].[Name] to ProductRoleName';


GO
EXECUTE sp_rename @objname = N'[Auth].[ProductRole].[Name]', @newname = N'ProductRoleName', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 6db4c03a-8567-487e-94ac-e4a03660f939';

PRINT N'Rename [Billing].[Product].[Name] to ProductName';


GO
EXECUTE sp_rename @objname = N'[Billing].[Product].[Name]', @newname = N'ProductName', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file cc86e587-721d-4924-b0ae-f9be368a3728';

PRINT N'Rename [Billing].[Sku].[Name] to SkuName';


GO
EXECUTE sp_rename @objname = N'[Billing].[Sku].[Name]', @newname = N'SkuName', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 7512f0b9-df37-401a-9910-7ed407fa20a3';

PRINT N'Rename [Crm].[Customer].[Name] to CustomerName';


GO
EXECUTE sp_rename @objname = N'[Crm].[Customer].[Name]', @newname = N'CustomerName', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 096d1cbd-fc9b-435c-bddc-aa83acb563ae';

PRINT N'Rename [Hrm].[PayClass].[Name] to PayClassName';


GO
EXECUTE sp_rename @objname = N'[Hrm].[PayClass].[Name]', @newname = N'PayClassName', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file bd89e74e-e33b-4308-9a39-377fb5b94c6f';

PRINT N'Rename [Lookup].[Country].[Name] to CountryName';


GO
EXECUTE sp_rename @objname = N'[Lookup].[Country].[Name]', @newname = N'CountryName', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 211ebb96-ecbd-45d8-b3fe-81a703a6bb94';

PRINT N'Rename [Lookup].[State].[Name] to StateName';


GO
EXECUTE sp_rename @objname = N'[Lookup].[State].[Name]', @newname = N'StateName', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 7519bf08-52a9-4575-9f57-472044e93083';

PRINT N'Rename [Pjm].[Project].[Name] to ProjectName';


GO
EXECUTE sp_rename @objname = N'[Pjm].[Project].[Name]', @newname = N'ProjectName', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 966e63d4-8b9f-4ada-bf32-bbf044843d07';

PRINT N'Rename [Auth].[Invitation].[CreatedUtc] to InvitationCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[Auth].[Invitation].[CreatedUtc]', @newname = N'InvitationCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 5e68ff46-6f6e-4378-870e-4a1d83294bab';

PRINT N'Rename [Auth].[Logging].[DateModifiedUtc] to LoggingDateModifiedUtc';


GO
EXECUTE sp_rename @objname = N'[Auth].[Logging].[DateModifiedUtc]', @newname = N'LoggingDateModifiedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 2d66c2ed-1503-4a43-8c7b-85cd9db6afc3';

PRINT N'Rename [Auth].[Organization].[CreatedUtc] to OrganizationCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[Auth].[Organization].[CreatedUtc]', @newname = N'OrganizationCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 28d87a44-ab6f-4518-9c2b-a91bbe5b8e6a';

PRINT N'Rename [Auth].[OrganizationUser].[CreatedUtc] to OrganizationUserCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[Auth].[OrganizationUser].[CreatedUtc]', @newname = N'OrganizationUserCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file b64b80b2-dd09-42e0-a52f-7a2b3eaa8c9d';

PRINT N'Rename [Auth].[User].[CreatedUtc] to UserCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[Auth].[User].[CreatedUtc]', @newname = N'UserCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file e2ebc197-28fe-4f40-9283-03f3072cd7ef';

PRINT N'Rename [Billing].[BillingHistory].[CreatedUtc] to BillingHistoryCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[Billing].[BillingHistory].[CreatedUtc]', @newname = N'BillingHistoryCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 8f896573-4e65-4726-9aa6-3f7001c87755';

PRINT N'Rename [Billing].[BillingHistory].[ModifiedUtc] to BillingHistoryModifiedUtc';


GO
EXECUTE sp_rename @objname = N'[Billing].[BillingHistory].[ModifiedUtc]', @newname = N'BillingHistoryModifiedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file f6a59a61-e520-4701-87f1-484ac030d378';

PRINT N'Rename [Billing].[StripeCustomerSubscriptionPlan].[CreatedUtc] to StripeCustomerSubscriptionPlanCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[Billing].[StripeCustomerSubscriptionPlan].[CreatedUtc]', @newname = N'StripeCustomerSubscriptionPlanCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 55937b5b-a45f-47b9-8998-81639d42a514';

PRINT N'Rename [Billing].[StripeCustomerSubscriptionPlan].[ModifiedUtc] to StripeCustomerSubscriptionPlanModifiedUtc';


GO
EXECUTE sp_rename @objname = N'[Billing].[StripeCustomerSubscriptionPlan].[ModifiedUtc]', @newname = N'StripeCustomerSubscriptionPlanModifiedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file ec6c346d-91ca-49ee-b20e-6d1e9e4973c7';

PRINT N'Rename [Billing].[StripeOrganizationCustomer].[CreatedUtc] to StripeOrganizationCustomerCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[Billing].[StripeOrganizationCustomer].[CreatedUtc]', @newname = N'StripeOrganizationCustomerCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 18ebc0f1-fe47-4b2a-ae14-3d688ec0bf59';

PRINT N'Rename [Billing].[StripeOrganizationCustomer].[ModifiedUtc] to StripeOrganizationCustomerModifiedUtc';


GO
EXECUTE sp_rename @objname = N'[Billing].[StripeOrganizationCustomer].[ModifiedUtc]', @newname = N'StripeOrganizationCustomerModifiedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 012aa26a-a293-45ff-ab49-366c72759ae5';

PRINT N'Rename [Billing].[Subscription].[CreatedUtc] to SubscriptionCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[Billing].[Subscription].[CreatedUtc]', @newname = N'SubscriptionCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 7b8c941a-8ac3-41e7-b812-739d2cbff2e0';

PRINT N'Rename [Billing].[Subscription].[ModifiedUtc] to SubscriptionModifiedUtc';


GO
EXECUTE sp_rename @objname = N'[Billing].[Subscription].[ModifiedUtc]', @newname = N'SubscriptionModifiedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 87e3eb67-4a6f-4543-bc7a-c88d4ac50b66';

PRINT N'Rename [Billing].[SubscriptionUser].[CreatedUtc] to SubscriptionUserCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[Billing].[SubscriptionUser].[CreatedUtc]', @newname = N'SubscriptionUserCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 5fdd2352-b43e-4123-b372-017df268fe9b';

PRINT N'Rename [Crm].[Customer].[CreatedUtc] to CustomerCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[Crm].[Customer].[CreatedUtc]', @newname = N'CustomerCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 73cc4553-2ee2-45df-8380-255b923f6254';

PRINT N'Rename [Expense].[ExpenseItem].[CreatedUtc] to ExpenseItemCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[Expense].[ExpenseItem].[CreatedUtc]', @newname = N'ExpenseItemCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file abe3058c-2402-473d-884c-a2cc825de950';

PRINT N'Rename [Expense].[ExpenseItem].[ModifiedUtc] to ExpenseItemModifiedUtc';


GO
EXECUTE sp_rename @objname = N'[Expense].[ExpenseItem].[ModifiedUtc]', @newname = N'ExpenseItemModifiedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 8aefaa49-66c9-4154-8530-613aa19b1ac1';

PRINT N'Rename [Expense].[ExpenseReport].[CreatedUtc] to ExpenseReportCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[Expense].[ExpenseReport].[CreatedUtc]', @newname = N'ExpenseReportCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 36cecd4a-2d4a-4cbc-a037-f3c8e238d6f3';

PRINT N'Rename [Expense].[ExpenseReport].[ModifiedUtc] to ExpenseReportModifiedUtc';


GO
EXECUTE sp_rename @objname = N'[Expense].[ExpenseReport].[ModifiedUtc]', @newname = N'ExpenseReportModifiedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 5ae1c196-02d1-4722-944b-d00a840814b2';

PRINT N'Rename [Pjm].[Project].[CreatedUtc] to ProjectCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[Pjm].[Project].[CreatedUtc]', @newname = N'ProjectCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 9e7a3ff2-c3dd-41e0-95fa-9e73129c20ca';

PRINT N'Rename [Pjm].[ProjectUser].[CreatedUtc] to ProjectUserCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[Pjm].[ProjectUser].[CreatedUtc]', @newname = N'ProjectUserCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 2f8ee18d-4ed2-4fea-b930-654b1b29b393';

PRINT N'Rename [TimeTracker].[TimeEntry].[CreatedUtc] to TimeEntryCreatedUtc';


GO
EXECUTE sp_rename @objname = N'[TimeTracker].[TimeEntry].[CreatedUtc]', @newname = N'TimeEntryCreatedUtc', @objtype = N'COLUMN';


GO
PRINT N'The following operation was generated from a refactoring log file 173ca97f-7ea2-441a-be18-3cf8d69b6ac2';

PRINT N'Rename [TimeTracker].[TimeEntry].[ModifiedUtc] to TimeEntryModifiedUtc';


GO
EXECUTE sp_rename @objname = N'[TimeTracker].[TimeEntry].[ModifiedUtc]', @newname = N'TimeEntryModifiedUtc', @objtype = N'COLUMN';


GO
PRINT N'Dropping [Auth].[OrganizationUser].[IX_OrganizationUser]...';


GO
DROP INDEX [IX_OrganizationUser]
    ON [Auth].[OrganizationUser];


GO
PRINT N'Dropping [Auth].[DF__User__EmailConfi__66603565]...';


GO
ALTER TABLE [Auth].[User] DROP CONSTRAINT [DF__User__EmailConfi__66603565];


GO
PRINT N'Dropping [Auth].[DF__User__PhoneNumbe__6754599E]...';


GO
ALTER TABLE [Auth].[User] DROP CONSTRAINT [DF__User__PhoneNumbe__6754599E];


GO
PRINT N'Dropping [Auth].[DF__User__TwoFactorE__68487DD7]...';


GO
ALTER TABLE [Auth].[User] DROP CONSTRAINT [DF__User__TwoFactorE__68487DD7];


GO
PRINT N'Dropping [Auth].[DF__User__AccessFail__693CA210]...';


GO
ALTER TABLE [Auth].[User] DROP CONSTRAINT [DF__User__AccessFail__693CA210];


GO
PRINT N'Dropping [Auth].[DF__User__LockoutEna__6A30C649]...';


GO
ALTER TABLE [Auth].[User] DROP CONSTRAINT [DF__User__LockoutEna__6A30C649];


GO
PRINT N'Dropping [Auth].[DF__User__CreatedUtc__6B24EA82]...';


GO
ALTER TABLE [Auth].[User] DROP CONSTRAINT [DF__User__CreatedUtc__6B24EA82];


GO
PRINT N'Dropping unnamed constraint on [Billing].[Subscription]...';


GO
ALTER TABLE [Billing].[Subscription] DROP CONSTRAINT [DF__Subscript__Numbe__778AC167];


GO
PRINT N'Dropping [Billing].[DF_Subscription_IsActive]...';


GO
ALTER TABLE [Billing].[Subscription] DROP CONSTRAINT [DF_Subscription_IsActive];


GO
PRINT N'Dropping [Billing].[DF_Subscription_CreatedUtc]...';


GO
ALTER TABLE [Billing].[Subscription] DROP CONSTRAINT [DF_Subscription_CreatedUtc];


GO
PRINT N'Dropping [Billing].[DF_Subscription_ModifiedUtc]...';


GO
ALTER TABLE [Billing].[Subscription] DROP CONSTRAINT [DF_Subscription_ModifiedUtc];


GO
PRINT N'Dropping unnamed constraint on [Pjm].[Project]...';


GO
ALTER TABLE [Pjm].[Project] DROP CONSTRAINT [DF__Project__Type__05D8E0BE];


GO
PRINT N'Dropping unnamed constraint on [Pjm].[Project]...';


GO
ALTER TABLE [Pjm].[Project] DROP CONSTRAINT [DF__Project__IsActiv__06CD04F7];


GO
PRINT N'Dropping unnamed constraint on [Pjm].[Project]...';


GO
ALTER TABLE [Pjm].[Project] DROP CONSTRAINT [DF__Project__Created__07C12930];


GO
PRINT N'Dropping [TimeTracker].[DF_Setting_StartOfWeek]...';


GO
ALTER TABLE [TimeTracker].[Setting] DROP CONSTRAINT [DF_Setting_StartOfWeek];


GO
PRINT N'Dropping [TimeTracker].[DF_Setting_OvertimeHours]...';


GO
ALTER TABLE [TimeTracker].[Setting] DROP CONSTRAINT [DF_Setting_OvertimeHours];


GO
PRINT N'Dropping [TimeTracker].[DF_Setting_OvertimePeriod]...';


GO
ALTER TABLE [TimeTracker].[Setting] DROP CONSTRAINT [DF_Setting_OvertimePeriod];


GO
PRINT N'Dropping [TimeTracker].[DF_Setting_OvertimeMultiplier]...';


GO
ALTER TABLE [TimeTracker].[Setting] DROP CONSTRAINT [DF_Setting_OvertimeMultiplier];


GO
PRINT N'Dropping [TimeTracker].[DF_Setting_LockDateUsed]...';


GO
ALTER TABLE [TimeTracker].[Setting] DROP CONSTRAINT [DF_Setting_LockDateUsed];


GO
PRINT N'Dropping [TimeTracker].[DF_Setting_LockDatePeriod]...';


GO
ALTER TABLE [TimeTracker].[Setting] DROP CONSTRAINT [DF_Setting_LockDatePeriod];


GO
PRINT N'Dropping [TimeTracker].[DF_Setting_LockDateQuantity]...';


GO
ALTER TABLE [TimeTracker].[Setting] DROP CONSTRAINT [DF_Setting_LockDateQuantity];


GO
PRINT N'Dropping unnamed constraint on [TimeTracker].[TimeEntry]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] DROP CONSTRAINT [DF__TimeEntry__LockS__10566F31];


GO
PRINT N'Dropping unnamed constraint on [TimeTracker].[TimeEntry]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] DROP CONSTRAINT [DF__TimeEntry__PayCl__114A936A];


GO
PRINT N'Dropping unnamed constraint on [TimeTracker].[TimeEntry]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] DROP CONSTRAINT [DF__TimeEntry__Creat__123EB7A3];


GO
PRINT N'Dropping unnamed constraint on [TimeTracker].[TimeEntry]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] DROP CONSTRAINT [DF__TimeEntry__Modif__1332DBDC];


GO
PRINT N'Dropping unnamed constraint on [Auth].[Invitation]...';


GO
ALTER TABLE [Auth].[Invitation] DROP CONSTRAINT [DF__Invitatio__Creat__619B8048];


GO
PRINT N'Dropping unnamed constraint on [Auth].[Logging]...';


GO
ALTER TABLE [Auth].[Logging] DROP CONSTRAINT [DF__Logging__DateMod__628FA481];


GO
PRINT N'Dropping unnamed constraint on [Auth].[Organization]...';


GO
ALTER TABLE [Auth].[Organization] DROP CONSTRAINT [DF__Organizat__IsAct__6383C8BA];


GO
PRINT N'Dropping unnamed constraint on [Auth].[Organization]...';


GO
ALTER TABLE [Auth].[Organization] DROP CONSTRAINT [DF__Organizat__Creat__6477ECF3];


GO
PRINT N'Dropping unnamed constraint on [Auth].[OrganizationUser]...';


GO
ALTER TABLE [Auth].[OrganizationUser] DROP CONSTRAINT [DF__Organizat__Creat__656C112C];


GO
PRINT N'Dropping unnamed constraint on [Auth].[Permission]...';


GO
ALTER TABLE [Auth].[Permission] DROP CONSTRAINT [DF__Permissio__IsAll__66603565];


GO
PRINT N'Dropping [Billing].[DF__BillingHi__Creat__6A30C649]...';


GO
ALTER TABLE [Billing].[BillingHistory] DROP CONSTRAINT [DF__BillingHi__Creat__6A30C649];


GO
PRINT N'Dropping [Billing].[DF__BillingHi__Modif__6B24EA82]...';


GO
ALTER TABLE [Billing].[BillingHistory] DROP CONSTRAINT [DF__BillingHi__Modif__6B24EA82];


GO
PRINT N'Dropping [Billing].[DF__Product__IsActiv__693CA210]...';


GO
ALTER TABLE [Billing].[Product] DROP CONSTRAINT [DF__Product__IsActiv__693CA210];


GO
PRINT N'Dropping [Billing].[DF__Sku__BillingFreq__6EF57B66]...';


GO
ALTER TABLE [Billing].[Sku] DROP CONSTRAINT [DF__Sku__BillingFreq__6EF57B66];


GO
PRINT N'Dropping [Billing].[DF__Sku__BlockSize__6FE99F9F]...';


GO
ALTER TABLE [Billing].[Sku] DROP CONSTRAINT [DF__Sku__BlockSize__6FE99F9F];


GO
PRINT N'Dropping [Billing].[DF__Sku__IsActive__70DDC3D8]...';


GO
ALTER TABLE [Billing].[Sku] DROP CONSTRAINT [DF__Sku__IsActive__70DDC3D8];


GO
PRINT N'Dropping [Billing].[DF__Sku__UserLimit__6E01572D]...';


GO
ALTER TABLE [Billing].[Sku] DROP CONSTRAINT [DF__Sku__UserLimit__6E01572D];


GO
PRINT N'Dropping [Billing].[DF__StripeCus__Creat__6C190EBB]...';


GO
ALTER TABLE [Billing].[StripeCustomerSubscriptionPlan] DROP CONSTRAINT [DF__StripeCus__Creat__6C190EBB];


GO
PRINT N'Dropping [Billing].[DF__StripeCus__Modif__6D0D32F4]...';


GO
ALTER TABLE [Billing].[StripeCustomerSubscriptionPlan] DROP CONSTRAINT [DF__StripeCus__Modif__6D0D32F4];


GO
PRINT N'Dropping unnamed constraint on [Billing].[StripeOrganizationCustomer]...';


GO
ALTER TABLE [Billing].[StripeOrganizationCustomer] DROP CONSTRAINT [DF__StripeOrg__Creat__75A278F5];


GO
PRINT N'Dropping unnamed constraint on [Billing].[StripeOrganizationCustomer]...';


GO
ALTER TABLE [Billing].[StripeOrganizationCustomer] DROP CONSTRAINT [DF__StripeOrg__Modif__76969D2E];


GO
PRINT N'Dropping unnamed constraint on [Crm].[Customer]...';


GO
ALTER TABLE [Crm].[Customer] DROP CONSTRAINT [DF__Customer__IsActi__7C4F7684];


GO
PRINT N'Dropping unnamed constraint on [Crm].[Customer]...';


GO
ALTER TABLE [Crm].[Customer] DROP CONSTRAINT [DF__Customer__Create__7D439ABD];


GO
PRINT N'Dropping unnamed constraint on [Expense].[ExpenseItem]...';


GO
ALTER TABLE [Expense].[ExpenseItem] DROP CONSTRAINT [DF__ExpenseIt__IsBil__7E37BEF6];


GO
PRINT N'Dropping unnamed constraint on [Expense].[ExpenseItem]...';


GO
ALTER TABLE [Expense].[ExpenseItem] DROP CONSTRAINT [DF__ExpenseIt__Creat__7F2BE32F];


GO
PRINT N'Dropping unnamed constraint on [Expense].[ExpenseItem]...';


GO
ALTER TABLE [Expense].[ExpenseItem] DROP CONSTRAINT [DF__ExpenseIt__Modif__00200768];


GO
PRINT N'Dropping unnamed constraint on [Expense].[ExpenseReport]...';


GO
ALTER TABLE [Expense].[ExpenseReport] DROP CONSTRAINT [DF__ExpenseRe__Repor__01142BA1];


GO
PRINT N'Dropping unnamed constraint on [Expense].[ExpenseReport]...';


GO
ALTER TABLE [Expense].[ExpenseReport] DROP CONSTRAINT [DF__ExpenseRe__Creat__02084FDA];


GO
PRINT N'Dropping unnamed constraint on [Expense].[ExpenseReport]...';


GO
ALTER TABLE [Expense].[ExpenseReport] DROP CONSTRAINT [DF__ExpenseRe__Modif__02FC7413];


GO
PRINT N'Dropping unnamed constraint on [Finance].[Account]...';


GO
ALTER TABLE [Finance].[Account] DROP CONSTRAINT [DF__Account__IsActiv__03F0984C];


GO
PRINT N'Dropping unnamed constraint on [Hrm].[Holiday]...';


GO
ALTER TABLE [Hrm].[Holiday] DROP CONSTRAINT [DF__Holiday__Organiz__04E4BC85];


GO
PRINT N'Dropping unnamed constraint on [Pjm].[ProjectUser]...';


GO
ALTER TABLE [Pjm].[ProjectUser] DROP CONSTRAINT [DF__ProjectUs__Creat__08B54D69];


GO
PRINT N'Dropping [Auth].[FK_Logging_User]...';


GO
ALTER TABLE [Auth].[Logging] DROP CONSTRAINT [FK_Logging_User];


GO
PRINT N'Dropping [Auth].[FK_OrganizationUser_User]...';


GO
ALTER TABLE [Auth].[OrganizationUser] DROP CONSTRAINT [FK_OrganizationUser_User];


GO
PRINT N'Dropping [Auth].[FK_User_Language]...';


GO
ALTER TABLE [Auth].[User] DROP CONSTRAINT [FK_User_Language];


GO
PRINT N'Dropping [Auth].[FK_User_Organization]...';


GO
ALTER TABLE [Auth].[User] DROP CONSTRAINT [FK_User_Organization];


GO
PRINT N'Dropping [Auth].[FK_User_Subscription]...';


GO
ALTER TABLE [Auth].[User] DROP CONSTRAINT [FK_User_Subscription];


GO
PRINT N'Dropping [Billing].[FK_BillingHistory_User]...';


GO
ALTER TABLE [Billing].[BillingHistory] DROP CONSTRAINT [FK_BillingHistory_User];


GO
PRINT N'Dropping [Billing].[FK_SubscriptionUser_User]...';


GO
ALTER TABLE [Billing].[SubscriptionUser] DROP CONSTRAINT [FK_SubscriptionUser_User];


GO
PRINT N'Dropping [Expense].[FK_ExpenseReport_User]...';


GO
ALTER TABLE [Expense].[ExpenseReport] DROP CONSTRAINT [FK_ExpenseReport_User];


GO
PRINT N'Dropping [Pjm].[FK_ProjectUser_User]...';


GO
ALTER TABLE [Pjm].[ProjectUser] DROP CONSTRAINT [FK_ProjectUser_User];


GO
PRINT N'Dropping [TimeTracker].[FK_TimeEntry_User]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] DROP CONSTRAINT [FK_TimeEntry_User];


GO
PRINT N'Dropping [Billing].[FK_Subscription_Organization]...';


GO
ALTER TABLE [Billing].[Subscription] DROP CONSTRAINT [FK_Subscription_Organization];


GO
PRINT N'Dropping [Billing].[FK_SubscriptionUser_Subscription]...';


GO
ALTER TABLE [Billing].[SubscriptionUser] DROP CONSTRAINT [FK_SubscriptionUser_Subscription];


GO
PRINT N'Dropping [Pjm].[FK_Project_Customer]...';


GO
ALTER TABLE [Pjm].[Project] DROP CONSTRAINT [FK_Project_Customer];


GO
PRINT N'Dropping [Pjm].[FK_ProjectUser_Project]...';


GO
ALTER TABLE [Pjm].[ProjectUser] DROP CONSTRAINT [FK_ProjectUser_Project];


GO
PRINT N'Dropping [TimeTracker].[FK_TimeEntry_Project]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] DROP CONSTRAINT [FK_TimeEntry_Project];


GO
PRINT N'Dropping [TimeTracker].[FK_Settings_Organization]...';


GO
ALTER TABLE [TimeTracker].[Setting] DROP CONSTRAINT [FK_Settings_Organization];


GO
PRINT N'Dropping [Auth].[FK_Invitation_EmployeeType]...';


GO
ALTER TABLE [Auth].[Invitation] DROP CONSTRAINT [FK_Invitation_EmployeeType];


GO
PRINT N'Dropping [Auth].[FK_OrganizationUser_EmployeeType]...';


GO
ALTER TABLE [Auth].[OrganizationUser] DROP CONSTRAINT [FK_OrganizationUser_EmployeeType];


GO
PRINT N'Dropping [Billing].[FK_CustomerSubscriptionPlan_Organization]...';


GO
ALTER TABLE [Billing].[StripeCustomerSubscriptionPlan] DROP CONSTRAINT [FK_CustomerSubscriptionPlan_Organization];


GO
PRINT N'Dropping [Billing].[FK_CustomerSubscriptionPlan_Product]...';


GO
ALTER TABLE [Billing].[StripeCustomerSubscriptionPlan] DROP CONSTRAINT [FK_CustomerSubscriptionPlan_Product];


GO
PRINT N'Dropping unnamed constraint on [Billing].[StripeOrganizationCustomer]...';


GO
ALTER TABLE [Billing].[StripeOrganizationCustomer] DROP CONSTRAINT [PK__StripeOr__D7983C831B58F1D5];


GO
PRINT N'Dropping [Hrm].[PK_PayClass_Id]...';


GO
ALTER TABLE [Hrm].[PayClass] DROP CONSTRAINT [PK_PayClass_Id];


GO
PRINT N'Dropping [Pjm].[PK_OrganizationUser_ProjectId_UserId]...';


GO
ALTER TABLE [Pjm].[ProjectUser] DROP CONSTRAINT [PK_OrganizationUser_ProjectId_UserId];


GO
PRINT N'Dropping [Auth].[CreateOrg]...';


GO
DROP PROCEDURE [Auth].[CreateOrg];


GO
PRINT N'Dropping [Auth].[CreateOrgUser]...';


GO
DROP PROCEDURE [Auth].[CreateOrgUser];


GO
PRINT N'Dropping [Auth].[CreateUserInfo]...';


GO
DROP PROCEDURE [Auth].[CreateUserInfo];


GO
PRINT N'Dropping [Auth].[EditOrgUsers]...';


GO
DROP PROCEDURE [Auth].[EditOrgUsers];


GO
PRINT N'Dropping [Auth].[UpdateOrg]...';


GO
DROP PROCEDURE [Auth].[UpdateOrg];


GO
PRINT N'Dropping [Billing].[CreateStripeOrgCustomer]...';


GO
DROP PROCEDURE [Billing].[CreateStripeOrgCustomer];


GO
PRINT N'Dropping [Billing].[EditSubscriptionUsers]...';


GO
DROP PROCEDURE [Billing].[EditSubscriptionUsers];


GO
PRINT N'Dropping [Hrm].[GetEmployeeTypeId]...';


GO
DROP PROCEDURE [Hrm].[GetEmployeeTypeId];


GO
PRINT N'Dropping [Hrm].[EmployeeType]...';


GO
DROP TABLE [Hrm].[EmployeeType];


GO
PRINT N'Altering [aaUser]...';


GO
ALTER USER [aaUser]
    WITH LOGIN = [aaUser];


GO
PRINT N'Altering [Auth].[Invitation]...';


GO
ALTER TABLE [Auth].[Invitation] DROP COLUMN [EmployeeTypeId];


GO
PRINT N'Altering [Auth].[OrganizationUser]...';


GO
ALTER TABLE [Auth].[OrganizationUser] DROP COLUMN [EmployeeTypeId];


GO
PRINT N'Creating [Auth].[OrganizationUser].[IX_OrganizationUser]...';


GO
CREATE NONCLUSTERED INDEX [IX_OrganizationUser]
    ON [Auth].[OrganizationUser]([UserId] ASC, [OrganizationRoleId] ASC);


GO
PRINT N'Starting rebuilding table [Auth].[User]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Auth].[tmp_ms_xx_User] (
    [UserId]                 INT              IDENTITY (111119, 3) NOT NULL,
    [FirstName]              NVARCHAR (32)    NOT NULL,
    [LastName]               NVARCHAR (32)    NOT NULL,
    [AddressId]              INT              NOT NULL,
    [Email]                  NVARCHAR (384)   NOT NULL,
    [PasswordHash]           NVARCHAR (512)   NOT NULL,
    [IsEmailConfirmed]       BIT              CONSTRAINT [DF__User__EmailConfirmed] DEFAULT ((0)) NOT NULL,
    [IsPhoneNumberConfirmed] BIT              CONSTRAINT [DF__User__PhoneNumberConfirmed] DEFAULT ((0)) NOT NULL,
    [IsTwoFactorEnabled]     BIT              CONSTRAINT [DF__User__TwoFactorEnabled] DEFAULT ((0)) NOT NULL,
    [AccessFailedCount]      INT              CONSTRAINT [DF__User__AccessFailedCount] DEFAULT ((0)) NOT NULL,
    [IsLockoutEnabled]       BIT              CONSTRAINT [DF__User__LockoutEnabled] DEFAULT ((0)) NOT NULL,
    [UserCreatedUtc]         DATETIME2 (0)    CONSTRAINT [DF__User__UserCreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [PreferredLanguageId]    INT              NULL,
    [DateOfBirth]            DATE             NULL,
    [PhoneNumber]            VARCHAR (16)     NULL,
    [PhoneExtension]         VARCHAR (8)      NULL,
    [LastUsedSubscriptionId] INT              NULL,
    [LastUsedOrganizationId] INT              NULL,
    [LockoutEndDateUtc]      DATETIME2 (0)    NULL,
    [PasswordResetCode]      UNIQUEIDENTIFIER NULL,
    [EmailConfirmationCode]  UNIQUEIDENTIFIER NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_User1] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [tmp_ms_xx_constraint_UQ_User1] UNIQUE NONCLUSTERED ([Email] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Auth].[User])
    BEGIN
        SET IDENTITY_INSERT [Auth].[tmp_ms_xx_User] ON;
        INSERT INTO [Auth].[tmp_ms_xx_User] ([UserId], [FirstName], [LastName], [AddressId], [Email], [PasswordHash], [AccessFailedCount], [UserCreatedUtc], [PreferredLanguageId], [DateOfBirth], [PhoneNumber], [PhoneExtension], [LastUsedSubscriptionId], [LastUsedOrganizationId], [LockoutEndDateUtc], [PasswordResetCode], [EmailConfirmationCode])
        SELECT   [UserId],
                 [FirstName],
                 [LastName],
                 [AddressId],
                 [Email],
                 [PasswordHash],
                 [AccessFailedCount],
                 [UserCreatedUtc],
                 [PreferredLanguageId],
                 [DateOfBirth],
                 [PhoneNumber],
                 [PhoneExtension],
                 [LastUsedSubscriptionId],
                 [LastUsedOrganizationId],
                 [LockoutEndDateUtc],
                 [PasswordResetCode],
                 [EmailConfirmationCode]
        FROM     [Auth].[User]
        ORDER BY [UserId] ASC;
        SET IDENTITY_INSERT [Auth].[tmp_ms_xx_User] OFF;
    END

DROP TABLE [Auth].[User];

EXECUTE sp_rename N'[Auth].[tmp_ms_xx_User]', N'User';

EXECUTE sp_rename N'[Auth].[tmp_ms_xx_constraint_PK_User1]', N'PK_User', N'OBJECT';

EXECUTE sp_rename N'[Auth].[tmp_ms_xx_constraint_UQ_User1]', N'UQ_User', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [Auth].[User].[IX_User]...';


GO
CREATE NONCLUSTERED INDEX [IX_User]
    ON [Auth].[User]([Email] ASC, [FirstName] ASC, [LastName] ASC);


GO
PRINT N'Starting rebuilding table [Pjm].[Project]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Pjm].[tmp_ms_xx_Project] (
    [ProjectId]         INT           IDENTITY (116827, 3) NOT NULL,
    [CustomerId]        INT           NOT NULL,
    [ProjectName]       NVARCHAR (64) NOT NULL,
    [ProjectOrgId]      NVARCHAR (16) NOT NULL,
    [IsHourly]          BIT           CONSTRAINT [DF_Project_IsHourly] DEFAULT ((0)) NOT NULL,
    [IsActive]          BIT           CONSTRAINT [DF_Project_IsActive] DEFAULT ((1)) NOT NULL,
    [ProjectCreatedUtc] DATETIME2 (0) CONSTRAINT [DF_Project_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [StartUtc]          DATETIME2 (0) NULL,
    [EndUtc]            DATETIME2 (0) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Project1] PRIMARY KEY NONCLUSTERED ([ProjectId] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Pjm].[Project])
    BEGIN
        SET IDENTITY_INSERT [Pjm].[tmp_ms_xx_Project] ON;
        INSERT INTO [Pjm].[tmp_ms_xx_Project] ([ProjectId], [CustomerId], [ProjectName], [ProjectOrgId], [IsActive], [ProjectCreatedUtc], [StartUtc], [EndUtc])
        SELECT [ProjectId],
               [CustomerId],
               [ProjectName],
               [ProjectOrgId],
               [IsActive],
               [ProjectCreatedUtc],
               [StartUtc],
               [EndUtc]
        FROM   [Pjm].[Project];
        SET IDENTITY_INSERT [Pjm].[tmp_ms_xx_Project] OFF;
    END

DROP TABLE [Pjm].[Project];

EXECUTE sp_rename N'[Pjm].[tmp_ms_xx_Project]', N'Project';

EXECUTE sp_rename N'[Pjm].[tmp_ms_xx_constraint_PK_Project1]', N'PK_Project', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [Pjm].[Project].[IX_Project]...';


GO
CREATE NONCLUSTERED INDEX [IX_Project]
    ON [Pjm].[Project]([CustomerId] ASC);


GO
PRINT N'Starting rebuilding table [TimeTracker].[Setting]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [TimeTracker].[tmp_ms_xx_Setting] (
    [OrganizationId]     INT            NOT NULL,
    [StartOfWeek]        INT            CONSTRAINT [DF_Setting_StartOfWeek] DEFAULT ((1)) NOT NULL,
    [OvertimeHours]      INT            CONSTRAINT [DF_Setting_OvertimeHours] DEFAULT ((40)) NOT NULL,
    [OvertimePeriod]     VARCHAR (10)   CONSTRAINT [DF_Setting_OvertimePeriod] DEFAULT ('week') NOT NULL,
    [OvertimeMultiplier] DECIMAL (9, 4) CONSTRAINT [DF_Setting_OvertimeMultiplier] DEFAULT ((1.5)) NOT NULL,
    [IsLockDateUsed]     BIT            CONSTRAINT [DF_Setting_LockDateUsed] DEFAULT ((0)) NOT NULL,
    [LockDatePeriod]     INT            CONSTRAINT [DF_Setting_LockDatePeriod] DEFAULT (1) NOT NULL,
    [LockDateQuantity]   INT            CONSTRAINT [DF_Setting_LockDateQuantity] DEFAULT ((14)) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Setting1] PRIMARY KEY CLUSTERED ([OrganizationId] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [TimeTracker].[Setting])
    BEGIN
        INSERT INTO [TimeTracker].[tmp_ms_xx_Setting] ([OrganizationId], [StartOfWeek], [OvertimeHours], [OvertimePeriod], [OvertimeMultiplier], [LockDatePeriod], [LockDateQuantity])
        SELECT   [OrganizationId],
                 [StartOfWeek],
                 [OvertimeHours],
                 [OvertimePeriod],
                 [OvertimeMultiplier],
                 [LockDatePeriod],
                 [LockDateQuantity]
        FROM     [TimeTracker].[Setting]
        ORDER BY [OrganizationId] ASC;
    END

DROP TABLE [TimeTracker].[Setting];

EXECUTE sp_rename N'[TimeTracker].[tmp_ms_xx_Setting]', N'Setting';

EXECUTE sp_rename N'[TimeTracker].[tmp_ms_xx_constraint_PK_Setting1]', N'PK_Setting', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [TimeTracker].[Setting].[IX_Setting]...';


GO
CREATE NONCLUSTERED INDEX [IX_Setting]
    ON [TimeTracker].[Setting]([OrganizationId] ASC);


GO
PRINT N'Starting rebuilding table [TimeTracker].[TimeEntry]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [TimeTracker].[tmp_ms_xx_TimeEntry] (
    [TimeEntryId]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]               INT            NOT NULL,
    [ProjectId]            INT            NOT NULL,
    [Date]                 DATE           NOT NULL,
    [Duration]             FLOAT (53)     NOT NULL,
    [Description]          NVARCHAR (128) NULL,
    [IsLockSaved]          BIT            CONSTRAINT [DF_TimeEntry_IsLockSaved] DEFAULT 0 NOT NULL,
    [PayClassId]           INT            CONSTRAINT [DF_TimeEntry_PayClassId] DEFAULT 1 NOT NULL,
    [TimeEntryCreatedUtc]  DATETIME2 (0)  CONSTRAINT [DF_TimeEntry_CreatedUtc] DEFAULT getutcdate() NOT NULL,
    [TimeEntryModifiedUtc] DATETIME2 (0)  CONSTRAINT [DF_TimeEntry_ModifiedUtc] DEFAULT getutcdate() NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_TimeEntry1] PRIMARY KEY NONCLUSTERED ([TimeEntryId] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [TimeTracker].[TimeEntry])
    BEGIN
        SET IDENTITY_INSERT [TimeTracker].[tmp_ms_xx_TimeEntry] ON;
        INSERT INTO [TimeTracker].[tmp_ms_xx_TimeEntry] ([TimeEntryId], [UserId], [ProjectId], [Date], [Duration], [Description], [PayClassId], [TimeEntryCreatedUtc], [TimeEntryModifiedUtc])
        SELECT [TimeEntryId],
               [UserId],
               [ProjectId],
               [Date],
               [Duration],
               [Description],
               [PayClassId],
               [TimeEntryCreatedUtc],
               [TimeEntryModifiedUtc]
        FROM   [TimeTracker].[TimeEntry];
        SET IDENTITY_INSERT [TimeTracker].[tmp_ms_xx_TimeEntry] OFF;
    END

DROP TABLE [TimeTracker].[TimeEntry];

EXECUTE sp_rename N'[TimeTracker].[tmp_ms_xx_TimeEntry]', N'TimeEntry';

EXECUTE sp_rename N'[TimeTracker].[tmp_ms_xx_constraint_PK_TimeEntry1]', N'PK_TimeEntry', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [TimeTracker].[TimeEntry].[IX_FK_TimeEntry]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_TimeEntry]
    ON [TimeTracker].[TimeEntry]([UserId] ASC, [ProjectId] ASC);


GO
PRINT N'Starting rebuilding table [Billing].[StripeCustomerSubscriptionPlan]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Billing].[tmp_ms_xx_StripeCustomerSubscriptionPlan] (
    [StripeTokenCustId]                         NVARCHAR (50) NOT NULL,
    [StripeTokenSubId]                          NCHAR (50)    NOT NULL,
    [NumberOfUsers]                             INT           NOT NULL,
    [Price]                                     INT           NOT NULL,
    [ProductId]                                 INT           NOT NULL,
    [IsActive]                                  BIT           NOT NULL,
    [OrganizationId]                            INT           NOT NULL,
    [StripeCustomerSubscriptionPlanCreatedUtc]  DATETIME2 (0) CONSTRAINT [DF__StripeCustomerSubscriptionPlan__CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [StripeCustomerSubscriptionPlanModifiedUtc] DATETIME2 (0) CONSTRAINT [DF__StripeCustomerSubscriptionPlan__ModifiedUtc] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK__StripeCustomerSubscriptionPlan1] PRIMARY KEY CLUSTERED ([StripeTokenSubId] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Billing].[StripeCustomerSubscriptionPlan])
    BEGIN
        INSERT INTO [Billing].[tmp_ms_xx_StripeCustomerSubscriptionPlan] ([StripeTokenSubId], [StripeTokenCustId], [NumberOfUsers], [Price], [ProductId], [IsActive], [OrganizationId], [StripeCustomerSubscriptionPlanCreatedUtc], [StripeCustomerSubscriptionPlanModifiedUtc])
        SELECT   [StripeTokenSubId],
                 [StripeTokenCustId],
                 [NumberOfUsers],
                 [Price],
                 [ProductId],
                 [IsActive],
                 [OrganizationId],
                 [StripeCustomerSubscriptionPlanCreatedUtc],
                 [StripeCustomerSubscriptionPlanModifiedUtc]
        FROM     [Billing].[StripeCustomerSubscriptionPlan]
        ORDER BY [StripeTokenSubId] ASC;
    END

DROP TABLE [Billing].[StripeCustomerSubscriptionPlan];

EXECUTE sp_rename N'[Billing].[tmp_ms_xx_StripeCustomerSubscriptionPlan]', N'StripeCustomerSubscriptionPlan';

EXECUTE sp_rename N'[Billing].[tmp_ms_xx_constraint_PK__StripeCustomerSubscriptionPlan1]', N'PK__StripeCustomerSubscriptionPlan', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [Billing].[StripeCustomerSubscriptionPlan].[IX_FK_StripeCustomerSubscriptionPlan]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_StripeCustomerSubscriptionPlan]
    ON [Billing].[StripeCustomerSubscriptionPlan]([ProductId] ASC, [OrganizationId] ASC);


GO
PRINT N'Creating [Lookup].[OrganizationLocation]...';


GO
CREATE TABLE [Lookup].[OrganizationLocation] (
    [OrganizationId] INT           NOT NULL,
    [AddressId]      INT           NOT NULL,
    [LocationName]   NVARCHAR (32) NULL,
    CONSTRAINT [PK_OrganizationLocation] PRIMARY KEY CLUSTERED ([OrganizationId] ASC, [AddressId] ASC)
);


GO
PRINT N'Creating [Billing].[PK_StripeOrganizationCustomer]...';


GO
ALTER TABLE [Billing].[StripeOrganizationCustomer]
    ADD CONSTRAINT [PK_StripeOrganizationCustomer] PRIMARY KEY NONCLUSTERED ([StripeTokenCustId] ASC);


GO
PRINT N'Creating [Hrm].[PK_PayClass]...';


GO
ALTER TABLE [Hrm].[PayClass]
    ADD CONSTRAINT [PK_PayClass] PRIMARY KEY NONCLUSTERED ([PayClassId] ASC);


GO
PRINT N'Creating [Pjm].[PK_ProjectUser]...';


GO
ALTER TABLE [Pjm].[ProjectUser]
    ADD CONSTRAINT [PK_ProjectUser] PRIMARY KEY NONCLUSTERED ([ProjectId] ASC, [UserId] ASC);


GO
PRINT N'Creating [Auth].[DF_Invitation_CreatedUtc]...';


GO
ALTER TABLE [Auth].[Invitation]
    ADD CONSTRAINT [DF_Invitation_CreatedUtc] DEFAULT (getutcdate()) FOR [InvitationCreatedUtc];


GO
PRINT N'Creating [Auth].[DF_Logging_DateModifiedUtc]...';


GO
ALTER TABLE [Auth].[Logging]
    ADD CONSTRAINT [DF_Logging_DateModifiedUtc] DEFAULT getutcdate() FOR [LoggingDateModifiedUtc];


GO
PRINT N'Creating [Auth].[DF_Organization_CreatedUtc]...';


GO
ALTER TABLE [Auth].[Organization]
    ADD CONSTRAINT [DF_Organization_CreatedUtc] DEFAULT (getutcdate()) FOR [OrganizationCreatedUtc];


GO
PRINT N'Creating [Auth].[DF_Organization_IsActive]...';


GO
ALTER TABLE [Auth].[Organization]
    ADD CONSTRAINT [DF_Organization_IsActive] DEFAULT ((1)) FOR [IsActive];


GO
PRINT N'Creating [Auth].[DF_OrganizationUser_CreatedUtc]...';


GO
ALTER TABLE [Auth].[OrganizationUser]
    ADD CONSTRAINT [DF_OrganizationUser_CreatedUtc] DEFAULT (getutcdate()) FOR [OrganizationUserCreatedUtc];


GO
PRINT N'Creating [Auth].[DF_Permission_IsAllowed]...';


GO
ALTER TABLE [Auth].[Permission]
    ADD CONSTRAINT [DF_Permission_IsAllowed] DEFAULT ((0)) FOR [IsAllowed];


GO
PRINT N'Creating [Billing].[DF__BillingHistory__CreatedUtc]...';


GO
ALTER TABLE [Billing].[BillingHistory]
    ADD CONSTRAINT [DF__BillingHistory__CreatedUtc] DEFAULT (getutcdate()) FOR [BillingHistoryCreatedUtc];


GO
PRINT N'Creating [Billing].[DF__BillingHistory__ModifiedUtc]...';


GO
ALTER TABLE [Billing].[BillingHistory]
    ADD CONSTRAINT [DF__BillingHistory__ModifiedUtc] DEFAULT (getutcdate()) FOR [BillingHistoryModifiedUtc];


GO
PRINT N'Creating [Billing].[DF__Product__IsActive]...';


GO
ALTER TABLE [Billing].[Product]
    ADD CONSTRAINT [DF__Product__IsActive] DEFAULT ((1)) FOR [IsActive];


GO
PRINT N'Creating [Billing].[DF__Sku__BillingFrequency]...';


GO
ALTER TABLE [Billing].[Sku]
    ADD CONSTRAINT [DF__Sku__BillingFrequency] DEFAULT ((1)) FOR [BillingFrequency];


GO
PRINT N'Creating [Billing].[DF__Sku__BlockSize]...';


GO
ALTER TABLE [Billing].[Sku]
    ADD CONSTRAINT [DF__Sku__BlockSize] DEFAULT ((1)) FOR [BlockSize];


GO
PRINT N'Creating [Billing].[DF__Sku__IsActive]...';


GO
ALTER TABLE [Billing].[Sku]
    ADD CONSTRAINT [DF__Sku__IsActive] DEFAULT ((1)) FOR [IsActive];


GO
PRINT N'Creating [Billing].[DF__Sku__UserLimit]...';


GO
ALTER TABLE [Billing].[Sku]
    ADD CONSTRAINT [DF__Sku__UserLimit] DEFAULT ((0)) FOR [UserLimit];


GO
PRINT N'Creating [Billing].[DF_StripeOrganizationCustomer_CreatedUtc]...';


GO
ALTER TABLE [Billing].[StripeOrganizationCustomer]
    ADD CONSTRAINT [DF_StripeOrganizationCustomer_CreatedUtc] DEFAULT (getutcdate()) FOR [StripeOrganizationCustomerCreatedUtc];


GO
PRINT N'Creating [Billing].[DF_StripeOrganizationCustomer_ModifiedUtc]...';


GO
ALTER TABLE [Billing].[StripeOrganizationCustomer]
    ADD CONSTRAINT [DF_StripeOrganizationCustomer_ModifiedUtc] DEFAULT (getutcdate()) FOR [StripeOrganizationCustomerModifiedUtc];


GO
PRINT N'Creating [Crm].[DF_Customer_CreatedUtc]...';


GO
ALTER TABLE [Crm].[Customer]
    ADD CONSTRAINT [DF_Customer_CreatedUtc] DEFAULT (getutcdate()) FOR [CustomerCreatedUtc];


GO
PRINT N'Creating [Crm].[DF_Customer_IsActive]...';


GO
ALTER TABLE [Crm].[Customer]
    ADD CONSTRAINT [DF_Customer_IsActive] DEFAULT ((1)) FOR [IsActive];


GO
PRINT N'Creating [Expense].[DF_ExpenseItem_CreatedUtc]...';


GO
ALTER TABLE [Expense].[ExpenseItem]
    ADD CONSTRAINT [DF_ExpenseItem_CreatedUtc] DEFAULT (getutcdate()) FOR [ExpenseItemCreatedUtc];


GO
PRINT N'Creating [Expense].[DF_ExpenseItem_IsBillableToCustomer]...';


GO
ALTER TABLE [Expense].[ExpenseItem]
    ADD CONSTRAINT [DF_ExpenseItem_IsBillableToCustomer] DEFAULT ((0)) FOR [IsBillableToCustomer];


GO
PRINT N'Creating [Expense].[DF_ExpenseItem_ModifiedUtc]...';


GO
ALTER TABLE [Expense].[ExpenseItem]
    ADD CONSTRAINT [DF_ExpenseItem_ModifiedUtc] DEFAULT (getutcdate()) FOR [ExpenseItemModifiedUtc];


GO
PRINT N'Creating [Expense].[DF_ExpenseReport_CreatedUtc]...';


GO
ALTER TABLE [Expense].[ExpenseReport]
    ADD CONSTRAINT [DF_ExpenseReport_CreatedUtc] DEFAULT (getutcdate()) FOR [ExpenseReportCreatedUtc];


GO
PRINT N'Creating [Expense].[DF_ExpenseReport_ModifiedUtc]...';


GO
ALTER TABLE [Expense].[ExpenseReport]
    ADD CONSTRAINT [DF_ExpenseReport_ModifiedUtc] DEFAULT (getutcdate()) FOR [ExpenseReportModifiedUtc];


GO
PRINT N'Creating [Expense].[DF_ExpenseReport_ReportStatus]...';


GO
ALTER TABLE [Expense].[ExpenseReport]
    ADD CONSTRAINT [DF_ExpenseReport_ReportStatus] DEFAULT ((1)) FOR [ReportStatus];


GO
PRINT N'Creating [Finance].[DF_Account_IsActive]...';


GO
ALTER TABLE [Finance].[Account]
    ADD CONSTRAINT [DF_Account_IsActive] DEFAULT ((1)) FOR [IsActive];


GO
PRINT N'Creating [Hrm].[DF_Holiday_OrganizationId]...';


GO
ALTER TABLE [Hrm].[Holiday]
    ADD CONSTRAINT [DF_Holiday_OrganizationId] DEFAULT ((0)) FOR [OrganizationId];


GO
PRINT N'Creating [Pjm].[DF_ProjectUser_CreatedUtc]...';


GO
ALTER TABLE [Pjm].[ProjectUser]
    ADD CONSTRAINT [DF_ProjectUser_CreatedUtc] DEFAULT getutcdate() FOR [ProjectUserCreatedUtc];


GO
PRINT N'Creating [Auth].[FK_Logging_User]...';


GO
ALTER TABLE [Auth].[Logging] WITH NOCHECK
    ADD CONSTRAINT [FK_Logging_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId]);


GO
PRINT N'Creating [Auth].[FK_OrganizationUser_User]...';


GO
ALTER TABLE [Auth].[OrganizationUser] WITH NOCHECK
    ADD CONSTRAINT [FK_OrganizationUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId]);


GO
PRINT N'Creating [Auth].[FK_User_Language]...';


GO
ALTER TABLE [Auth].[User] WITH NOCHECK
    ADD CONSTRAINT [FK_User_Language] FOREIGN KEY ([PreferredLanguageId]) REFERENCES [Lookup].[Language] ([Id]) ON DELETE SET DEFAULT;


GO
PRINT N'Creating [Auth].[FK_User_Organization]...';


GO
ALTER TABLE [Auth].[User] WITH NOCHECK
    ADD CONSTRAINT [FK_User_Organization] FOREIGN KEY ([LastUsedOrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]);


GO
PRINT N'Creating [Billing].[FK_BillingHistory_User]...';


GO
ALTER TABLE [Billing].[BillingHistory] WITH NOCHECK
    ADD CONSTRAINT [FK_BillingHistory_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId]);


GO
PRINT N'Creating [Billing].[FK_SubscriptionUser_User]...';


GO
ALTER TABLE [Billing].[SubscriptionUser] WITH NOCHECK
    ADD CONSTRAINT [FK_SubscriptionUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId]) ON DELETE CASCADE;


GO
PRINT N'Creating [Expense].[FK_ExpenseReport_User]...';


GO
ALTER TABLE [Expense].[ExpenseReport] WITH NOCHECK
    ADD CONSTRAINT [FK_ExpenseReport_User] FOREIGN KEY ([SubmittedById]) REFERENCES [Auth].[User] ([UserId]);


GO
PRINT N'Creating [Pjm].[FK_ProjectUser_User]...';


GO
ALTER TABLE [Pjm].[ProjectUser] WITH NOCHECK
    ADD CONSTRAINT [FK_ProjectUser_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId]);


GO
PRINT N'Creating [TimeTracker].[FK_TimeEntry_User]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] WITH NOCHECK
    ADD CONSTRAINT [FK_TimeEntry_User] FOREIGN KEY ([UserId]) REFERENCES [Auth].[User] ([UserId]);


GO
PRINT N'Creating [Pjm].[FK_Project_Customer]...';


GO
ALTER TABLE [Pjm].[Project] WITH NOCHECK
    ADD CONSTRAINT [FK_Project_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Crm].[Customer] ([CustomerId]);


GO
PRINT N'Creating [Pjm].[FK_ProjectUser_Project]...';


GO
ALTER TABLE [Pjm].[ProjectUser] WITH NOCHECK
    ADD CONSTRAINT [FK_ProjectUser_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Pjm].[Project] ([ProjectId]);


GO
PRINT N'Creating [TimeTracker].[FK_TimeEntry_Project]...';


GO
ALTER TABLE [TimeTracker].[TimeEntry] WITH NOCHECK
    ADD CONSTRAINT [FK_TimeEntry_Project] FOREIGN KEY ([ProjectId]) REFERENCES [Pjm].[Project] ([ProjectId]);


GO
PRINT N'Creating [TimeTracker].[FK_Settings_Organization]...';


GO
ALTER TABLE [TimeTracker].[Setting] WITH NOCHECK
    ADD CONSTRAINT [FK_Settings_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]);


GO
PRINT N'Creating [Billing].[FK_StripeCustomerSubscriptionPlan_Organization]...';


GO
ALTER TABLE [Billing].[StripeCustomerSubscriptionPlan] WITH NOCHECK
    ADD CONSTRAINT [FK_StripeCustomerSubscriptionPlan_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]);


GO
PRINT N'Creating [Billing].[FK_StripeCustomerSubscriptionPlan_Product]...';


GO
ALTER TABLE [Billing].[StripeCustomerSubscriptionPlan] WITH NOCHECK
    ADD CONSTRAINT [FK_StripeCustomerSubscriptionPlan_Product] FOREIGN KEY ([ProductId]) REFERENCES [Billing].[Product] ([ProductId]);


GO
PRINT N'Creating [Lookup].[FK_OrganizationLocation_Address]...';


GO
ALTER TABLE [Lookup].[OrganizationLocation] WITH NOCHECK
    ADD CONSTRAINT [FK_OrganizationLocation_Address] FOREIGN KEY ([AddressId]) REFERENCES [Lookup].[Address] ([AddressId]);


GO
PRINT N'Creating [Lookup].[FK_OrganizationLocation_Organization]...';


GO
ALTER TABLE [Lookup].[OrganizationLocation] WITH NOCHECK
    ADD CONSTRAINT [FK_OrganizationLocation_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]);


GO
PRINT N'Creating [TimeTracker].[trg_update_TimeEntry]...';


GO

CREATE TRIGGER [TimeTracker].trg_update_TimeEntry ON [TimeTracker].[TimeEntry] FOR UPDATE AS
BEGIN
	UPDATE [TimeTracker].[TimeEntry] SET [TimeEntryModifiedUtc] = CONVERT(DATETIME2(0), GETUtcDATE()) 
	WHERE [TimeEntryId] IN (SELECT [TimeEntryId] FROM [deleted]);
END
GO
PRINT N'Creating [Billing].[trg_update_CustomerSubscriptionPlan]...';


GO
CREATE TRIGGER [Billing].trg_update_CustomerSubscriptionPlan ON [Billing].[StripeCustomerSubscriptionPlan] FOR UPDATE AS
BEGIN
    UPDATE [Billing].[StripeCustomerSubscriptionPlan] SET [StripeCustomerSubscriptionPlanModifiedUtc] = CONVERT(DATETIME2(0), GETUtcDATE()) FROM [Billing].[StripeCustomerSubscriptionPlan] INNER JOIN [deleted] [d] ON [StripeCustomerSubscriptionPlan].[StripeTokenSubId] = [d].[StripeTokenSubId];
END
GO
PRINT N'Creating [Billing].[trg_update_BillingHistory]...';


GO
CREATE TRIGGER [Billing].trg_update_BillingHistory ON [Billing].[BillingHistory] FOR UPDATE AS
BEGIN
    UPDATE [Billing].[BillingHistory] SET [BillingHistoryModifiedUtc] = CONVERT(DATETIME2(0), GETUtcDATE()) FROM [Billing].[BillingHistory] INNER JOIN [deleted] [d] ON [BillingHistory].[Date] = [d].[Date];
END
GO
PRINT N'Creating [Billing].[trg_update_OrganizationCustomer]...';


GO
CREATE TRIGGER [Billing].trg_update_OrganizationCustomer ON [Billing].[StripeOrganizationCustomer] FOR UPDATE AS
BEGIN
	UPDATE [Billing].[StripeOrganizationCustomer] SET [StripeOrganizationCustomerModifiedUtc] = CONVERT(DATETIME2(0), GETUtcDATE()) FROM [Billing].[StripeOrganizationCustomer] INNER JOIN [deleted] [d] ON [StripeOrganizationCustomer].[StripeTokenCustId] = [d].[StripeTokenCustId];
END
GO
PRINT N'Creating [Expense].[trg_update_item_ModifiedUtc]...';


GO

CREATE TRIGGER [Expense].trg_update_item_ModifiedUtc ON [Expense].[ExpenseItem] FOR UPDATE AS
BEGIN
    UPDATE [Expense].[ExpenseItem] SET [ExpenseItemModifiedUtc] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Expense].[ExpenseItem] INNER JOIN [deleted] [d] ON [ExpenseItem].[ExpenseItemId] = [d].[ExpenseItemId]
END
GO
PRINT N'Creating [Expense].[trg_update_report_ModifiedUtc]...';


GO

CREATE TRIGGER [Expense].trg_update_report_ModifiedUtc ON [Expense].[ExpenseReport] FOR UPDATE AS
BEGIN
    UPDATE [Expense].[ExpenseReport] SET [ExpenseReportModifiedUtc] = CONVERT(DATETIME2(0), GETUTCDATE()) FROM [Expense].[ExpenseReport] INNER JOIN [deleted] [d] ON [ExpenseReport].[ExpenseReportId] = [d].[ExpenseReportId]
END
GO
PRINT N'Creating [Billing].[GetNumberSubscriptionUsers]...';


GO
CREATE FUNCTION [Billing].[GetNumberSubscriptionUsers](@subscriptoinID Int)
RETURNS INT
AS
BEGIN
	Return (SELECT COUNT(*) FROM [Billing].[SubscriptionUser] [s] Where [s].SubscriptionId = @subscriptoinID); 
END
GO
PRINT N'Starting rebuilding table [Billing].[Subscription]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Billing].[tmp_ms_xx_Subscription] (
    [SubscriptionId]          INT           IDENTITY (113969, 7) NOT NULL,
    [OrganizationId]          INT           NOT NULL,
    [SkuId]                   INT           NOT NULL,
    [SubscriptionName]        NVARCHAR (64) NULL,
    [NumberOfUsers]           AS            [Billing].[GetNumberSubscriptionUsers]([SubscriptionId]),
    [IsActive]                BIT           CONSTRAINT [DF_Subscription_IsActive] DEFAULT ((1)) NOT NULL,
    [SubscriptionCreatedUtc]  DATETIME2 (0) CONSTRAINT [DF_Subscription_CreatedUtc] DEFAULT (getutcdate()) NOT NULL,
    [SubscriptionModifiedUtc] DATETIME2 (0) CONSTRAINT [DF_Subscription_ModifiedUtc] DEFAULT (getutcdate()) NOT NULL,
    [PromoExpirationDateUtc]  DATETIME2 (0) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Subscription1] PRIMARY KEY NONCLUSTERED ([SubscriptionId] ASC)
);

CREATE CLUSTERED INDEX [tmp_ms_xx_index_IX_Subscription_OrganizationId1]
    ON [Billing].[tmp_ms_xx_Subscription]([OrganizationId] ASC);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Billing].[Subscription])
    BEGIN
        SET IDENTITY_INSERT [Billing].[tmp_ms_xx_Subscription] ON;
        INSERT INTO [Billing].[tmp_ms_xx_Subscription] ([OrganizationId], [SubscriptionId], [SkuId], [SubscriptionName], [IsActive], [SubscriptionCreatedUtc], [SubscriptionModifiedUtc], [PromoExpirationDateUtc])
        SELECT   [OrganizationId],
                 [SubscriptionId],
                 [SkuId],
                 [SubscriptionName],
                 [IsActive],
                 [SubscriptionCreatedUtc],
                 [SubscriptionModifiedUtc],
                 [PromoExpirationDateUtc]
        FROM     [Billing].[Subscription]
        ORDER BY [OrganizationId] ASC;
        SET IDENTITY_INSERT [Billing].[tmp_ms_xx_Subscription] OFF;
    END

DROP TABLE [Billing].[Subscription];

EXECUTE sp_rename N'[Billing].[tmp_ms_xx_Subscription]', N'Subscription';

EXECUTE sp_rename N'[Billing].[Subscription].[tmp_ms_xx_index_IX_Subscription_OrganizationId1]', N'IX_Subscription_OrganizationId', N'INDEX';

EXECUTE sp_rename N'[Billing].[tmp_ms_xx_constraint_PK_Subscription1]', N'PK_Subscription', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [Billing].[Subscription].[IX_Subscription]...';


GO
CREATE NONCLUSTERED INDEX [IX_Subscription]
    ON [Billing].[Subscription]([OrganizationId] ASC, [SkuId] ASC);


GO
PRINT N'Creating [Billing].[FK_Subscription_Organization]...';


GO
ALTER TABLE [Billing].[Subscription] WITH NOCHECK
    ADD CONSTRAINT [FK_Subscription_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Auth].[Organization] ([OrganizationId]);


GO
PRINT N'Creating [Billing].[FK_SubscriptionUser_Subscription]...';


GO
ALTER TABLE [Billing].[SubscriptionUser] WITH NOCHECK
    ADD CONSTRAINT [FK_SubscriptionUser_Subscription] FOREIGN KEY ([SubscriptionId]) REFERENCES [Billing].[Subscription] ([SubscriptionId]);


GO
PRINT N'Creating [Auth].[FK_User_Subscription]...';


GO
ALTER TABLE [Auth].[User] WITH NOCHECK
    ADD CONSTRAINT [FK_User_Subscription] FOREIGN KEY ([LastUsedSubscriptionId]) REFERENCES [Billing].[Subscription] ([SubscriptionId]);


GO
PRINT N'Altering [Auth].[AcceptInvitation]...';


GO
ALTER PROCEDURE [Auth].[AcceptInvitation]
	@InvitationId INT,
	@CallingUserId INT
AS
BEGIN
	SET NOCOUNT ON;

	-- Retrieve the invitation information
	DECLARE @OrganizationId INT;
	DECLARE @OrganizationRole INT;
	DECLARE @Email NVARCHAR(384);
	DECLARE @EmployeeId NVARCHAR(16);
	SELECT
		@OrganizationId = [OrganizationId],
		@OrganizationRole = [OrganizationRoleId],
		@Email = [Email],
		@EmployeeId = [EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	WHERE [Invitation].[InvitationId] = @InvitationId AND [Invitation].[IsActive] = 1

	IF @OrganizationId IS NOT NULL
	BEGIN -- Invitation found

		-- Retrieve invited user
		DECLARE @UserId INT;
		SET @UserId = (
			SELECT [UserId]
			FROM [Auth].[User] WITH (NOLOCK)
			WHERE [User].[Email] = @Email
		)

		IF @UserId IS NOT NULL AND @UserId = @CallingUserId
		BEGIN -- Invited user found and matches calling user id
			BEGIN TRANSACTION

			-- Add user to organization
			IF EXISTS (
				SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
				WHERE [OrganizationUser].[UserId] = @UserId AND [OrganizationUser].[OrganizationId] = @OrganizationId
			)
			BEGIN -- User already in organization
				UPDATE [Auth].[OrganizationUser]
				SET [OrganizationRoleId] = @OrganizationRole,
					[EmployeeId] = @EmployeeId
				WHERE [UserId] = @UserId AND 
					[OrganizationId] = @OrganizationId;
			END
			ELSE
			BEGIN -- User not in organization
				INSERT INTO [Auth].[OrganizationUser]  (
					[UserId], 
					[OrganizationId], 
					[OrganizationRoleId], 
					[EmployeeId]
				)
				VALUES (
					@UserId, 
					@OrganizationId,
					@OrganizationRole, 
					@EmployeeId
				);
			END

			DELETE FROM [Auth].[Invitation]
			WHERE [InvitationId] = @InvitationId
			
			-- On success, return name of organization and role
			SELECT [Organization].[OrganizationName]
			FROM [Auth].[Organization]
			WHERE [Organization].[OrganizationId] = @OrganizationId

			SELECT [OrganizationRole].[OrganizationRoleName]
			FROM [Auth].[OrganizationRole]
			WHERE [OrganizationRole].[OrganizationRoleId] = @OrganizationRole

			COMMIT
		END
	END
END
GO
PRINT N'Altering [Auth].[CreateUserInvitation]...';


GO
ALTER PROCEDURE [Auth].[CreateUserInvitation]
	@Email NVARCHAR(384),
	@FirstName NVARCHAR(40),
	@LastName NVARCHAR(40),
	@DateOfBirth NVARCHAR(40),
	@OrganizationId INT,
	@AccessCode VARCHAR(50),
	@OrganizationRole INT,
	@retId INT OUTPUT,
	@EmployeeId NVARCHAR(16)
AS

BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Auth].[Invitation] 
		([Email], 
		[FirstName], 
		[LastName], 
		[DateOfBirth], 
		[OrganizationId], 
		[AccessCode], 
		[IsActive], 
		[OrganizationRoleId], 
		[EmployeeId])
	VALUES 
		(@Email, 
		@FirstName, 
		@LastName, 
		@DateOfBirth, 
		@OrganizationId, 
		@AccessCode, 
		1, 
		@OrganizationRole, 
		@EmployeeId);

	SET @retId = SCOPE_IDENTITY();

	SELECT SCOPE_IDENTITY();
END
GO
PRINT N'Altering [Auth].[GetAddMemberInfo]...';


GO
ALTER PROCEDURE [Auth].[GetAddMemberInfo]
	@OrganizationId INT
AS
	SELECT TOP 1
		[EmployeeId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK)
	WHERE [OrganizationUser].[OrganizationId] = @OrganizationId
	ORDER BY [EmployeeId] DESC

	SELECT	[Product].[ProductId],
		[Product].[ProductName] AS [ProductName],
		[Subscription].[SubscriptionId],
		[Organization].[OrganizationId],
		[Subscription].[SkuId],
		[Subscription].[NumberOfUsers],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Sku].[SkuName] AS [SkuName]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	LEFT JOIN [Billing].[Sku]			WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
	LEFT JOIN [Billing].[Product]		WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
	WHERE [Subscription].[OrganizationId] = @OrganizationId
	AND [Subscription].[IsActive] = 1
	ORDER BY [Product].[ProductName]

	SELECT 
		[ProductRole].[ProductRoleName],
		[ProductRole].[ProductRoleId],
		[ProductRole].[ProductId]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	RIGHT JOIN [Auth].[ProductRole]  WITH (NOLOCK) ON [ProductRole].[ProductId] = [Sku].[ProductId]
	WHERE [Subscription].[OrganizationId] = @OrganizationId AND [Subscription].[IsActive] = 1

	SELECT	[Project].[ProjectId],
		[Project].[CustomerId],
		[Customer].[OrganizationId],
		[Project].[ProjectCreatedUtc],
		[Project].[ProjectName] AS [ProjectName],
		[Project].[IsActive],
		[ProjectOrgId],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Customer].[CustomerName] AS [CustomerName],
		[Customer].[CustomerOrgId],
		[Customer].[IsActive] AS [IsCustomerActive],
		[Project].[IsHourly] AS [IsHourly]
	FROM (
		[Auth].[Organization]	WITH (NOLOCK) 
		JOIN [Crm].[Customer]	WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @OrganizationId)
		JOIN [Pjm].[Project]	WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
	)
	
	WHERE [Customer].[IsActive] >= 1
		AND [Project].[IsActive] >= 1

	ORDER BY [Project].[ProjectName]

	SELECT TOP 1
		[EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1
	ORDER BY [EmployeeId] DESC
GO
PRINT N'Altering [Auth].[GetOrgManagementInfo]...';


GO
ALTER PROCEDURE [Auth].[GetOrgManagementInfo]
	@OrganizationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[OrganizationId],
		[Organization].[OrganizationName],
		[SiteUrl], 
		[Address].[Address1] AS 'Address',
		[Address].[City], 
		[State].[StateName] AS 'State', 
		[Country].[CountryName] AS 'Country', 
		[Address].[PostalCode], 
		[PhoneNumber], 
		[FaxNumber], 
		[Subdomain],
		[OrganizationCreatedUtc]

	FROM [Auth].[Organization] WITH (NOLOCK)
		LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
		LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
		LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE OrganizationId = @OrganizationId

	SELECT [OU].[OrganizationId],
	    [OU].[UserId],
		[OU].[OrganizationRoleId],
		[O].[OrganizationName] AS [OrganizationName],
		[OU].[EmployeeId],
		[U].[Email],
		[U].[FirstName],
		[U].[LastName]
    FROM [Auth].[OrganizationUser]	AS [OU]
	WITH (NOLOCK)
    INNER JOIN [Auth].[User]		AS [U] WITH (NOLOCK) 
		ON [U].[UserId] = [OU].[UserId]
	INNER JOIN [Auth].[Organization] AS [O] WITH (NOLOCK)
		ON [O].[OrganizationId] = [OU].[OrganizationId]
    WHERE [OU].[OrganizationId] = @OrganizationId
	ORDER BY [U].[LastName]

	SELECT	[Product].[ProductId],
		[Product].[ProductName] AS [ProductName],
		[Product].[AreaUrl],
		[Subscription].[SubscriptionId],
		[Organization].[OrganizationId],
		[Subscription].[SkuId],
		[Subscription].[NumberOfUsers],
		[Subscription].[SubscriptionName],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Sku].[SkuName] AS [SkuName]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	LEFT JOIN [Billing].[Sku]			WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
	LEFT JOIN [Billing].[Product]		WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
	WHERE [Subscription].[OrganizationId] = @OrganizationId
	AND [Subscription].[IsActive] = 1
	ORDER BY [Product].[ProductName]

	SELECT 
		[InvitationId],
		[Email],
		[FirstName],
		[LastName], 
		[DateOfBirth], 
		[OrganizationId], 
		[AccessCode], 
		[Invitation].[OrganizationRoleId],
		[OrganizationRoleName] AS [OrganizationRoleName],
		[EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	LEFT JOIN [Auth].[OrganizationRole] WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [Invitation].[OrganizationRoleId]
	WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1

	SELECT [StripeTokenCustId]
	FROM [Billing].[StripeOrganizationCustomer] WITH (NOLOCK) 
	WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1

	SELECT
		[Product].[ProductId],
		[Sku].[SkuName],
		[Product].[Description],
		[Product].[AreaUrl]
	FROM [Billing].[Product] WITH (NOLOCK) 
	INNER JOIN [Billing].[Sku] WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
	RIGHT JOIN [Billing].[Subscription] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	WHERE [Product].[IsActive] = 1 AND [Subscription].[IsActive] = 1 AND [Subscription].OrganizationId = @OrganizationId
	ORDER BY [Product].[ProductName]
END
GO
PRINT N'Altering [Auth].[GetUserInvitationsByOrgId]...';


GO
ALTER PROCEDURE [Auth].[GetUserInvitationsByOrgId]
	@OrganizationId INT
AS
	SET NOCOUNT ON;
	SELECT 
		[InvitationId],
		[Email],
		[FirstName],
		[LastName], 
		[DateOfBirth], 
		[OrganizationId], 
		[AccessCode], 
		[Invitation].[OrganizationRoleId],
		[OrganizationRoleName] AS [OrganizationRoleName],
		[EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	LEFT JOIN [Auth].[OrganizationRole] WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [Invitation].[OrganizationRoleId]
	WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1
GO
PRINT N'Altering [Auth].[GetUserOrgsAndInvitationInfo]...';


GO
ALTER PROCEDURE [Auth].[GetUserOrgsAndInvitationInfo]
	@userId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [User].[UserId],
			[User].[FirstName],
			[User].[LastName],
			[User].[DateOfBirth],
			[User].[Email],
			[User].[PhoneNumber],
			[User].[LastUsedSubscriptionId],
			[User].[LastUsedOrganizationId],
			[PreferredLanguageId]
	FROM [Auth].[User]
	WITH (NOLOCK)
	LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [User].[AddressId]
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [UserId] = @userId;

	SELECT [Auth].[Organization].[OrganizationId],
		   [Organization].[OrganizationName],
		   [SiteUrl],
		   [Address1] AS 'Address',
		   [City],
		   [Country].[CountryName] AS 'Country',
		   [State].[StateName] AS 'State',
		   [PostalCode],
		   [PhoneNumber],
		   [FaxNumber],
		   [Subdomain],
		   [Organization].[OrganizationCreatedUtc]
	FROM [Auth].[Organization] WITH (NOLOCK)
	RIGHT JOIN [Auth].[OrganizationUser]	WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
	LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [OrganizationUser].[UserId] = @userId 
		  AND [Auth].[Organization].[IsActive] = 1
	ORDER BY [OrganizationUser].[OrganizationRoleId] DESC, [Organization].[OrganizationName]

	SELECT 
		[InvitationId], 
		[Invitation].[Email], 
		[Invitation].[FirstName], 
		[Invitation].[LastName], 
		[Invitation].[DateOfBirth], 
		[Invitation].[OrganizationId],
		[Organization].[OrganizationName] AS 'OrganizationName',
		[AccessCode], 
		[OrganizationRoleId],
		[EmployeeId] 
	FROM [Auth].[User] WITH (NOLOCK)
	LEFT JOIN [Auth].[Invitation] WITH (NOLOCK) ON [User].[Email] = [Invitation].[Email]
	LEFT JOIN [Auth].[Organization] WITH (NOLOCK) ON [Invitation].[OrganizationId] = [Organization].[OrganizationId]
	WHERE [User].[UserId] = @userId AND [Invitation].[IsActive] = 1

	DECLARE @AddressId INT
	SET @AddressId = (SELECT m.AddressId
				FROM [Auth].[User] AS m
				WHERE [UserId] = @userId)

	SELECT [Address].[Address1],
		   [Address].[City],
		   [State].[StateName] AS 'State',
		   [Country].[CountryName] AS 'CountryId',
		   [Address].[PostalCode]
	FROM [Lookup].[Address] AS [Address] WITH (NOLOCK)
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [AddressId] = @AddressId
END
GO
PRINT N'Altering [Auth].[InviteUser]...';


GO
ALTER PROCEDURE [Auth].[InviteUser]
	@UserId INT,
	@Email NVARCHAR(384),
	@FirstName NVARCHAR(40),
	@LastName NVARCHAR(40),
	@OrganizationId INT,
	@AccessCode VARCHAR(50),
	@OrganizationRole INT,
	@retId INT OUTPUT,
	@EmployeeId NVARCHAR(16),
	@SubscriptionId INT,
	@SubRoleId INT
AS

BEGIN
	SET NOCOUNT ON;
	IF EXISTS (
		SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
		INNER JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
		WHERE [Email] = @Email AND [OrganizationId] = @OrganizationId
	)
	BEGIN
		SELECT -1 --Indicates the user is already in the organization
	END
	ELSE
	BEGIN
		-- Check for existing employee id
		IF EXISTS (
			SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrganizationId AND [EmployeeId] = @EmployeeId
		) OR EXISTS (
			SELECT * FROM [Auth].[Invitation] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1 AND [EmployeeId] = @EmployeeId
		)
		BEGIN
			SELECT -2 -- Indicates employee id already taken
		END
		ELSE
		BEGIN
			INSERT INTO [Auth].[Invitation] 
				([Email], 
				[FirstName], 
				[LastName], 
				[OrganizationId], 
				[AccessCode], 
				[IsActive], 
				[OrganizationRoleId],
				[EmployeeId],
				[DateOfBirth])
			VALUES 
				(@Email, 
				@FirstName, 
				@LastName, 
				@OrganizationId, 
				@AccessCode, 
				1, 
				@OrganizationRole, 
				@EmployeeId,
				'1755-01-01');

			-- Return invitation id
			SELECT SCOPE_IDENTITY()

			-- Return first and last names of inviting user
			SELECT [FirstName] FROM [Auth].[User] WITH (NOLOCK) WHERE [UserId] = @UserId
			SELECT [LastName] FROM [Auth].[User] WITH (NOLOCK) WHERE [UserId] = @UserId
		END
	END
END
GO
PRINT N'Altering [Auth].[UpdateMember]...';


GO
ALTER PROCEDURE [Auth].[UpdateMember]
	@UserId INT,
	@OrgId INT,
	@EmployeeId NVARCHAR(100),
	@EmployeeRoleId INT,
	@IsInvited BIT,
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR (100)
AS
BEGIN
	IF EXISTS (
			SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrgId AND [EmployeeId] = @EmployeeId AND [UserId] != @UserId
		) OR EXISTS (
			SELECT * FROM [Auth].[Invitation] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrgId AND [IsActive] = 1 AND [EmployeeId] = @EmployeeId AND [InvitationId] != @UserId
		)
	BEGIN
		IF @IsInvited = 0
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[OrganizationUser]
			SET [OrganizationRoleId] = @EmployeeRoleId
			WHERE [UserId] = @UserId AND [OrganizationId] = @OrgId;
		END
		ELSE
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[Invitation]
			SET [OrganizationRoleId] = @EmployeeRoleId,
				[FirstName] = @FirstName,
				[LastName] = @LastName
			WHERE [InvitationId] = @UserId AND [OrganizationId] = @OrgId;
		END
		SELECT 1;
	END
	ELSE
	BEGIN
		IF @IsInvited = 0
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[OrganizationUser]
			SET [EmployeeId] = @EmployeeId,
				[OrganizationRoleId] = @EmployeeRoleId
			WHERE [UserId] = @UserId AND [OrganizationId] = @OrgId;
		END
		ELSE
		BEGIN
			SET NOCOUNT ON;
			UPDATE [Auth].[Invitation]
			SET [EmployeeId] = @EmployeeId,
				[OrganizationRoleId] = @EmployeeRoleId,
				[FirstName] = @FirstName,
				[LastName] = @LastName
			WHERE [InvitationId] = @UserId AND [OrganizationId] = @OrgId;
		END
		SELECT 2;
	END
END
GO
PRINT N'Altering [Auth].[GetOrgAndSubRoles]...';


GO
ALTER PROCEDURE [Auth].[GetOrgAndSubRoles]
	@OrganizationId INT
AS
	SELECT
		[User].[FirstName],
		[User].[LastName],
		[User].[UserId],
		[OrganizationUser].[OrganizationRoleId],
		[OrganizationRole].[OrganizationRoleName],
		[User].[Email],
		[SubscriptionUser].[ProductRoleId], 
		[SubscriptionUser].[SubscriptionId]
	FROM [Auth].[User]
	WITH (NOLOCK)
	INNER JOIN [Auth].[OrganizationUser]	WITH (NOLOCK) ON [OrganizationUser].[UserId] = [User].[UserId]
	INNER JOIN [Auth].[OrganizationRole]				WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [OrganizationUser].[OrganizationRoleId]
	LEFT JOIN [Billing].[Subscription]		WITH (NOLOCK) ON [Subscription].[OrganizationId] = @OrganizationId
	LEFT JOIN [Billing].[SubscriptionUser] WITH (NOLOCK) 
											ON [SubscriptionUser].[UserId] = [User].[UserId]
											AND [SubscriptionUser].[SubscriptionId] = [Subscription].[SubscriptionId]
	WHERE [OrganizationUser].[OrganizationId] = @OrganizationId
	ORDER BY [User].[LastName]

	SELECT 
		[Subscription].[SubscriptionId],
		[Sku].[ProductId],
		[Product].[ProductName] AS 'ProductName'
	FROM [Billing].[Subscription] WITH (NOLOCK)
	LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	LEFT JOIN [Billing].[Product] WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
	WHERE [OrganizationId] = @OrganizationId AND [Subscription].[IsActive] = 1
GO
PRINT N'Altering [Auth].[GetOrganizationsByUserId]...';


GO
ALTER PROCEDURE [Auth].[GetOrganizationsByUserId]
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;
SELECT [Auth].[Organization].[OrganizationId]
      ,[Organization].[OrganizationName]
      ,[SiteUrl]
      ,[Address1] AS 'Address'
      ,[City]
      ,[State].[StateName] AS 'State'
      ,[Country].[CountryName] AS 'Country'
      ,[PostalCode]
      ,[PhoneNumber]
	  ,[FaxNumber]
      ,[Organization].[OrganizationCreatedUtc]
FROM [Auth].[Organization] WITH (NOLOCK)
RIGHT JOIN [Auth].[OrganizationUser]	WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
JOIN [Lookup].[Address]					WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
LEFT JOIN [Lookup].[Country]			WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
LEFT JOIN [Lookup].[State]				WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
WHERE [OrganizationUser].[UserId] = @UserId 
      AND [Auth].[Organization].[IsActive] = 1
ORDER BY [OrganizationUser].[OrganizationRoleId] DESC, [Organization].[OrganizationName]
END
GO
PRINT N'Altering [Auth].[GetOrgUserList]...';


GO
ALTER PROCEDURE [Auth].[GetOrgUserList]
	@OrganizationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [OU].[OrganizationId],
	       [OU].[UserId],
		   [OU].[OrganizationRoleId],
		   [O].[OrganizationName] AS [OrganizationName],
		   [OU].[EmployeeId],
		   [U].[Email]
    FROM [Auth].[OrganizationUser]	AS [OU]
	WITH (NOLOCK)
    INNER JOIN [Auth].[User]		AS [U] WITH (NOLOCK) 
		ON [U].[UserId] = [OU].[UserId]
	INNER JOIN [Auth].[Organization] AS [O] WITH (NOLOCK)
		ON [O].[OrganizationId] = [OU].[OrganizationId]
    WHERE [OU].[OrganizationId] = @OrganizationId
	ORDER BY [U].[LastName]
END
GO
PRINT N'Altering [Auth].[GetOrgUserRole]...';


GO
ALTER PROCEDURE [Auth].[GetOrgUserRole]
	@OrganizationId INT,
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[OrganizationRole].[OrganizationRoleId], 
		[OrganizationRole].[OrganizationRoleName]
	FROM [Auth].[OrganizationUser]
	WITH (NOLOCK)
	INNER JOIN [Auth].[OrganizationRole] WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [OrganizationUser].[OrganizationRoleId]
	JOIN [Auth].[User]			WITH (NOLOCK) ON [User].[UserId] = @UserId
	WHERE [OrganizationUser].[OrganizationId] = @OrganizationId
		AND [OrganizationUser].[UserId] = @UserId
END
GO
PRINT N'Altering [Auth].[GetOrgWithCountriesAndEmployeeId]...';


GO
ALTER PROCEDURE [Auth].[GetOrgWithCountriesAndEmployeeId]
	@OrganizationId int,
	@UserId int
AS
	SELECT 
		[OrganizationId],
		[Organization].[OrganizationName],
		[SiteUrl], 
		[Organization].[AddressId],
		[Address].[Address1] AS 'Address',
		[Address].[City], 
		[State].[StateName] AS 'State', 
		[Country].[CountryName] AS 'Country', 
		[Address].[PostalCode], 
		[PhoneNumber], 
		[FaxNumber], 
		[Subdomain],
		[OrganizationCreatedUtc]

	FROM [Auth].[Organization] WITH (NOLOCK)
		LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
		LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
		LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE OrganizationId = @OrganizationId

	SELECT [CountryName] FROM [Lookup].[Country] WITH (NOLOCK)

	SELECT [EmployeeId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK)
	WHERE [OrganizationUser].[OrganizationId] = @OrganizationId AND [OrganizationUser].[UserId] = @UserId
GO
PRINT N'Altering [Auth].[GetRolesAndPermissions]...';


GO
ALTER PROCEDURE [Auth].[GetRolesAndPermissions]
	@OrgId INT
AS 
BEGIN
	SET NOCOUNT ON;

	SELECT
		[User].[FirstName],
		[User].[LastName],
		[User].[UserId],
		[OrganizationUser].[OrganizationRoleId],
		[OrganizationRole].[OrganizationRoleName],
		[User].[Email],
		[SubscriptionUser].[ProductRoleId], 
		[SubscriptionUser].[SubscriptionId]
	FROM [Auth].[User]
	WITH (NOLOCK)
	INNER JOIN [Auth].[OrganizationUser]	WITH (NOLOCK) ON [OrganizationUser].[UserId] = [User].[UserId]
	INNER JOIN [Auth].[OrganizationRole]				WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [OrganizationUser].[OrganizationRoleId]
	LEFT JOIN [Billing].[Subscription]		WITH (NOLOCK) ON [Subscription].[OrganizationId] = @OrgId
	LEFT JOIN [Billing].[SubscriptionUser] WITH (NOLOCK) 
											ON [SubscriptionUser].[UserId] = [User].[UserId]
											AND [SubscriptionUser].[SubscriptionId] = [Subscription].[SubscriptionId]
	WHERE [OrganizationUser].[OrganizationId] = @OrgId
	ORDER BY [User].[LastName]
END
GO
PRINT N'Altering [Auth].[GetUserContextInfo]...';


GO
ALTER PROCEDURE [Auth].[GetUserContextInfo]
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [U].[UserId],
		   [U].[FirstName],
		   [U].[LastName],
		   [U].[Email],
		   [U].[LastUsedSubscriptionId],
		   [U].[LastUsedOrganizationId],
		   [U].[PreferredLanguageId],
		   [O].[OrganizationId],
		   [O].[OrganizationName] AS 'OrganizationName',
		   [OU].[OrganizationRoleId],
		   [SUB].[SubscriptionId],
		   [SUB].[SubscriptionName],
		   [SUB].[ProductId],
		   [SUB].[ProductName],
		   [SUB].[SkuId],
		   [SUB].[ProductRoleId],
		   [SUB].[AreaUrl]
	FROM [Auth].[User] AS [U] WITH (NOLOCK)
		LEFT JOIN [Auth].[OrganizationUser] AS [OU] WITH (NOLOCK) ON [U].[UserId] = [OU].[UserId]
		LEFT JOIN [Auth].[Organization] AS [O] WITH (NOLOCK) ON [OU].[OrganizationId] = [O].[OrganizationId]
		LEFT JOIN (
			SELECT	[S].[SubscriptionId],
					[S].[SubscriptionName],
					[PR].[ProductId],
					[P].[ProductName] AS 'ProductName',
					[P].[AreaUrl],
					[PR].[ProductRoleName] AS 'ProductRoleName',
					[S].[SkuId],
					[SU].[ProductRoleId],
					[SU].[UserId],
					[S].[OrganizationId]
			FROM [Billing].[SubscriptionUser] AS [SU] WITH (NOLOCK)
				JOIN [Billing].[Subscription] AS [S] WITH (NOLOCK) ON [SU].[SubscriptionId] = [S].SubscriptionId
				JOIN [Billing].[Sku] AS [SK] WITH (NOLOCK) ON [SK].[SkuId] = [S].[SkuId]
				JOIN [Auth].[ProductRole] AS [PR] WITH (NOLOCK) ON [SU].[ProductRoleId] = [PR].[ProductRoleId] AND [Sk].[ProductId] = [PR].[ProductId]
				LEFT JOIN [Billing].[Product] AS [P] WITH (NOLOCK) ON [PR].[ProductId] = [P].[ProductId]
			WHERE [S].[IsActive] = 1
		) [SUB] ON [SUB].[UserId] = [U].[UserId] AND [SUB].[OrganizationId] = [O].[OrganizationId]
	WHERE [U].[UserId] = @UserId;
END
GO
PRINT N'Altering [Billing].[UpdateSubscription]...';


GO
/*This query will change the Sku for an organization's subscription and will prevent multiple Skus of the same product*/
ALTER PROCEDURE [Billing].[UpdateSubscription]
	@OrganizationId INT,
	@SkuId INT,
	@ProductId INT,/*Leave this null unless you are trying to delete something (unsubscribe)*/
	@SubscriptionName NVARCHAR(50), 
	@retId INT OUTPUT
AS
	SET NOCOUNT ON;
IF(@SkuId = 0)
	BEGIN
		UPDATE [Billing].[Subscription] SET [Subscription].[IsActive] = 0
			 WHERE [OrganizationId] = @OrganizationId
			 AND [SkuId] IN (SELECT [SkuId] FROM [Billing].[Sku]
								WHERE [ProductId] = @ProductId);
		DELETE [Billing].[SubscriptionUser]
			WHERE (SELECT [IsActive] FROM [Subscription]
					WHERE [Subscription].[SubscriptionId] = [SubscriptionUser].[SubscriptionId])
					= 0
		UPDATE [Auth].[User] SET [User].[LastUsedSubscriptionId] = NULL
			WHERE (SELECT [IsActive] FROM [Subscription]
					WHERE [Subscription].[SubscriptionId] = [User].[LastUsedSubscriptionId])
					= 0
		--Delete from [Billing].[Subscription] where OrganizationId=@OrganizationId and SkuId in 
		--	(select SkuId from Billing.Sku where  ProductId=@productId);
		SET @retId = 0;
	END
ELSE
	BEGIN
		--Find existing subscription by given org that has the same SkuId, if found do nothing and return 0
		IF EXISTS (
			SELECT * FROM [Billing].[Subscription] 
			WHERE [SkuId] = @SkuId AND [OrganizationId] = @OrganizationId AND [IsActive] = 1
		)
		BEGIN
			SET @retId = 0;
		END
		ELSE

		--Find existing subscription that has the same ProductId but different SkuId, update it to the new SkuId
		--Because the productRoleId and subscriptionId don't change so no need to update SubscriptionUser table
		UPDATE [Billing].[Subscription] SET [SkuId] = @SkuId, [SubscriptionName] = @SubscriptionName
			WHERE [OrganizationId] = @OrganizationId
			AND [Subscription].[IsActive] = 1
			AND [SkuId] IN (SELECT [SkuId] FROM [Billing].[Sku]
							WHERE [SkuId] != @SkuId
							AND [ProductId] = (SELECT [ProductId] FROM [Billing].[Sku] WHERE [SkuId] = @SkuId)
							AND [OrganizationId] = @OrganizationId);

		--If not exist, create new subscription and add all org members to the new subscription as sub users
		IF(@@ROWCOUNT=0)
			BEGIN
				--Create the new subscription
				INSERT INTO [Billing].[Subscription] ([OrganizationId], [SkuId], [SubscriptionName])
				VALUES (@OrganizationId, @SkuId, @SubscriptionName);
				SET @retId = SCOPE_IDENTITY();		

				DECLARE @OrgMemberTable TABLE (userId INT) 
				DECLARE @UserProductRoleId INT

				--Find the productId of the given sku
				SELECT @ProductId = [ProductId]
				FROM [Billing].[Sku]
				WHERE [SkuId] = @SkuId

				--Find the ProductRoleId of the User role for the given Product
				SELECT @UserProductRoleId = [ProductRoleId]
				FROM [Auth].[ProductRole]
				WHERE ([ProductId] = @ProductId AND [ProductRoleName] = 'User')

				--Insert all members of given org to SubscriptionUser table with User role
				INSERT INTO [Billing].[SubscriptionUser] ([UserId], [SubscriptionId], [ProductRoleId])
				SELECT [UserId], @retId, @UserProductRoleId FROM [Auth].[OrganizationUser] WHERE [OrganizationId] = @OrganizationId;
			END
		ELSE
			SET @retId = 0;
	END
GO
PRINT N'Altering [Pjm].[GetProjectEditInfo]...';


GO
ALTER PROCEDURE [Pjm].[GetProjectEditInfo]
	@ProjectId INT,
	@SubscriptionId INT
AS
	SET NOCOUNT ON;
	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[ProjectCreatedUtc],
			[Project].[ProjectName] AS [ProjectName],
			[Organization].[OrganizationName] AS [OrganizationName],
			[Customer].[CustomerName] AS [CustomerName],
			[Customer].[CustomerOrgId],
			[Project].[isHourly] AS [isHourly],
			[Project].[StartUtc] AS [StartDate],
			[Project].[EndUtc] AS [EndDate],
			[Project].[ProjectOrgId]
			FROM (
		(SELECT [ProjectId], [CustomerId], [ProjectName], [isHourly], [StartUtc], [EndUtc], [IsActive], 
				[ProjectCreatedUtc], [ProjectOrgId] FROM [Pjm].[Project] WITH (NOLOCK) WHERE [ProjectId] = @ProjectId) AS [Project]
			JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
			JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [Customer].[OrganizationId]
	)

	SELECT [ProjectUser].[UserId], [FirstName], [LastName]
	FROM [Pjm].[ProjectUser] WITH (NOLOCK) 
	LEFT JOIN [Pjm].[Project]	WITH (NOLOCK) ON [Project].[ProjectId] = [ProjectUser].[ProjectId]
	LEFT JOIN [Crm].[Customer]	WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
	LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [ProjectUser].[UserId]
	WHERE [Customer].[IsActive] = 1 
		AND [Project].[IsActive] = 1
		AND [ProjectUser].[IsActive] = 1
		AND [ProjectUser].[ProjectId] = @ProjectId
	ORDER BY [User].[LastName]

	SELECT [FirstName], [LastName], [ProductRoleId], [User].[UserId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK) 
	LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
	LEFT JOIN (SELECT [UserId], [ProductRoleId] 
				FROM [Billing].[SubscriptionUser] WITH (NOLOCK) 
				WHERE [SubscriptionId] = @SubscriptionId)
				AS [OnRoles]
				ON [OnRoles].[UserId] = [User].[UserId]
	WHERE [OrganizationId] = (
		SELECT TOP 1
			[OrganizationId]
		FROM [Pjm].[Project] WITH (NOLOCK)
		LEFT JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
		WHERE [ProjectId] = @ProjectId
	) AND [ProductRoleId] IS NOT NULL
	ORDER BY [User].[LastName]
GO
PRINT N'Altering [Pjm].[GetProjectsByUserAndOrganization]...';


GO
ALTER PROCEDURE [Pjm].[GetProjectsByUserAndOrganization]
	@UserId INT,
	@OrgId INT,
	@Activity INT = 1
AS
	SET NOCOUNT ON;
	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[ProjectCreatedUtc],
			[Project].[ProjectName] AS [ProjectName],
			[Project].[IsActive],
			[Project].[StartUtc] AS [StartDate],
			[Project].[EndUtc] AS [EndDate],
			[Project].[IsHourly] AS [PriceType],
			[Organization].[OrganizationName] AS [OrganizationName],
			[Customer].[CustomerName] AS [CustomerName],
			[Customer].[CustomerOrgId],
			[Customer].[IsActive] AS [IsCustomerActive],
			[ProjectUser].[IsActive] AS [IsUserActive],
			[OrganizationRoleId],
			[ProjectOrgId]
FROM (
	(SELECT [OrganizationId], [UserId], [OrganizationRoleId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK) WHERE [UserId] = @UserId AND [OrganizationId] = @OrgId)
	AS [OrganizationUser]
	JOIN [Auth].[Organization]		WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
	JOIN [Crm].[Customer]		WITH (NOLOCK) ON [Customer].[OrganizationId] = [Organization].[OrganizationId]
	JOIN ( [Pjm].[Project]
		JOIN [Pjm].[ProjectUser] WITH (NOLOCK) ON [ProjectUser].[ProjectId] = [Project].[ProjectId]
	)
									ON [Project].[CustomerId] = [Customer].[CustomerId]
									AND [ProjectUser].[UserId] = [OrganizationUser].[UserId]
	
)
WHERE [Customer].[IsActive] >= @Activity
	AND [Project].[IsActive] >= @Activity
	AND [ProjectUser].[IsActive] >= @Activity
	UNION ALL
SELECT	[ProjectId],
		[CustomerId],
		0,
		[ProjectCreatedUtc],
		[ProjectName],
		[IsActive],
		[StartUtc],
		[EndUtc],
		[IsHourly],
		(SELECT [OrganizationName] FROM [Auth].[Organization]  WITH (NOLOCK) WHERE [OrganizationId] = 0),
		(SELECT [CustomerName] FROM [Crm].[Customer]  WITH (NOLOCK) WHERE [CustomerId] = 0),
		NULL,
		0,
		0,
		0,
		[ProjectOrgId]
		FROM [Pjm].[Project]  WITH (NOLOCK) WHERE [ProjectId] = 0
ORDER BY [Project].[ProjectName]
GO
PRINT N'Altering [TimeTracker].[GetReportInfo]...';


GO
ALTER PROCEDURE [TimeTracker].[GetReportInfo]
	@OrgId INT,
	@SubscriptionId INT
AS
	SET NOCOUNT ON
	SELECT [Customer].[CustomerId],
		   [Customer].[CustomerName],
		   [Address1] AS 'Address',
		   [City],
		   [State].[StateName] AS 'State',
		   [Country].[CountryName] AS 'Country',
		   [PostalCode],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CustomerCreatedUtc],
		   [Customer].[CustomerOrgId]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Customer].[OrganizationId] = @OrgId
	ORDER BY [Customer].[CustomerName]

	SELECT	[Project].[ProjectId],
		[Project].[CustomerId],
		[Customer].[OrganizationId],
		[Project].[ProjectCreatedUtc],
		[Project].[ProjectName] AS [ProjectName],
		[Project].[IsActive],
		[ProjectOrgId],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Customer].[CustomerName] AS [CustomerName],
		[Customer].[CustomerOrgId],
		[Customer].[IsActive] AS [IsCustomerActive],
		[Project].[IsHourly] AS [IsHourly]
	FROM [Auth].[Organization] WITH (NOLOCK) 
		JOIN [Crm].[Customer]	WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @OrgId)
		JOIN [Pjm].[Project]		WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]

	SELECT [FirstName], [LastName], [ProductRoleId], [User].[UserId]
	FROM [Auth].[OrganizationUser] WITH (NOLOCK) 
	LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId]
	LEFT JOIN (SELECT [UserId], [ProductRoleId] 
				FROM [Billing].[SubscriptionUser] WITH (NOLOCK) 
				WHERE [SubscriptionId] = @SubscriptionId)
				AS [OnRoles]
				ON [OnRoles].[UserId] = [User].[UserId]
	WHERE [OrganizationId] = @OrgId AND [ProductRoleId] IS NOT NULL
GO
PRINT N'Altering [TimeTracker].[GetTimeEntriesByUserOverDateRange]...';


GO
ALTER PROCEDURE [TimeTracker].[GetTimeEntriesByUserOverDateRange]
	@UserId [Auth].[UserTable] READONLY,
	@OrganizationId INT,
	@StartingDate DATE,
	@EndingDate DATE
AS
	SET NOCOUNT ON;
SELECT DISTINCT [TimeEntryId] 
	,[User].[UserId] AS [UserId]
	,[User].[FirstName] AS [FirstName]
	,[User].[LastName] AS [LastName]
	,[User].[Email]
	,[OrganizationUser].[EmployeeId]
	,[TimeEntry].[ProjectId]
	,[TimeEntry].[PayClassId]
	,[PayClass].[PayClassName] AS [PayClassName]
	,[Date]
	,[Duration]
	,[Description]
FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) 
JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [TimeEntry].[UserId]
JOIN [Hrm].[PayClass] WITH (NOLOCK) ON [PayClass].[PayClassId] = [TimeEntry].[PayClassId]
JOIN [Auth].[OrganizationUser] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId] AND [OrganizationUser].[OrganizationId] = @OrganizationId
WHERE [User].[UserId] IN (SELECT [userId] FROM @UserId)
	AND [Date] >= @StartingDate
	AND [Date] <= @EndingDate
	AND [PayClass].[OrganizationId] = @OrganizationId
ORDER BY [Date] ASC
GO
PRINT N'Altering [TimeTracker].[GetTimeEntriesOverDateRange]...';


GO
ALTER PROCEDURE [TimeTracker].[GetTimeEntriesOverDateRange]
	@OrganizationId INT,
	@StartingDate DATE,
	@EndingDate DATE
AS
	SET NOCOUNT ON;
SELECT DISTINCT [TimeEntryId] 
	,[User].[UserId] AS [UserId]
	,[User].[FirstName] AS [FirstName]
	,[User].[LastName] AS [LastName]
	,[User].[Email]
	,[OrganizationUser].[EmployeeId]
    ,[TimeEntry].[ProjectId]
	,[TimeEntry].[PayClassId]
	,[PayClass].[PayClassName] AS [PayClassName]
    ,[Date]
    ,[Duration]
    ,[Description]
FROM [TimeTracker].[TimeEntry] WITH (NOLOCK)
JOIN [Hrm].[PayClass] WITH (NOLOCK) ON [PayClass].[PayClassId] = [TimeEntry].[PayClassId]
JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [TimeEntry].[UserId]
JOIN [Auth].[OrganizationUser] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId] AND [OrganizationUser].[OrganizationId] = @OrganizationId
WHERE [Date] >= @StartingDate
	AND [Date] <= @EndingDate
	AND [PayClass].[OrganizationId] = @OrganizationId
ORDER BY [Date] ASC
GO
PRINT N'Altering [TimeTracker].[GetTimeEntryIndexInfo]...';


GO
ALTER PROCEDURE [TimeTracker].[GetTimeEntryIndexInfo]
	@OrganizationId INT,
	@UserId INT,
	@ProductId INT,
	@StartingDate DATE,
	@EndingDate DATE
AS
	SET NOCOUNT ON;

	-- Settings is declared as a table here so that the StartOfWeek field can be used in other Select
	-- blocks lower in this same stored procedure, while also letting the settings table itself be returned
	DECLARE @Settings TABLE (
		StartOfWeek INT,
		IsLockDateUsed INT,
		LockDatePeriod VARCHAR(10),
		LockDateQuantity INT
	);
	INSERT INTO @Settings (StartOfWeek, IsLockDateUsed, LockDatePeriod, LockDateQuantity)
	SELECT [StartOfWeek], [IsLockDateUsed], [LockDatePeriod], [LockDateQuantity]
	FROM [TimeTracker].[Setting] 
	WITH (NOLOCK) 
	WHERE [OrganizationId] = @OrganizationId

	-- Starting and Ending date parameters are adjusted if the input is null, using the StartOfWeek from above
	DECLARE @StartOfWeek INT;
	SET @StartOfWeek = (
		SELECT TOP 1
			[StartOfWeek]
		FROM @Settings
	)
	DECLARE @TodayDayOfWeek INT;
	SET @TodayDayOfWeek = ((6 + DATEPART(dw, GETDATE()) + @@DATEFIRST) % 7);

	IF(@StartingDate IS NULL)
	BEGIN
		DECLARE @DaysIntoWeek INT;
		IF (@TodayDayOfWeek < @StartOfWeek)
			SET @DaysIntoWeek = @StartOfWeek - @TodayDayOfWeek - 7;
		ELSE
			SET @DaysIntoWeek = @StartOfWeek - @TodayDayOfWeek;
		SET @StartingDate = DATEADD(dd, @DaysIntoWeek, GETDATE());
	END

	IF(@EndingDate IS NULL)
	BEGIN
		DECLARE @DaysLeftInWeek INT;
		IF (@TodayDayOfWeek < @StartOfWeek)
			SET @DaysLeftInWeek = @StartOfWeek - @TodayDayOfWeek - 1;
		ELSE
			SET @DaysLeftInWeek = @StartOfWeek - @TodayDayOfWeek + 6;
		SET @EndingDate = DATEADD(dd, @DaysLeftInWeek, GETDATE());
	END

	-- Begin select statements

	SELECT * FROM @Settings

	IF(SELECT COUNT(*) FROM [Hrm].[PayClass] WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [PayClassId], [PayClassName], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId;
	ELSE
		SELECT [PayClassId], [PayClassName], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = 0;

	IF(SELECT COUNT(*) FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId ORDER BY [Date];
	ELSE
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = 0 ORDER BY [Date];

	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[ProjectCreatedUtc],
			[Project].[StartUtc] as [StartDate],
			[Project].[EndUtc] as [EndDate],
			[Project].[ProjectName] AS [ProjectName],
			[Project].[IsActive],
			[Project].[IsHourly] AS [IsHourly],
			[Organization].[OrganizationName] AS [OrganizationName],
			[Customer].[CustomerName] AS [CustomerName],
			[Customer].[CustomerOrgId],
			[Customer].[IsActive] AS [IsCustomerActive],
			[ProjectUser].[IsActive] AS [IsUserActive],
			[OrganizationRoleId],
			[ProjectOrgId]
	FROM (
		(SELECT [OrganizationId], [UserId], [OrganizationRoleId]
		FROM [Auth].[OrganizationUser] WITH (NOLOCK) WHERE [UserId] = @UserId AND [OrganizationId] = @OrganizationId)
		AS [OrganizationUser]
		JOIN [Auth].[Organization]		WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
		JOIN [Crm].[Customer]		WITH (NOLOCK) ON [Customer].[OrganizationId] = [Organization].[OrganizationId]
		JOIN ( [Pjm].[Project]
			JOIN [Pjm].[ProjectUser] WITH (NOLOCK) ON [ProjectUser].[ProjectId] = [Project].[ProjectId]
		)
										ON [Project].[CustomerId] = [Customer].[CustomerId]
										AND [ProjectUser].[UserId] = [OrganizationUser].[UserId]
	
	)
	UNION ALL
	SELECT	[ProjectId],
			[CustomerId],
			0,
			[ProjectCreatedUtc],
			[StartUtc],
			[EndUtc],
			[ProjectName],
			[IsActive],
			[IsHourly],
			(SELECT [OrganizationName] FROM [Auth].[Organization] WITH (NOLOCK) WHERE [OrganizationId] = 0),
			(SELECT [CustomerName] FROM [Crm].[Customer] WITH (NOLOCK) WHERE [CustomerId] = 0),
			NULL,
			0,
			0,
			0,
			[ProjectOrgId]
			FROM [Pjm].[Project] WITH (NOLOCK) WHERE [ProjectId] = 0
	ORDER BY [Project].[ProjectName]

	SELECT [User].[UserId],
		[User].[FirstName],
		[User].[LastName],
		[User].[Email]
	FROM [Auth].[User] WITH (NOLOCK) 
	LEFT JOIN [Billing].[SubscriptionUser]	WITH (NOLOCK) ON [SubscriptionUser].[UserId] = [User].[UserId]
	LEFT JOIN [Billing].[Subscription]		WITH (NOLOCK) ON [Subscription].[SubscriptionId] = [SubscriptionUser].[SubscriptionId]
	WHERE 
		[Subscription].[SubscriptionId] = (
		SELECT [SubscriptionId] 
		FROM [Billing].[Subscription] WITH (NOLOCK) 
		LEFT JOIN [Billing].[Sku]		WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
		LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
		WHERE [Subscription].[OrganizationId] = @OrganizationId
			AND [Sku].[ProductId] = @ProductId
			AND [Subscription].[IsActive] = 1
		)
	ORDER BY [User].[LastName]

	SELECT DISTINCT [TimeEntryId] 
		,[User].[UserId] AS [UserId]
		,[User].[FirstName] AS [FirstName]
		,[User].[LastName] AS [LastName]
		,[User].[Email]
		,[OrganizationUser].[EmployeeId]
		,[TimeEntry].[ProjectId]
		,[TimeEntry].[PayClassId]
		,[PayClass].[PayClassName] AS [PayClassName]
		,[Date]
		,[Duration]
		,[Description]
	FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) 
	JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [TimeEntry].[UserId]
	JOIN [Hrm].[PayClass] WITH (NOLOCK) ON [PayClass].[PayClassId] = [TimeEntry].[PayClassId]
	JOIN [Auth].[OrganizationUser] WITH (NOLOCK) ON [User].[UserId] = [OrganizationUser].[UserId] AND [OrganizationUser].[OrganizationId] = @OrganizationId
	WHERE [User].[UserId] = @UserId
		AND [Date] >= @StartingDate
		AND [Date] <= @EndingDate
		AND [PayClass].[OrganizationId] = @OrganizationId
	ORDER BY [Date] ASC
GO
PRINT N'Altering [Auth].[CreateUser]...';


GO
ALTER PROCEDURE [Auth].[CreateUser]
	@firstName NVARCHAR(32),
	@lastName NVARCHAR(32),
    @address NVARCHAR(100), 
    @city NVARCHAR(32), 
    @state NVARCHAR(32), 
    @country NVARCHAR(32), 
    @postalCode NVARCHAR(16),
	@email NVARCHAR(256), 
    @phoneNumber VARCHAR(32),
	@dateOfBirth DATETIME2(0),
	@passwordHash NVARCHAR(MAX),
	@emailConfirmationCode UNIQUEIdENTIFIER,
	@isTwoFactorEnabled BIT,
	@isLockoutEnabled BIT,
	@lockoutEndDateUtc DATE,
	@languageId INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Lookup].[Address]
		([Address1],
		[City],
		[StateId],
		[CountryId],
		[PostalCode])
	VALUES
		(@address,
		@city,
		(SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [StateName] = @state),
		(SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [CountryName] = @country),
		@postalCode);

	INSERT INTO [Auth].[User] 
		([FirstName], 
		[LastName], 
		[AddressId],
		[Email], 
		[PhoneNumber], 
		[DateOfBirth],
		[PasswordHash],
		[EmailConfirmationCode],
		[IsEmailConfirmed],
		[IsTwoFactorEnabled],
		[AccessFailedCount],
		[IsLockoutEnabled],
		[LockoutEndDateUtc],
		[PreferredLanguageId])
	VALUES 
		(@firstName, 
		@lastName,
		SCOPE_IDENTITY(),
		@email,
		@phoneNumber,
		@dateOfBirth, 
		@passwordHash, 
		@emailConfirmationCode,
		0,
		COALESCE(@isTwoFactorEnabled,0),
		0,
		COALESCE(@isLockoutEnabled,0),
		COALESCE(@lockoutEndDateUtc,NULL),
		@languageId);

	SELECT SCOPE_IDENTITY();
END
GO
PRINT N'Altering [Auth].[GetUserFromEmail]...';


GO
ALTER PROCEDURE [Auth].[GetUserFromEmail]
	@Email NVARCHAR(384)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [UserId]
		,[FirstName]
		,[LastName]
		,[DateOfBirth]
		,[Address1] as 'Address'
		,[City]
		,[State].[StateName] AS 'State'
		,[Country].[CountryName] AS 'Country'
		,[PostalCode]
		,[Email]
		,[PhoneNumber]
		,[PhoneExtension]
		,[LastUsedSubscriptionId]
		,[LastUsedOrganizationId]
		,[PasswordHash]
		,[PreferredLanguageId]
	FROM [Auth].[User]
	WITH (NOLOCK)
	LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [User].[AddressId]
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Email] = @Email;
END
GO
PRINT N'Altering [Auth].[GetUserInfo]...';


GO
ALTER PROCEDURE [Auth].[GetUserInfo]
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [User].[UserId],
		   [User].[FirstName],
		   [User].[LastName],
		   [User].[DateOfBirth],
		   [User].[AddressId],
		   [User].[Email],
		   [User].[PhoneNumber],
		   [User].[LastUsedSubscriptionId],
		   [User].[LastUsedOrganizationId],
		   [User].[IsEmailConfirmed],
		   [User].[EmailConfirmationCode],
		   [PreferredLanguageId]
	FROM [Auth].[User]
	WITH (NOLOCK)
	JOIN [Lookup].[Address]			WITH (NOLOCK) ON [Address].[AddressId] = [User].[AddressId]
	WHERE [UserId] = @UserId;

	DECLARE @AddressId INT
	SET @AddressId = (SELECT U.AddressId
					 FROM [Auth].[User] AS U
					 WHERE [UserId] = @UserId)

	SELECT [Address].[AddressId],
		   [Address].[Address1],
		   [Address].[City],
		   [State].[StateName] AS 'State',
		   [Country].[CountryName] AS 'CountryId',
		   [Address].[PostalCode]
	FROM [Lookup].[Address] AS [Address] WITH (NOLOCK)
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [AddressId] = @AddressId
END
GO
PRINT N'Altering [Auth].[UpdateEmailConfirmed]...';


GO
ALTER PROCEDURE [Auth].[UpdateEmailConfirmed]
	@emailConfirmCode uniqueidentifier
AS
BEGIN
	UPDATE [Auth].[User]
	SET [IsEmailConfirmed] = 1
	WHERE [EmailConfirmationCode] = @emailConfirmCode
END
GO
PRINT N'Altering [Auth].[UpdateUserInfo]...';


GO
ALTER PROCEDURE [Auth].[UpdateUserInfo]
	@userId INT,
	@addressId INT,
	@firstName NVARCHAR(32),
	@lastName NVARCHAR(32),
	@address NVARCHAR(32),
	@city NVARCHAR(32),
	@state NVARCHAR(100),
	@country NVARCHAR(100),
	@postalCode NVARCHAR(16),
	@phoneNumber VARCHAR(16),
	@dateOfBirth DATE
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[User]
	SET [FirstName] = @firstName,
		[LastName] = @lastName,
		[PhoneNumber] = @phoneNumber,
		[DateOfBirth] = @dateOfBirth
	WHERE [UserId] = @userId

	UPDATE [Lookup].[Address]
	SET [Address1] = @address,
		[City] = @city,
		[StateId] = (SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [StateName] = @state),
		[CountryId] = (SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [CountryName] = @country),
		[PostalCode] = @postalCode
	WHERE [AddressId] = @addressId
END
GO
PRINT N'Altering [Auth].[UpdateUserPassword]...';


GO
ALTER PROCEDURE [Auth].[UpdateUserPassword]
	@userId int,
	@passwordHash nvarchar(512)
AS
BEGIN
	SET NOCOUNT ON
	UPDATE [Auth].[User]
	SET [PasswordHash] = @passwordHash
	WHERE [UserId] = @userId
END
GO
PRINT N'Altering [Auth].[UpdateUserPasswordResetCode]...';


GO
ALTER PROCEDURE [Auth].[UpdateUserPasswordResetCode]
	@email nvarchar (384),
	@passwordResetCode uniqueidentifier
AS
BEGIN
	UPDATE [Auth].[User]
	SET PasswordResetCode = @passwordResetCode
	WHERE Email = @email
END
GO
PRINT N'Altering [Auth].[UpdateUserPasswordUsingCode]...';


GO
ALTER PROCEDURE [Auth].[UpdateUserPasswordUsingCode]
	@passwordHash nvarchar(512),
	@passwordResetCode uniqueidentifier
AS
BEGIN
	UPDATE [Auth].[User]
	SET [PasswordHash] = @passwordHash, [PasswordResetCode] = NULL
	WHERE [PasswordResetCode] = @passwordResetCode
END
GO
PRINT N'Altering [Billing].[GetBillingHistoryByOrg]...';


GO
ALTER PROCEDURE [Billing].[GetBillingHistoryByOrg]
	@OrganizationId INT
AS
	SET NOCOUNT ON;
SELECT
	[BillingHistory].[OrganizationId],
	[BillingHistory].[BillingHistoryCreatedUtc] AS [Date],
	[BillingHistory].[Description],
	[BillingHistory].[UserId],
	[BillingHistory].[SkuId],
	[Sku].[SkuName] AS [SkuName],
	[Sku].[ProductId],
	[Product].[ProductName] AS [ProductName]
FROM [Billing].[BillingHistory] WITH (NOLOCK)
LEFT JOIN [Auth].[User] WITH (NOLOCK) ON [User].[UserId] = [BillingHistory].[UserId]
LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [BillingHistory].[SkuId]
LEFT JOIN [Billing].[Product] WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
WHERE [OrganizationId] = @OrganizationId
ORDER BY [Date] desc
GO
PRINT N'Altering [Pjm].[GetProjectsForOrgAndUser]...';


GO
ALTER PROCEDURE [Pjm].[GetProjectsForOrgAndUser]
	@UserId INT,
	@OrgId INT
AS
	SELECT [P].[ProjectId],
			[P].[ProjectName],
			[P].[ProjectOrgId],
			[C].[CustomerName] AS CustomerName
	FROM [Pjm].[ProjectUser] AS [PU] WITH (NOLOCK)
	LEFT JOIN [Pjm].[Project] AS [P] WITH (NOLOCK) ON [P].[ProjectId] = [PU].[ProjectId]
		JOIN [Crm].[Customer] AS [C] WITH (NOLOCK) ON [C].[CustomerId] = [P].[CustomerId]
		LEFT JOIN [Auth].[Organization] AS [O] WITH (NOLOCK) ON [O].[OrganizationId] = [C].[OrganizationId]
	WHERE [PU].[UserId] = @UserId AND [O].[OrganizationId] = @OrgId AND [PU].[IsActive] = 1 AND [P].[IsActive] = 1

	SELECT [P].[ProjectId],
			[P].[ProjectName],
			[P].[ProjectOrgId],
			[C].[CustomerName] AS CustomerName
	FROM [Pjm].[Project] AS [P] WITH (NOLOCK)
		JOIN [Crm].[Customer] AS [C] WITH (NOLOCK) ON [C].[CustomerId] = [P].[CustomerId]
		LEFT JOIN [Auth].[Organization] AS [O] WITH (NOLOCK) ON [O].[OrganizationId] = [C].[OrganizationId]
	WHERE [O].[OrganizationId] = @OrgId AND [P].[IsActive] = 1
	
	SELECT [FirstName],
		[LastName],
		[Email]
	FROM [Auth].[User] WITH (NOLOCK)
	WHERE [User].[UserId] = @UserId
GO
PRINT N'Altering [Auth].[GetActiveProductRoleForUser]...';


GO
ALTER PROCEDURE [Auth].[GetActiveProductRoleForUser]
	@ProductName VARCHAR (32),
	@OrganizationId INT,
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [ProductRoleName]												
	FROM [Auth].[ProductRole]
	WITH (NOLOCK)								
	WHERE [ProductRoleId] =										
		(SELECT [ProductRoleId]									
		FROM [Billing].[SubscriptionUser]
		WITH (NOLOCK)						
		WHERE [UserId] = @UserId								
		AND [SubscriptionId] =									
			(SELECT [SubscriptionId]							
			FROM [Billing].[Subscription]
			WITH (NOLOCK)						
			WHERE [OrganizationId] = @OrganizationId			
				AND [IsActive] = 1								
				AND [SkuId] IN									
					(SELECT [SkuId]								
					FROM [Billing].[Sku]	
					WITH (NOLOCK)					
					WHERE [ProductId] IN						
						(SELECT [ProductId]						
						FROM [Billing].[Product]
						WITH (NOLOCK)				
						WHERE [Product].[ProductName] = @ProductName))));
END
GO
PRINT N'Altering [Billing].[DeleteSubscription]...';


GO
ALTER PROCEDURE [Billing].[DeleteSubscription]
	@SubscriptionId INT
AS
BEGIN
	SET NOCOUNT ON;

	-- Delete subscription
	UPDATE [Billing].[Subscription]
	SET [IsActive] = 0
	WHERE [SubscriptionId] = @SubscriptionId

	-- Return sku name for the notification
	SELECT [Sku].[SkuName]
	FROM [Billing].[Subscription] WITH (NOLOCK)
	JOIN [Billing].[Sku] WITH (NOLOCK) ON [Subscription].[SkuId] = [Sku].[SkuId]
	WHERE [Subscription].[SubscriptionId] = @SubscriptionId
END
GO
PRINT N'Altering [Billing].[GetOrgSkus]...';


GO
ALTER PROCEDURE [Billing].[GetOrgSkus]
	@OrganizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		[A1].[SubscriptionId],
		[A1].[ProductName],
		[A2].[UserCount],
		[A1].[SkuId]
	FROM
	(
		SELECT
			[OrgSub].[OrganizationId],
			[OrgSub].[SkuId],
			[ProductName],
			[SubscriptionId]
		FROM [Billing].[Subscription] AS [OrgSub] WITH (NOLOCK) 
		INNER JOIN 
		(
			SELECT
				[Sku].[SkuId],
				[Product].[ProductName]
			FROM [Billing].[Product] AS [Product] WITH (NOLOCK) 
			INNER JOIN [Billing].[Sku] AS [Sku] WITH (NOLOCK) 
			ON [Product].[ProductId] = [Sku].[ProductId]
		) AS [ProductSku]
		ON [ProductSku].[SkuId] = [OrgSub].[SkuId] AND [OrgSub].[IsActive] = 1
		WHERE [OrgSub].[OrganizationId] = @OrganizationId
	) AS [A1]
	INNER JOIN
	(
		SELECT [SubUser].[SubscriptionId], COUNT(*) AS [UserCount]
		FROM [Billing].[SubscriptionUser] AS [SubUser] WITH (NOLOCK) 
		INNER JOIN [Billing].[Subscription] AS [OrgSubs] WITH (NOLOCK) 
		ON [OrgSubs].[SubscriptionId] = [SubUser].[SubscriptionId]
		WHERE [OrgSubs].[OrganizationId] = @OrganizationId AND [OrgSubs].[IsActive] = 1
		GROUP BY [SubUser].[SubscriptionId]
	) AS [A2]
	ON [A1].[SubscriptionId] = [A2].[SubscriptionId]
	ORDER BY [A1].[ProductName]
END
GO
PRINT N'Altering [Billing].[GetProductAreaBySubscription]...';


GO
ALTER PROCEDURE [Billing].[GetProductAreaBySubscription]
	@SubscriptionId INT
AS
	SET NOCOUNT ON;
	SELECT DISTINCT [Product].[ProductName]
	FROM [Billing].[Product] WITH (NOLOCK) 
	INNER JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[ProductId] = [Product].[ProductId]
	INNER JOIN [Billing].[Subscription] WITH (NOLOCK) ON [Subscription].[SkuId] = [Sku].[SkuId]
GO
PRINT N'Altering [Billing].[GetProductRolesFromSubscription]...';


GO
ALTER PROCEDURE [Billing].[GetProductRolesFromSubscription]
	@SubscriptionId INT
AS
	SET NOCOUNT ON;
SELECT 
	[ProductRole].[ProductRoleName],
	[ProductRole].[ProductRoleId]
FROM [Billing].[Subscription] WITH (NOLOCK) 
LEFT JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
RIGHT JOIN [Auth].[ProductRole]  WITH (NOLOCK) ON [ProductRole].[ProductId] = [Sku].[ProductId]
WHERE [SubscriptionId] = @SubscriptionId
GO
PRINT N'Altering [Billing].[GetProductSubscriptionInfo]...';


GO
ALTER PROCEDURE [Billing].[GetProductSubscriptionInfo]
	@skuId INT,
	@orgId INT
AS
	SET NOCOUNT ON;
	DECLARE @ProductId INT;
	DECLARE @SubscriptionId INT;

SELECT @ProductId = [Product].[ProductId]
FROM [Billing].[Product] 
	  LEFT JOIN [Billing].[Sku] WITH (NOLOCK) 
	  ON [Product].ProductId = [Sku].ProductId	  
	  WHERE [Sku].SkuId = @skuId

SELECT 
	[Product].[ProductName], 
	[Product].[ProductId], 
	[Product].[Description], 
	[Product].[AreaUrl]
	FROM [Billing].[Product]   
	WHERE [Product].ProductId = @ProductId

	SELECT
		@SubscriptionId = [SubscriptionId]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	WHERE [OrganizationId] = @orgId AND [Subscription].[SkuId] = @skuId AND [Subscription].[IsActive] = 1

	SELECT
		[SubscriptionId],
		[SkuId],
		[NumberOfUsers],
		[SubscriptionCreatedUtc],
		[OrganizationId]
	FROM [Billing].[Subscription] WITH (NOLOCK) 
	WHERE [SubscriptionId] = @SubscriptionId

	SELECT [SkuId],
		[ProductId],
		[SkuName],
		[CostPerBlock],
		[UserLimit],
		[BillingFrequency],
		[BlockBasedOn],
		[BlockSize],
		[PromoCostPerBlock],
		[PromoDeadline],
		[IsActive],
		[Description]
	FROM [Billing].[Sku] WITH (NOLOCK) 
	WHERE [Billing].[Sku].[ProductId] = @ProductId

	SELECT [StripeTokenCustId]
	FROM [Billing].[StripeOrganizationCustomer] WITH (NOLOCK) 
	WHERE [OrganizationId] = @orgId AND [IsActive] = 1

	SELECT COUNT([UserId])
	FROM (
		SELECT [SubscriptionUser].[UserId]
		FROM [Billing].[SubscriptionUser] WITH (NOLOCK)
		LEFT JOIN [Billing].[Subscription] WITH (NOLOCK) ON [Subscription].[SubscriptionId] = [SubscriptionUser].[SubscriptionId]
		WHERE 
			[Subscription].[SubscriptionId] = @SubscriptionId
	) src
GO
PRINT N'Altering [Billing].[GetSubscriptionDetailsById]...';


GO
ALTER PROCEDURE [Billing].[GetSubscriptionDetailsById]
	@SubscriptionId INT
AS
	SET NOCOUNT ON;
SELECT [OrganizationId]
      ,[SkuId]
	  ,[NumberOfUsers]
      ,[SubscriptionCreatedUtc]
      ,[IsActive]
FROM [Billing].[Subscription] WITH (NOLOCK) 
WHERE [SubscriptionId] = @SubscriptionId AND [IsActive] = 1
GO
PRINT N'Altering [Billing].[GetSubscriptionsDisplayByOrg]...';


GO
ALTER PROCEDURE [Billing].[GetSubscriptionsDisplayByOrg]
	@OrganizationId INT
AS
	SET NOCOUNT ON;
SELECT	[Product].[ProductId],
		[Product].[ProductName] AS [ProductName],
		[Subscription].[SubscriptionId],
		[Organization].[OrganizationId],
		[Subscription].[SkuId],
		[Subscription].[NumberOfUsers],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Sku].[SkuName] AS [SkuName]
  FROM [Billing].[Subscription] WITH (NOLOCK) 
  LEFT JOIN [Billing].[Sku]			WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
  LEFT JOIN [Auth].[Organization]	WITH (NOLOCK) ON [Organization].[OrganizationId] = [Subscription].[OrganizationId]
  LEFT JOIN [Billing].[Product]		WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId]
  WHERE [Subscription].[OrganizationId] = @OrganizationId
	AND [Subscription].[IsActive] = 1
ORDER BY [Product].[ProductName]
GO
PRINT N'Altering [TimeTracker].[CreateBulkTimeEntry]...';


GO
ALTER PROCEDURE [TimeTracker].[CreateBulkTimeEntry]
	@Date DATETIME2(0),
	@Duration FLOAT,
	@Description NVARCHAR(120),
	@PayClassId INT,
	@OrganizationId INT,
	@Overwrite BIT = 0
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TTProductId INT = (SELECT [ProductId] FROM [Billing].[Product] WHERE [Product].[ProductName] = 'TimeTracker');
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
			
			Declare @FirstProject as int
			set @FirstProject = (SELECT TOP 1 [ProjectId] 
				FROM [Pjm].[ProjectUser], #Tmp
				WITH (NOLOCK) 
				WHERE [ProjectUser].[UserId] = [#Tmp].[UserId] AND [ProjectUser].[IsActive] = 1);

			if @FirstProject is null begin set @FirstProject = 0 end
			UPDATE #Tmp SET [FirstProject] = @FirstProject

				

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
			DECLARE @SubscriptionId INT = 
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
			FROM [Billing].[SubscriptionUser] WITH (NOLOCK) WHERE [SubscriptionId] = @SubscriptionId;

			Declare @FirstProejct as int
			set @FirstProject = (SELECT TOP 1 [ProjectId] 
				FROM [Pjm].[ProjectUser], #OrgTmp WITH (NOLOCK) 
				WHERE [ProjectUser].[UserId] = [#OrgTmp].[UserId] AND [ProjectUser].[IsActive] = 1 
					AND [ProjectId] IN 
						(SELECT [ProjectId] 
						FROM [Pjm].[Project] WITH (NOLOCK) 
						JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId] WHERE [Customer].[OrganizationId] = @OrganizationId));
			--^^^ Sets the column that contains the first project id for each user for the specified org

			if (@FirstProject is null) begin set @FirstProject = 0 end
			UPDATE #OrgTmp SET [FirstProject] = @FirstProject
				
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
GO
PRINT N'Altering [Crm].[DeleteCustomer]...';


GO
ALTER PROCEDURE [Crm].[DeleteCustomer]
	@CustomerId INT
AS
BEGIN
	SET NOCOUNT ON;
	-- Retrieve the customer's name
	DECLARE @CustomerName NVARCHAR(384);

	SELECT 
		@CustomerName = [CustomerName] 
	FROM [Crm].[Customer] WITH (NOLOCK)
	WHERE [CustomerId] = @CustomerId

	IF @CustomerName IS NOT NULL
	BEGIN --Customer found
		UPDATE [Crm].[Customer] SET [IsActive] = 0
		WHERE [Customer].[CustomerId] = @CustomerId;
	 
		UPDATE [Pjm].[Project] SET [IsActive] = 0
		WHERE [Project].[CustomerId] IN (SELECT [CustomerId] FROM [Crm].[Customer] WHERE [IsActive] = 0);
	END
	SELECT @CustomerName
END
GO
PRINT N'Altering [Crm].[GetInactiveProjectsAndCustomersForOrgAndUser]...';


GO
ALTER PROCEDURE [Crm].[GetInactiveProjectsAndCustomersForOrgAndUser]
	@OrgId int,
	@UserId int
AS
	SET NOCOUNT ON

	SELECT	[Project].[ProjectId],
		[Project].[CustomerId],
		[Customer].[OrganizationId],
		[Project].[ProjectCreatedUtc],
		[Project].[ProjectName] AS [ProjectName],
		[Project].[IsActive],
		[ProjectOrgId],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Customer].[CustomerName] AS [CustomerName],
		[Customer].[CustomerOrgId],
		[Customer].[IsActive] AS [IsCustomerActive],
		[Project].[IsHourly] AS [IsHourly],
		[SUB].[IsProjectUser]
	FROM (
		[Auth].[Organization] WITH (NOLOCK) 
		JOIN [Crm].[Customer] WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @OrgId)
		JOIN [Pjm].[Project] WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
		LEFT JOIN (
			SELECT 1 AS 'IsProjectUser',
			[ProjectUser].[ProjectId]
			FROM [Pjm].[ProjectUser] WITH (NOLOCK)
			WHERE [ProjectUser].[UserId] = @UserId
		) [SUB] ON [SUB].[ProjectId] = [Project].[ProjectId]
	)
	
	WHERE [Customer].[IsActive] = 0
	OR [Project].[IsActive] = 0

	ORDER BY [Project].[ProjectName]

	SELECT DISTINCT [Customer].[CustomerId],
		   [Customer].[CustomerName],
		   [Address1],
		   [City],
		   [State].[StateName] AS 'State',
		   [Country].[CountryName] AS 'Country',
		   [PostalCode],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CustomerCreatedUtc],
		   [Customer].[CustomerOrgId],
		   [Customer].[IsActive]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Pjm].[Project] WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Customer].[OrganizationId] = @OrgId
	AND ([Customer].[IsActive] = 0
	OR [Project].[IsActive] = 0)
	ORDER BY [Customer].[CustomerName]
GO
PRINT N'Altering [Crm].[GetProjectByIdAndUser]...';


GO
ALTER PROCEDURE [Crm].[GetProjectByIdAndUser]
	@ProjectId int,
	@UserId int
AS
	SET NOCOUNT ON;
	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[ProjectCreatedUtc],
			[Project].[ProjectName] AS [ProjectName],
			[Organization].[OrganizationName] AS [OrganizationName],
			[Customer].[CustomerName] AS [CustomerName],
			[Customer].[CustomerOrgId],
			[Project].[IsHourly] AS [PriceType],
			[Project].[StartUtc] AS [StartDate],
			[Project].[EndUtc] AS [EndDate],
			[Project].[ProjectOrgId],
			[SUB].[IsProjectUser]
			FROM (
		(SELECT [ProjectId], [CustomerId], [ProjectName], [IsHourly], [StartUtc], [EndUtc], [IsActive], 
				[ProjectCreatedUtc], [ProjectOrgId] FROM [Pjm].[Project] WITH (NOLOCK) WHERE [ProjectId] = @ProjectId) AS [Project]
			JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
			JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [Customer].[OrganizationId]
			LEFT JOIN (
				SELECT 1 AS 'IsProjectUser',
				[ProjectUser].[ProjectId]
				FROM [Pjm].[ProjectUser] WITH (NOLOCK)
				WHERE [ProjectUser].[ProjectId] = @ProjectId AND [ProjectUser].[UserId] = @UserId
			) [SUB] ON [SUB].[ProjectId] = @ProjectId
		)
GO
PRINT N'Altering [Hrm].[DeleteHoliday]...';


GO
ALTER PROCEDURE [Hrm].[DeleteHoliday]
	@HolidayName NVarChar(120),
	@Date DATE,
	@OrganizationId INT
AS
	SET NOCOUNT ON;

	DELETE FROM [Hrm].[Holiday] WHERE [HolidayName] = @HolidayName AND [Date] = @Date AND [OrganizationId] = @OrganizationId;

	BEGIN
		DELETE FROM [TimeTracker].[TimeEntry]
		WHERE [Date] = @Date
		AND [Duration] = 8
		AND [PayClassId] = (SELECT TOP 1 [PayClassId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [PayClassName] = 'Holiday')
		AND [ProjectId] IN (SELECT [ProjectId]
							FROM [Pjm].[Project] WITH (NOLOCK) JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
							WHERE [Customer].[OrganizationId] = @OrganizationId);
	END
GO
PRINT N'Altering [Pjm].[CreateProject]...';


GO
ALTER PROCEDURE [Pjm].[CreateProject]
	@CustomerId INT,
	@ProjectName NVARCHAR(MAX),
	@IsHourly BIT,
	@ProjectOrgId NVARCHAR(16),
	@StartingDate DATETIME2(0),
	@EndingDate DATETIME2(0),
    @retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	INSERT INTO [Pjm].[Project] ([CustomerId], [ProjectName], [IsHourly], [ProjectOrgId], [StartUtc], [EndUtc])
	VALUES	(@CustomerId, @ProjectName, @IsHourly, @ProjectOrgId, @StartingDate, @EndingDate);
	SET @retId = SCOPE_IDENTITY()
	COMMIT TRANSACTION
	SELECT SCOPE_IDENTITY();
END
GO
PRINT N'Altering [Pjm].[CreateProjectAndUpdateItsUserList]...';


GO
ALTER PROCEDURE [Pjm].[CreateProjectAndUpdateItsUserList]
	@CustomerId INT,
	@ProjectName NVARCHAR(MAX),
	@IsHourly BIT,
	@ProjectOrgId NVARCHAR(16),
	@StartingDate DATETIME2(0),
	@EndingDate DATETIME2(0),
	@UserIds [Auth].[UserTable] READONLY,
    @retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS (
		SELECT * FROM [Pjm].[Project] WITH (NOLOCK)
		WHERE [ProjectOrgId] = @ProjectOrgId
		AND [CustomerId] = @CustomerId
	)
		BEGIN
			-- ProjectOrgId is not unique
			SET @retId = -1;
		END
	ELSE
		BEGIN
			BEGIN TRANSACTION
				-- Create the new project in Project table
				INSERT INTO [Pjm].[Project] ([CustomerId], [ProjectName], [IsHourly], [ProjectOrgId], [StartUtc], [EndUtc])
				VALUES	(@CustomerId, @ProjectName, @IsHourly, @ProjectOrgId, @StartingDate, @EndingDate);
				SET @retId = SCOPE_IDENTITY()

				/* Update new users that used to be users at some point */
				UPDATE [Pjm].[ProjectUser] SET IsActive = 1
				WHERE [ProjectUser].[ProjectId] = @retId 
					AND [ProjectUser].[UserId] IN (SELECT userId FROM @UserIds) 
					AND [ProjectUser].[IsActive] = 0

				/* Add new users that have never been on the project */
				INSERT INTO [Pjm].[ProjectUser] ([ProjectId], [UserId], [IsActive])
				SELECT @retId, userId, 1
				FROM @UserIds
				WHERE userId NOT IN
					(SELECT [ProjectUser].[UserId]
					FROM [Pjm].[ProjectUser] WITH (NOLOCK)
					WHERE [ProjectUser].[ProjectId] = @retId)

				/* Set inactive existing users that are not in the updated users list */
				UPDATE [Pjm].[ProjectUser] SET IsActive = 0
				WHERE [ProjectUser].[ProjectId] = @retId
					AND [ProjectUser].[UserId] NOT IN (SELECT userId FROM @UserIds) 
					AND [ProjectUser].[IsActive] = 1

			COMMIT TRANSACTION		
		END
	SELECT @retId;
END
GO
PRINT N'Altering [Pjm].[DeleteProject]...';


GO
ALTER PROCEDURE [Pjm].[DeleteProject]
	@ProjectId INT,
	@DeactivateDate DATE
AS
BEGIN
	SET NOCOUNT ON;
	-- Retrieve the project's name
	DECLARE @ProjectName NVARCHAR(384);

	SELECT 
		@ProjectName = [ProjectName] 
	FROM [Pjm].[Project] WITH (NOLOCK)
	WHERE [ProjectId] = @ProjectId

	IF @ProjectName IS NOT NULL
	BEGIN --Project found
		UPDATE [Pjm].[Project]
		SET [IsActive] = 0, [EndUtc] = @DeactivateDate
		WHERE [ProjectId] = @ProjectId
	 
		UPDATE [Pjm].[ProjectUser] SET [IsActive] = 0
		WHERE [ProjectUser].[ProjectId] IN (SELECT [ProjectId] FROM [Pjm].[Project] WHERE [IsActive] = 0);
	END
	SELECT @ProjectName
END
GO
PRINT N'Altering [Pjm].[GetInactiveProjectsByCustomer]...';


GO
ALTER PROCEDURE [Pjm].[GetInactiveProjectsByCustomer]
	@CustomerId INT
AS
	SET NOCOUNT ON;
	SELECT [ProjectName],
		   [ProjectId],
		   [ProjectOrgId],
		   [IsHourly],
		   [CustomerId],
		   [StartUtc] AS [StartingDate],
		   [EndUtc] AS [EndingDate]
	FROM [Pjm].[Project] WITH (NOLOCK) 
	WHERE [IsActive] = 0 AND [CustomerId] = @CustomerId
	ORDER BY [Project].[ProjectName]
GO
PRINT N'Altering [Pjm].[GetProjectById]...';


GO
ALTER PROCEDURE [Pjm].[GetProjectById]
	@ProjectId INT
AS
	SET NOCOUNT ON;
	SELECT	[Project].[ProjectId],
			[Project].[CustomerId],
			[Customer].[OrganizationId],
			[Project].[ProjectCreatedUtc],
			[Project].[ProjectName] AS [ProjectName],
			[Organization].[OrganizationName] AS [OrganizationName],
			[Customer].[CustomerName] AS [CustomerName],
			[Customer].[CustomerOrgId],
			[Project].[IsHourly] AS [PriceType],
			[Project].[StartUtc] AS [StartDate],
			[Project].[EndUtc] AS [EndDate],
			[Project].[ProjectOrgId]
			FROM (
		(SELECT [ProjectId], [CustomerId], [ProjectName], [IsHourly], [StartUtc], [EndUtc], [IsActive], 
				[ProjectCreatedUtc], [ProjectOrgId] FROM [Pjm].[Project] WITH (NOLOCK) WHERE [ProjectId] = @ProjectId) AS [Project]
			JOIN [Crm].[Customer] WITH (NOLOCK) ON [Customer].[CustomerId] = [Project].[CustomerId]
			JOIN [Auth].[Organization] WITH (NOLOCK) ON [Organization].[OrganizationId] = [Customer].[OrganizationId]
	)
GO
PRINT N'Altering [Pjm].[GetProjectsAndCustomersForOrgAndUser]...';


GO
ALTER PROCEDURE [Pjm].[GetProjectsAndCustomersForOrgAndUser]
	@OrgId int,
	@UserId int
AS
	SET NOCOUNT ON

	SELECT	[Project].[ProjectId],
		[Project].[CustomerId],
		[Customer].[OrganizationId],
		[Project].[ProjectCreatedUtc],
		[Project].[ProjectName] AS [ProjectName],
		[Project].[IsActive],
		[ProjectOrgId],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Customer].[CustomerName] AS [CustomerName],
		[Customer].[CustomerOrgId],
		[Customer].[IsActive] AS [IsCustomerActive],
		[Project].[IsHourly] AS [IsHourly],
		[SUB].[IsProjectUser]
	FROM (
		[Auth].[Organization] WITH (NOLOCK) 
		JOIN [Crm].[Customer]	WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @OrgId)
		JOIN [Pjm].[Project]		WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
		LEFT JOIN (
			SELECT 1 AS 'IsProjectUser',
			[ProjectUser].[ProjectId]
			FROM [Pjm].[ProjectUser] WITH (NOLOCK)
			WHERE [ProjectUser].[UserId] = @UserId
		) [SUB] ON [SUB].[ProjectId] = [Project].[ProjectId]
	)
	
	WHERE [Customer].[IsActive] >= 1
		AND [Project].[IsActive] >= 1

	ORDER BY [Project].[ProjectName]

	SELECT [Customer].[CustomerId],
		   [Customer].[CustomerName],
		   [Address1],
		   [City],
		   [State].[StateName] AS 'State',
		   [Country].[CountryName] AS 'Country',
		   [PostalCode],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CustomerCreatedUtc],
		   [Customer].[CustomerOrgId]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Customer].[OrganizationId] = @OrgId
	AND [Customer].[IsActive] = 1
	ORDER BY [Customer].[CustomerName]
GO
PRINT N'Altering [Pjm].[GetProjectsByCustomer]...';


GO
ALTER PROCEDURE [Pjm].[GetProjectsByCustomer]
	@CustomerId INT
AS
	SET NOCOUNT ON;
	SELECT [ProjectName],
		   [ProjectId],
		   [ProjectOrgId],
		   [IsHourly],
		   [CustomerId],
		   [StartUtc] AS [StartingDate],
		   [EndUtc] AS [EndingDate]
	FROM [Pjm].[Project] WITH (NOLOCK) 
	WHERE [IsActive] = 1 AND [CustomerId] = @CustomerId
	ORDER BY [Project].[ProjectName]
GO
PRINT N'Altering [Pjm].[GetProjectsByOrgId]...';


GO
ALTER PROCEDURE [Pjm].[GetProjectsByOrgId]
	@OrgId INT,
	@Activity INT = 1
AS
	SET NOCOUNT ON;
SELECT	[Project].[ProjectId],
		[Project].[CustomerId],
		[Customer].[OrganizationId],
		[Project].[ProjectCreatedUtc],
		[Project].[ProjectName] AS [ProjectName],
		[Project].[IsActive],
		[ProjectOrgId],
		[Organization].[OrganizationName] AS [OrganizationName],
		[Customer].[CustomerName] AS [CustomerName],
		[Customer].[CustomerOrgId],
		[Customer].[IsActive] AS [IsCustomerActive],
		[Project].[IsHourly] AS [IsHourly]
		--[OrganizationRoleId]
FROM (
--(SELECT [OrganizationId], [OrganizationRoleId] FROM [Auth].[OrganizationUser] WHERE [OrganizationId] = @orgId) AS OrganizationUser
	[Auth].[Organization] WITH (NOLOCK) 
	JOIN [Crm].[Customer]	WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId] AND [Organization].[OrganizationId] = @OrgId)
	JOIN [Pjm].[Project]		WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
)
	
WHERE [Customer].[IsActive] >= @Activity
	AND [Project].[IsActive] >= @Activity

ORDER BY [Project].[ProjectName]
GO
PRINT N'Altering [Pjm].[UpdateProject]...';


GO
ALTER PROCEDURE [Pjm].[UpdateProject]
	@ProjectId INT,
	@ProjectName NVARCHAR(MAX),
	@IsHourly BIT,
    @StartingDate DATE,
    @EndingDate DATE,
	@ProjectOrgId NVARCHAR(16)
AS
	SET NOCOUNT ON;
	UPDATE [Pjm].[Project]
	SET 
		[ProjectName] = @ProjectName,
		[IsHourly] = @IsHourly,
		[StartUtc] = @StartingDate,
		[EndUtc] = @EndingDate,
		[ProjectOrgId] = @ProjectOrgId

	WHERE [ProjectId] = @ProjectId
GO
PRINT N'Altering [Pjm].[UpdateProjectAndUsers]...';


GO
ALTER PROCEDURE [Pjm].[UpdateProjectAndUsers]
	@ProjectId INT,
	@ProjectName NVARCHAR(MAX),
	@OrgId NVARCHAR(16),
	@IsHourly BIT,
    @StartingDate DATE,
    @EndingDate DATE,
	@UserIds [Auth].[UserTable] READONLY
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
		
		/* Update new users that used to be users at some point */
		UPDATE [Pjm].[ProjectUser] SET IsActive = 1
		WHERE [ProjectUser].[ProjectId] = @ProjectId 
			AND [ProjectUser].[UserId] IN (SELECT userId FROM @UserIds) 
			AND [ProjectUser].[IsActive] = 0

		/* Add new users that have never been on the project */
		INSERT INTO [Pjm].[ProjectUser] ([ProjectId], [UserId], [IsActive])
		SELECT @ProjectId, userId, 1
		FROM @UserIds
		WHERE userId NOT IN
			(SELECT [ProjectUser].[UserId]
			FROM [Pjm].[ProjectUser] WITH (NOLOCK)
			WHERE [ProjectUser].[ProjectId] = @ProjectId)

		/* Set inactive existing users that are not in the updated users list */
		UPDATE [Pjm].[ProjectUser] SET IsActive = 0
		WHERE [ProjectUser].[ProjectId] = @ProjectId
			AND [ProjectUser].[UserId] NOT IN (SELECT userId FROM @UserIds) 
			AND [ProjectUser].[IsActive] = 1

		/* Update other project properties */
		UPDATE [Pjm].[Project]
		SET 
			[ProjectName] = @ProjectName,
			[ProjectOrgId] = @OrgId,
			[IsHourly] = @IsHourly,
			[StartUtc] = @StartingDate,
			[EndUtc] = @EndingDate
		WHERE [ProjectId] = @ProjectId

	COMMIT TRANSACTION
END
GO
PRINT N'Altering [TimeTracker].[GetAllSettings]...';


GO
ALTER PROCEDURE [TimeTracker].[GetAllSettings]
	@OrganizationId INT
AS
	SET NOCOUNT ON;
	SELECT [OrganizationId], [StartOfWeek], [OvertimeHours], [OvertimePeriod], [OvertimeMultiplier], [IsLockDateUsed], [LockDatePeriod], [LockDateQuantity]
	FROM [TimeTracker].[Setting] 
	WITH (NOLOCK) 
	WHERE [OrganizationId] = @OrganizationId;

	IF(SELECT COUNT(*) FROM [Hrm].[PayClass] WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [PayClassId], [PayClassName], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId;
	ELSE
		SELECT [PayClassId], [PayClassName], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = 0;

	IF(SELECT COUNT(*) FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId ORDER BY [Date];
	ELSE
		SELECT [HolidayId], [HolidayName], [Date], [OrganizationId] FROM [Hrm].[Holiday] WITH (NOLOCK) WHERE [OrganizationId] = 0 ORDER BY [Date];
GO
PRINT N'Altering [TimeTracker].[GetLockDate]...';


GO
ALTER PROCEDURE [TimeTracker].[GetLockDate]
	@OrganizationId INT
AS
	SET NOCOUNT ON;
	SELECT [IsLockDateUsed], [LockDatePeriod], [LockDateQuantity] FROM [TimeTracker].[Setting] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId;
GO
PRINT N'Altering [TimeTracker].[GetSettings]...';


GO
ALTER PROCEDURE [TimeTracker].[GetSettings]
	@OrganizationId INT
AS
	SET NOCOUNT ON;
	SELECT [OrganizationId], [StartOfWeek], [OvertimeHours], [OvertimePeriod], [OvertimeMultiplier], [IsLockDateUsed], [LockDatePeriod], [LockDateQuantity]
	FROM [TimeTracker].[Setting] 
	WITH (NOLOCK) 
	WHERE [OrganizationId] = @OrganizationId;
GO
PRINT N'Altering [TimeTracker].[UpdateLockDate]...';


GO
ALTER PROCEDURE [TimeTracker].[UpdateLockDate]
	@OrganizationId INT,
	@IsLockDateUsed BIT,
	@LockDatePeriod INT,
	@LockDateQuantity INT
AS
	SET NOCOUNT ON;
	UPDATE [TimeTracker].[Setting]
		SET [IsLockDateUsed] = @IsLockDateUsed,
			[LockDatePeriod] = @LockDatePeriod,
			[LockDateQuantity] = @LockDateQuantity
		WHERE [OrganizationId] = @OrganizationId;
GO
PRINT N'Altering [TimeTracker].[GetTimeEntriesThatUseAPayClass]...';


GO
ALTER PROCEDURE [TimeTracker].[GetTimeEntriesThatUseAPayClass]
	@PayClassId INT
AS
	SET NOCOUNT ON;
SELECT DISTINCT [TimeEntryId], [ProjectId], [PayClassId], [Duration], [Description], [IsLockSaved] 
FROM [TimeTracker].[TimeEntry] WITH (NOLOCK)
WHERE [PayClassId] = @PayClassId
GO
PRINT N'Altering [TimeTracker].[GetTimeEntryById]...';


GO
ALTER PROCEDURE [TimeTracker].[GetTimeEntryById]
	@TimeEntryId INT
AS
	SET NOCOUNT ON;
	SELECT [UserId],
		[ProjectId],
		[PayClassId],
		[Date],
		[Duration],
		[Description],
		[IsLockSaved]
	FROM [TimeTracker].[TimeEntry] WITH (NOLOCK) 
	WHERE [TimeEntryId] = @TimeEntryId
GO
PRINT N'Altering [TimeTracker].[UpdateTimeEntry]...';


GO
ALTER PROCEDURE [TimeTracker].[UpdateTimeEntry]
	@TimeEntryId INT,
    @ProjectId INT,
	@PayClassId INT,
	@Duration FLOAT,
	@Description NVARCHAR(120),
	@IsLockSaved BIT
AS
	SET NOCOUNT ON;
UPDATE [TimeTracker].[TimeEntry]
   SET [ProjectId] = @ProjectId
	  ,[PayClassId] = @PayClassId
      ,[Duration] = @Duration
      ,[Description] = @Description
	  ,[IsLockSaved] = @IsLockSaved
 WHERE [TimeEntryId] = @TimeEntryId
GO
PRINT N'Altering [Auth].[GetOrg]...';


GO
ALTER PROCEDURE [Auth].[GetOrg]
	@OrganizationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[OrganizationId],
		[Organization].[OrganizationName],
		[SiteUrl], 
		[Address1] AS 'Address', 
		[City], 
		[State].[StateName] AS 'State', 
		[Country].[CountryName] AS 'Country', 
		[PostalCode], 
		[PhoneNumber], 
		[FaxNumber], 
		[Subdomain],
		[OrganizationCreatedUtc]
	FROM [Auth].[Organization] WITH (NOLOCK)
		LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
		LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
		LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE OrganizationId = @OrganizationId
END
GO
PRINT N'Altering [Billing].[CreateBillingHistory]...';


GO
ALTER PROCEDURE [Billing].[CreateBillingHistory]
	@description NVARCHAR(MAX),
	@organizationId INT,
	@userId INT,
	@skuId INT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [Billing].[BillingHistory]
		([Date],
		[Description],
		[OrganizationId],
		[UserId],
		[SkuId])
	VALUES (
		SYSDATETIME(),
		@description,
		@organizationId,
		@userId,
		@skuId);
END
GO
PRINT N'Altering [Billing].[CreateSubscriptionPlan]...';


GO
ALTER PROCEDURE [Billing].[CreateSubscriptionPlan]
	@organizationId INT,
	@stripeTokenCustId NVARCHAR(50),
	@stripeTokenSubId NVARCHAR(50),
	@numberOfUsers INT,
	@price INT,
	@productId INT,
	@userId INT,
	@skuId INT,
	@description NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
		INSERT INTO [Billing].[StripeCustomerSubscriptionPlan] (
			[OrganizationId],
			[StripeTokenCustId],
			[StripeTokenSubId],
			[NumberOfUsers],
			[Price],
			[ProductId],
			[IsActive])
		VALUES (
			@organizationId,
			@stripeTokenCustId,
			@stripeTokenSubId,
			@numberOfUsers,
			@price,
			@productId,
			1);

		INSERT INTO [Billing].[BillingHistory]
			([Date],
			[Description],
			[OrganizationId],
			[UserId],
			[SkuId])
		VALUES
			(SYSDATETIME(),
			@description,
			@organizationId,
			@userId,
			@skuId)
	COMMIT
END
GO
PRINT N'Altering [Billing].[GetAllActiveProductsAndSkus]...';


GO
ALTER PROCEDURE [Billing].[GetAllActiveProductsAndSkus]
AS
	SET NOCOUNT ON;
	SELECT
		[Product].[ProductId],
		[Product].[ProductName],
		[Product].[Description],
		[Product].[AreaUrl]
	FROM [Billing].[Product] WITH (NOLOCK) 
	WHERE [IsActive] = 1
	ORDER BY [Product].[ProductId]

	SELECT
		[Product].[ProductId],
		[Sku].[SkuId],
		[Sku].[SkuName],
		[Sku].[CostPerBlock] AS 'Price',
		[Sku].[UserLimit],
		[Sku].[BillingFrequency],
		[Sku].[BlockBasedOn],
		[Sku].[BlockSize],
		[Sku].[Description],
		[Sku].[PromoCostPerBlock],
		[Sku].[PromoDeadline]
	FROM [Billing].[Product] 
	LEFT JOIN [Billing].[Sku]
	WITH (NOLOCK) 
	ON [Product].[ProductId] = [Sku].[ProductId]
	WHERE ([Product].[IsActive] = 1 AND [Sku].[IsActive] = 1)
	ORDER BY [Product].[ProductId]
GO
PRINT N'Altering [Billing].[GetProductById]...';


GO
ALTER PROCEDURE [Billing].[GetProductById]
	@ProductId INT 
AS
	SET NOCOUNT ON;
	SELECT [Product].[ProductName], [Product].[ProductId], [Product].[Description], [Product].[AreaUrl]
	  FROM [Billing].[Product] WITH (NOLOCK) WHERE [ProductId] = @ProductId
GO
PRINT N'Altering [Billing].[GetProductIdByName]...';


GO
ALTER PROCEDURE [Billing].[GetProductIdByName]
	@ProductName NVARCHAR(128)
AS
	SET NOCOUNT ON;
	SELECT [ProductId] FROM [Billing].[Product] WITH (NOLOCK) WHERE [ProductName] = @ProductName;
GO
PRINT N'Altering [Billing].[GetProductList]...';


GO
ALTER PROCEDURE [Billing].[GetProductList]
AS
	SET NOCOUNT ON;
	SELECT
		[Product].[ProductId],
		[Product].[ProductName],
		[Product].[Description]
	FROM [Billing].[Product] WITH (NOLOCK) 
	WHERE [IsActive] = 1
	ORDER BY [Product].[ProductName]
GO
PRINT N'Altering [Billing].[GetSkuById]...';


GO
ALTER PROCEDURE [Billing].[GetSkuById]
	@SkuId INT
AS
	SET NOCOUNT ON;
	SELECT [SkuId]
      ,[ProductId]
      ,[SkuName]
      ,[CostPerBlock]
      ,[UserLimit]
      ,[BillingFrequency]
	  ,[BlockBasedOn]
      ,[BlockSize]
	  ,[Description]
      ,[PromoCostPerBlock]
      ,[PromoDeadline]
      ,[IsActive]
       FROM [Billing].[Sku] WITH (NOLOCK) WHERE [SkuId] = @SkuId
GO
PRINT N'Altering [Crm].[CreateCustomerInfo]...';


GO
ALTER PROCEDURE [Crm].[CreateCustomerInfo]
	@CustomerName NVARCHAR(32),
    @Address NVARCHAR(100),
    @City NVARCHAR(100), 
    @State NVARCHAR(100), 
    @Country NVARCHAR(100), 
    @PostalCode NVARCHAR(50),
	@ContactEmail NVARCHAR(384), 
    @ContactPhoneNumber VARCHAR(50),
	@FaxNumber VARCHAR(50),
	@Website NVARCHAR(50),
	@EIN NVARCHAR(50),
	@OrganizationId INT,
	@CustomerOrgId NVARCHAR(16),
	@retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @AddressId INT

	IF EXISTS (
		SELECT * FROM [Crm].[Customer] WITH (NOLOCK)
		WHERE [CustomerOrgId] = @CustomerOrgId
	)
	BEGIN
		-- CustomerOrgId is not unique
		SET @retId = -1;
	END
	ELSE
	BEGIN
		BEGIN TRANSACTION
			INSERT INTO [Lookup].[Address]
				([Address1],
				[City],
				[StateId],
				[CountryId],
				[PostalCode])
			VALUES (@Address,
				@City,
				(SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [StateName] = @State),
				(SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [CountryName] = @Country),
				@PostalCode)

			SET @AddressId = SCOPE_IDENTITY();

			-- Create customer
			INSERT INTO [Crm].[Customer] 
				([CustomerName], 
				[AddressId],
				[ContactEmail], 
				[ContactPhoneNumber], 
				[FaxNumber], 
				[Website], 
				[EIN], 
				[OrganizationId], 
				[CustomerOrgId])
			VALUES (@CustomerName, 
				@AddressId,
				@ContactEmail, 
				@ContactPhoneNumber, 
				@FaxNumber, 
				@Website, 
				@EIN, 
				@OrganizationId, 
				@CustomerOrgId);
			SET @retId = SCOPE_IDENTITY();
		COMMIT TRANSACTION		
	END
	SELECT @retId;
END
GO
PRINT N'Altering [Crm].[GetCustomerAndCountries]...';


GO
ALTER PROCEDURE [Crm].[GetCustomerAndCountries]
	@CustomerId INT
AS
BEGIN

	DECLARE @AddressId AS INT
	
	SET NOCOUNT ON;
	SELECT [Customer].[CustomerId],
		   [Customer].[CustomerName],
		   [Customer].[AddressId],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CustomerCreatedUtc],
		   [Customer].[OrganizationId],
		   [Customer].[CustomerOrgId]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [CustomerId] = @CustomerId
	
	SELECT [CountryName] FROM [Lookup].[Country] WITH (NOLOCK);

	SET @AddressId = (SELECT m.AddressId
					FROM [Crm].[Customer] AS m
					WHERE [CustomerId] = @CustomerId)

	SELECT [Address].[Address1],
		   [Address].[City],
		   [State].[StateName] AS 'State',
		   [Country].[CountryName] AS 'CountryId',
		   [Address].[PostalCode]
	FROM [Lookup].[Address] AS [Address] WITH (NOLOCK)
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [AddressId] = @AddressId
END
GO
PRINT N'Altering [Crm].[GetCustomerInfo]...';


GO
ALTER PROCEDURE [Crm].[GetCustomerInfo]
	@CustomerId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [Customer].[CustomerId],
		   [Customer].[CustomerName],
		   [Address1] AS 'Address',
		   [City],
		   [State].[StateName] AS 'State',
		   [Country].[CountryName] AS 'Country',
		   [PostalCode],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CustomerCreatedUtc],
		   [Customer].[OrganizationId],
		   [Customer].[CustomerOrgId]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [CustomerId] = @CustomerId
END
GO
PRINT N'Altering [Crm].[GetCustomersByOrgId]...';


GO
ALTER PROCEDURE [Crm].[GetCustomersByOrgId]
	@OrgId INT
AS
	BEGIN
	SET NOCOUNT ON;
	SELECT [Customer].[CustomerId],
		   [Customer].[CustomerName],
		   [Address1] AS 'Address',
		   [City],
		   [State].[StateName] AS 'State',
		   [Country].[CountryName] AS 'Country',
		   [PostalCode],
		   [Customer].[ContactEmail],
		   [Customer].[ContactPhoneNumber],
		   [Customer].[FaxNumber],
		   [Customer].[Website],
		   [Customer].[EIN],
		   [Customer].[CustomerCreatedUtc],
		   [Customer].[CustomerOrgId]
	FROM [Crm].[Customer] AS [Customer] WITH (NOLOCK) 
	LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Customer].[OrganizationId] = @OrgId
	AND [Customer].[IsActive] = 1
	ORDER BY [Customer].[CustomerName]
END
GO
PRINT N'Altering [Crm].[GetNextCustIdAndCountries]...';


GO
ALTER PROCEDURE [Crm].[GetNextCustIdAndCountries]
	@OrgId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 1
		[CustomerOrgId]
	FROM [Crm].[Customer] WITH (NOLOCK)
	WHERE [Customer].[OrganizationId] = @OrgId
	ORDER BY [CustomerOrgId] DESC
	
	SELECT [CountryName] FROM [Lookup].[Country] WITH (NOLOCK) ;
END
GO
PRINT N'Altering [Crm].[ReactivateCustomer]...';


GO
ALTER PROCEDURE [Crm].[ReactivateCustomer]
	@customerId INT
AS
BEGIN
	SET NOCOUNT ON;
	-- Retrieve the customer's name
	DECLARE @CustomerName NVARCHAR(384);

	SELECT 
		@CustomerName = [CustomerName] 
	FROM [Crm].[Customer] WITH (NOLOCK)
	WHERE [CustomerId] = @customerId

	IF @CustomerName IS NOT NULL
	BEGIN --Customer found
		UPDATE [Crm].[Customer] SET [IsActive] = 1
		WHERE [Customer].[CustomerId] = @customerId;
	END
	SELECT @CustomerName
END
GO
PRINT N'Altering [Crm].[UpdateCustomerInfo]...';


GO
ALTER PROCEDURE [Crm].[UpdateCustomerInfo]
	@CustomerId INT,
	@CustomerName NVARCHAR(50),
	@ContactEmail NVARCHAR(384),
	@AddressId INT,
    @Address NVARCHAR(100), 
    @City NVARCHAR(100), 
    @State NVARCHAR(100), 
    @Country NVARCHAR(100), 
    @PostalCode NVARCHAR(50),
    @ContactPhoneNumber VARCHAR(50),
	@FaxNumber VARCHAR(50),
	@Website NVARCHAR(50),
	@EIN NVARCHAR(50),
	@OrgId NVARCHAR(16),
	@retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS (
		SELECT * FROM [Crm].[Customer] WITH (NOLOCK)
		WHERE [CustomerOrgId] = @OrgId
		AND [IsActive] = 1
		AND [CustomerId] != @CustomerId
	)
	BEGIN
		-- new CustomerOrgId is taken by a different Customer
		SET @retId = -1;
	END
	ELSE
	BEGIN
		BEGIN TRANSACTION
			UPDATE [Lookup].[Address]
			SET [Address1] = @Address,
				[City] = @City,
				[StateId] = (SELECT [StateId] FROM [Lookup].[State] WHERE [StateName] = @State), 
				[CountryId] = (SELECT [CountryId] FROM [Lookup].[Country] WHERE [CountryName] = @Country), 
				[PostalCode] = @PostalCode
			WHERE [AddressId] = @AddressId

			-- update customer
			UPDATE [Crm].[Customer]
			SET [CustomerName] = @CustomerName,
				[ContactEmail] = @ContactEmail,
				[ContactPhoneNumber] = @ContactPhoneNumber, 
				[FaxNumber] = @FaxNumber,
				[Website] = @Website,
				[EIN] = @EIN,
				[CustomerOrgId] = @OrgId
			WHERE [CustomerId] = @CustomerId 
				AND [IsActive] = 1
			SET @retId = 1;
		COMMIT TRANSACTION		
	END
	SELECT @retId;
END
GO
PRINT N'Altering [Expense].[GetExpenseItemsByAccountId]...';


GO
ALTER PROCEDURE [Expense].[GetExpenseItemsByAccountId]
	@accountId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[ExpenseItemId],
		[ItemDescription],
		[TransactionDate],
		[Amount],
		[ExpenseReportId],
		[AccountId],
		[IsBillableToCustomer],
		[ExpenseItemCreatedUtc],
		[ExpenseItemModifiedUtc]
	FROM [Expense].[ExpenseItem] WITH (NOLOCK)
	WHERE AccountId = @accountId
END
GO
PRINT N'Altering [Expense].[GetExpenseItemsByExpenseItemId]...';


GO
ALTER PROCEDURE [Expense].[GetExpenseItemsByExpenseItemId]
	@expenseItemId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[ExpenseItemId],
		[ItemDescription],
		[TransactionDate],
		[Amount],
		[ExpenseReportId],
		[AccountId],
		[IsBillableToCustomer],
		[ExpenseItemCreatedUtc],
		[ExpenseItemModifiedUtc]
	FROM [Expense].[ExpenseItem] WITH (NOLOCK)
	WHERE ExpenseItemId = @expenseItemId
END
GO
PRINT N'Altering [Expense].[GetExpenseReportByExpenseReportId]...';


GO
ALTER PROCEDURE [Expense].[GetExpenseReportByExpenseReportId]
	@expenseReportId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[ExpenseReportId],
		[ReportTitle],
		[ReportDate],
		[OrganizationId],
		[SubmittedById],
		[ReportStatus],
		[BusinessJustification],
		[ExpenseReportCreatedUtc],
		[ExpenseReportModifiedUtc]
	FROM [Expense].[ExpenseReport] WITH (NOLOCK)
	WHERE ExpenseReportId = @expenseReportId
END
GO
PRINT N'Altering [Expense].[GetExpenseReportsByOrganizationId]...';


GO
ALTER PROCEDURE [Expense].[GetExpenseReportsByOrganizationId]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[ExpenseReportId],
		[ReportTitle],
		[ReportDate],
		[OrganizationId],
		[SubmittedById],
		[ReportStatus],
		[BusinessJustification],
		[ExpenseReportCreatedUtc],
		[ExpenseReportModifiedUtc]
	FROM [Expense].[ExpenseReport] WITH (NOLOCK)
	WHERE OrganizationId = @organizationId
END
GO
PRINT N'Altering [Expense].[GetExpenseReportsBySubmittedById]...';


GO
ALTER PROCEDURE [Expense].[GetExpenseReportsBySubmittedById]
	@submittedById INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[ExpenseReportId],
		[ReportTitle],
		[ReportDate],
		[OrganizationId],
		[SubmittedById],
		[ReportStatus],
		[BusinessJustification],
		[ExpenseReportCreatedUtc],
		[ExpenseReportModifiedUtc]
	FROM [Expense].[ExpenseReport] WITH (NOLOCK)
	WHERE SubmittedById = @submittedById
END
GO
PRINT N'Altering [Hrm].[CreatePayClass]...';


GO
ALTER PROCEDURE [Hrm].[CreatePayClass]
	@PayClassName NVARCHAR(50),
	@OrganizationId INT
AS
	SET NOCOUNT ON;
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId])
	VALUES (@PayClassName, @OrganizationId);
	SELECT SCOPE_IDENTITY();
GO
PRINT N'Altering [Hrm].[GetPayClasses]...';


GO
ALTER PROCEDURE [Hrm].[GetPayClasses]
	@OrganizationId INT = 0
AS
	SET NOCOUNT ON;
	IF(SELECT COUNT(*) FROM [Hrm].[PayClass] WHERE [OrganizationId] = @OrganizationId) > 0
		SELECT [PayClassId], [PayClassName], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = @OrganizationId;
	ELSE
		SELECT [PayClassId], [PayClassName], [OrganizationId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [OrganizationId] = 0;
GO
PRINT N'Altering [Lookup].[GetStatesByCountry]...';


GO
ALTER PROCEDURE [Lookup].[GetStatesByCountry]
	@CountryName NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	IF ((SELECT Count(*) FROM [Lookup].[State] WITH (NOLOCK) 
			INNER JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryCode] = [State].[CountryCode]
			WHERE [Country].[CountryName] = @CountryName) <> 0)

			SELECT [State].[StateName]
			FROM [Lookup].[State] WITH (NOLOCK) 
			INNER JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryCode] = [State].[CountryCode]
			WHERE [Country].[CountryName] = @CountryName
			ORDER BY [State].[StateName]
		
	ELSE
		SELECT @CountryName
			 
END
GO
PRINT N'Creating [Auth].[CreateOrganization]...';


GO
CREATE PROCEDURE [Auth].[CreateOrganization]
	@organizationName NVARCHAR(100),
	@siteUrl NVARCHAR(100),
	@address NVARCHAR(100),
	@city NVARCHAR(100),
	@state NVARCHAR(100),
	@country NVARCHAR(100),
	@postalCode NVARCHAR(50),
	@phoneNumber VARCHAR(50),
	@faxNumber VARCHAR(50),
	@subdomain NVARCHAR(40)
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRANSACTION
		-- Create Address
		INSERT INTO [Lookup].[Address]
				([Address1],
				[City],
				[StateId],
				[CountryId],
				[PostalCode])
		VALUES(@address,
				@city,
				(SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [StateName] = @state),
				(SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [CountryName] = @country),
				@postalCode);

		-- Create org
		INSERT INTO [Auth].[Organization] 
				([OrganizationName],
				[SiteUrl],
				[AddressId],
				[PhoneNumber],
				[FaxNumber],
				[Subdomain])
		VALUES (@organizationName,
				@siteUrl,
				SCOPE_IDENTITY(),
				@phoneNumber,
				@faxNumber,
				@subdomain);
	COMMIT TRANSACTION

	-- return the new organization id
	SELECT IDENT_CURRENT('[Auth].[Organization]');
END
GO
PRINT N'Creating [Auth].[CreateOrganizationUser]...';


GO
CREATE PROCEDURE [Auth].[CreateOrganizationUser]
	@userId INT,
	@organizationId INT,
	@roleId INT,
	@employeeId NVARCHAR(128) = NULL
AS
BEGIN
	INSERT INTO [Auth].[OrganizationUser]
		([UserId],
		[OrganizationId],
		[OrganizationRoleId],
		[EmployeeId])
	VALUES
		(@userId,
		@organizationId,
		@roleId,
		@employeeId);
END
GO
PRINT N'Creating [Auth].[DeleteOrganizationUsers]...';


GO
CREATE PROCEDURE [Auth].[DeleteOrganizationUsers]
	@organizationId	INT,
	@userIds [Auth].[UserTable] READONLY
AS
	DELETE FROM [Auth].[OrganizationUser]
	WHERE [UserId] IN (SELECT [userId] FROM @userIds) AND [OrganizationId] = @organizationId
GO
PRINT N'Creating [Auth].[GetOrganizationOwnerEmails]...';


GO
CREATE PROCEDURE [Auth].[GetOrganizationOwnerEmails]
	@OrganizationId INT
AS
BEGIN
	SELECT [User].[Email]
	FROM [Auth].[OrganizationUser]
	JOIN [Auth].[User]
	ON [OrganizationUser].[UserId] = [User].[UserId]
	WHERE [OrganizationRoleId] = 2 AND @OrganizationId = [OrganizationId]
End
GO
PRINT N'Creating [Auth].[GetUserInvitationsByInviteId]...';


GO
CREATE PROCEDURE [Auth].[GetUserInvitationsByInviteId]
	@InviteId INT
AS
	SET NOCOUNT ON;
	SELECT 
		[InvitationId],
		[Email],
		[FirstName],
		[LastName], 
		[DateOfBirth], 
		[OrganizationId], 
		[AccessCode], 
		[Invitation].[OrganizationRoleId],
		[OrganizationRoleName] AS [OrganizationRoleName],
		[EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	LEFT JOIN [Auth].[OrganizationRole] WITH (NOLOCK) ON [OrganizationRole].[OrganizationRoleId] = [Invitation].[OrganizationRoleId]
	WHERE [InvitationId] = @InviteId AND [IsActive] = 1
GO
PRINT N'Creating [Auth].[UpdateOrganization]...';


GO
CREATE PROCEDURE [Auth].[UpdateOrganization]
	@organizationId INT,
	@organizationName NVARCHAR(100),
	@siteUrl NVARCHAR(100),
	@address NVARCHAR(100), 
	@city NVARCHAR(100), 
	@state NVARCHAR(100), 
	@country NVARCHAR(100), 
	@postalCode NVARCHAR(50),
	@phoneNumber VARCHAR (50),
	@faxNumber VARCHAR (50),
	@subdomainName NVARCHAR (40),
	@addressId INT
AS
BEGIN
	UPDATE [Lookup].[Address]
	SET [Address1] = @address,
		[City] = @city,
		[StateId] = (SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [StateName] = @state),
		[CountryId] = (SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [CountryName] = @country),
		[PostalCode] = @postalCode
	WHERE [AddressId] = @addressId

	UPDATE [Auth].[Organization]
	SET [OrganizationName] = @organizationName,
		[SiteUrl] = @siteUrl,
		[PhoneNumber] = @phoneNumber,
		[FaxNumber] = @faxNumber,
		[Subdomain] = @subdomainName
	WHERE [OrganizationId] = @organizationId
END
GO
PRINT N'Creating [Auth].[UpdateOrganizationUsersRole]...';


GO
CREATE PROCEDURE [Auth].[UpdateOrganizationUsersRole]
	@organizationId INT,
	@userIds [Auth].[UserTable] READONLY,
	@organizationRole INT
AS
BEGIN
	UPDATE [Auth].[OrganizationUser]
	SET [OrganizationRoleId] = @organizationRole
	WHERE [UserId] IN (SELECT [userId] FROM @userIds) AND [OrganizationId] = @organizationId
END
GO
PRINT N'Creating [Billing].[CreateStripeOrganizationCustomer]...';


GO
CREATE PROCEDURE [Billing].[CreateStripeOrganizationCustomer]
	@organizationId INT,
	@userId INT,
	@customerId NVARCHAR(50),
	@skuId INT,
	@description NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRANSACTION
		INSERT INTO [Billing].[StripeOrganizationCustomer] ([OrganizationId], [StripeTokenCustId], [IsActive])
		VALUES (@organizationId, @customerId, 1)

		INSERT INTO [Billing].[BillingHistory] ([Date], [Description], [OrganizationId], [UserId], [SkuId])
		VALUES (SYSDATETIME(), @description, @organizationId, @userId, @skuId)
	COMMIT TRANSACTION
END
GO
PRINT N'Creating [Billing].[DeleteSubscriptionUsers]...';


GO
CREATE PROCEDURE [Billing].[DeleteSubscriptionUsers]
	@organizationId INT,
	@userIds [Auth].[UserTable] READONLY,
	@productId INT
AS
BEGIN TRANSACTION
	-- get the subscription id from the productId and orgId provided
	DECLARE @subscriptionId INT;
	SELECT
		@subscriptionId = [SubscriptionId]
	FROM [Billing].[Subscription] WITH (NOLOCK)
	JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	JOIN [Billing].[Product] WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId] -- @productId
	WHERE [Subscription].[OrganizationId] = @organizationId AND [Product].[ProductId] = @productId AND [Subscription].[IsActive] = 1


	DELETE [Billing].[SubscriptionUser] 
	WHERE [SubscriptionId] = @subscriptionId AND [UserId] IN (SELECT [userId] FROM @userIds)
COMMIT TRANSACTION
GO
PRINT N'Creating [Billing].[UpdateSubscriptionUserRoles]...';


GO
CREATE PROCEDURE [Billing].[UpdateSubscriptionUserRoles]
	@organizationId INT,
	@userIds [Auth].[UserTable] READONLY,
	@productRoleId INT,
	@productId INT
AS
BEGIN TRANSACTION
	-- get the subscription id from the productId and orgId provided
	DECLARE @subscriptionId INT;
	SELECT
		@subscriptionId = [Subscription].[SubscriptionId]
	FROM [Billing].[Subscription] WITH (NOLOCK)
	JOIN [Billing].[Sku] WITH (NOLOCK) ON [Sku].[SkuId] = [Subscription].[SkuId]
	JOIN [Billing].[Product] WITH (NOLOCK) ON [Product].[ProductId] = [Sku].[ProductId] -- @productId
	WHERE [Subscription].[OrganizationId] = @organizationId AND [Product].[ProductId] = @productId AND [Subscription].[IsActive] = 1

	-- Update users already in subscription
	UPDATE [Billing].[SubscriptionUser] 
	SET [ProductRoleId] = @productRoleId
	WHERE [SubscriptionId] = @subscriptionId AND [UserId] IN (SELECT [userId] FROM @userIds)

	-- return updated users count
	SELECT @@ROWCOUNT

	-- Select users from @userIds that are not already subscribed
	DECLARE @addingUsers TABLE ([userId] INT);
	INSERT INTO @addingUsers ([userId])
		SELECT [UID].[userId]
		FROM @userIds AS [UID] LEFT OUTER JOIN (
			SELECT [SubscriptionId], [UserId]
			FROM [Billing].[SubscriptionUser] WITH (NOLOCK)
			WHERE [SubscriptionId] = @subscriptionId)
				[SubUsers] ON [SubUsers].[UserId] = [UID].[userId]
		WHERE [SubscriptionId] IS NULL
			
	-- Add the new users
	DECLARE @subAndRole TABLE ([SubId] INT, [TTRoleId] INT);
	INSERT INTO @subAndRole ([SubId], [TTRoleId]) VALUES (@subscriptionId, @productRoleId)

	INSERT INTO [Billing].[SubscriptionUser] ([SubscriptionId], [UserId], [ProductRoleId])
	SELECT [SubId], [userId], [TTRoleId]
	FROM @addingUsers CROSS JOIN @subAndRole

	-- return added users count
	SELECT COUNT(*) FROM @addingUsers
COMMIT TRANSACTION
GO
PRINT N'Creating [Hrm].[CreateDefaultPayClass]...';


GO
CREATE PROCEDURE [Hrm].[CreateDefaultPayClass]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Regular',           @organizationId);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Paid Time Off',     @organizationId);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Unpaid Time Off',   @organizationId);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Holiday',           @organizationId);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Bereavement Leave', @organizationId);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Jury Duty',         @organizationId);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Overtime',          @organizationId);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId]) VALUES ('Other Leave',       @organizationId);
END
GO
PRINT N'Altering [Hrm].[CreateHoliday]...';


GO
ALTER PROCEDURE [Hrm].[CreateHoliday]
	@HolidayName NVARCHAR(50),
	@Date DATE,
	@OrganizationId INT
AS
	SET NOCOUNT ON;
	INSERT INTO [Hrm].[Holiday] ([HolidayName], [Date], [OrganizationId]) VALUES (@HolidayName, @Date, @OrganizationId);
	
	declare 
		@payClassIdForHoliday int;
		
	IF (SELECT COUNT([PayClassId]) FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [PayClassName] = 'Holiday') > 0
		SET @payClassIdForHoliday = (SELECT TOP 1 [PayClassId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [PayClassName] = 'Holiday');
	ELSE
		BEGIN
			EXEC [Hrm].[CreatePayClass] @PayClassName = 'Holiday', @OrganizationId = @OrganizationId
			SET @payClassIdForHoliday = (SELECT TOP 1 [PayClassId] FROM [Hrm].[PayClass] WITH (NOLOCK) WHERE [PayClassName] = 'Holiday');
		END
		
	EXEC [TimeTracker].[CreateBulkTimeEntry]
		@Date = @Date,
		@Duration = 8, 
		@Description = @HolidayName,
		@PayClassId = @payClassIdForHoliday,
		@OrganizationId = @OrganizationId,
		@Overwrite = 0;
SELECT SCOPE_IDENTITY();
GO
PRINT N'Creating [Auth].[SetupOrganization]...';


GO
CREATE PROCEDURE [Auth].[SetupOrganization]
	@userId INT,
	@roleId INT,
	@organizationName NVARCHAR(100),
	@siteUrl NVARCHAR(100),
	@address NVARCHAR(100),
	@city NVARCHAR(100),
	@state NVARCHAR(100),
	@country NVARCHAR(100),
	@postalCode NVARCHAR(50),
	@phoneNumber VARCHAR(50),
	@faxNumber VARCHAR(50),
	@subdomainName NVARCHAR(40),
	@employeeId NVARCHAR(16)
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRANSACTION
		-- Create organization
		EXEC [Auth].[CreateOrganization] @organizationName, @siteUrl, @address, @city, @state, @country, @postalCode, @phoneNumber, @faxNumber, @subdomainName;

		-- get the new organization id
		DECLARE @organizationId INT = IDENT_CURRENT('[Auth].[Organization]');

		-- Add user to the org
		EXEC [Auth].[CreateOrganizationUser] @userId, @organizationId, @roleId, @employeeId;

		-- Init default pay classes for org
		EXEC [Hrm].[CreateDefaultPayClass] @organizationId;
	COMMIT TRANSACTION

	-- return the new organization id
	SELECT @organizationId;
END
GO
PRINT N'Refreshing [Auth].[DeleteOrg]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[DeleteOrg]';


GO
PRINT N'Refreshing [Auth].[DeleteUserInvitation]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[DeleteUserInvitation]';


GO
PRINT N'Refreshing [Auth].[GetUserInvitationsByUserData]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetUserInvitationsByUserData]';


GO
PRINT N'Refreshing [Auth].[RemoveInvitation]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[RemoveInvitation]';


GO
PRINT N'Refreshing [Auth].[DeleteOrgUser]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[DeleteOrgUser]';


GO
PRINT N'Refreshing [Auth].[GetOrgUserByEmail]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrgUserByEmail]';


GO
PRINT N'Refreshing [Auth].[GetOrgUserEmployeeId]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetOrgUserEmployeeId]';


GO
PRINT N'Refreshing [Pjm].[GetNextProjectIdAndSubUsers]...';


GO
EXECUTE sp_refreshsqlmodule N'[Pjm].[GetNextProjectIdAndSubUsers]';


GO
PRINT N'Refreshing [Auth].[GetPasswordHashFromUserId]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetPasswordHashFromUserId]';


GO
PRINT N'Refreshing [Auth].[GetUsersWithSubscriptionToProductInOrganization]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[GetUsersWithSubscriptionToProductInOrganization]';


GO
PRINT N'Refreshing [Auth].[UpdateUserActiveOrg]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateUserActiveOrg]';


GO
PRINT N'Refreshing [Auth].[UpdateUserActiveSub]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateUserActiveSub]';


GO
PRINT N'Refreshing [Auth].[UpdateUserLanguagePreference]...';


GO
EXECUTE sp_refreshsqlmodule N'[Auth].[UpdateUserLanguagePreference]';


GO
PRINT N'Refreshing [Billing].[UpdateSubscriptionName]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[UpdateSubscriptionName]';


GO
PRINT N'Refreshing [Pjm].[ReactivateProject]...';


GO
EXECUTE sp_refreshsqlmodule N'[Pjm].[ReactivateProject]';


GO
PRINT N'Refreshing [TimeTracker].[UpdateOvertime]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[UpdateOvertime]';


GO
PRINT N'Refreshing [TimeTracker].[UpdateSettings]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[UpdateSettings]';


GO
PRINT N'Refreshing [TimeTracker].[UpdateStartOfWeek]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[UpdateStartOfWeek]';


GO
PRINT N'Refreshing [TimeTracker].[CreateTimeEntry]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[CreateTimeEntry]';


GO
PRINT N'Refreshing [TimeTracker].[DeleteTimeEntry]...';


GO
EXECUTE sp_refreshsqlmodule N'[TimeTracker].[DeleteTimeEntry]';


GO
PRINT N'Refreshing [Billing].[DeleteCustomerSubscription]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[DeleteCustomerSubscription]';


GO
PRINT N'Refreshing [Billing].[DeleteSubPlanAndAddHistory]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[DeleteSubPlanAndAddHistory]';


GO
PRINT N'Refreshing [Billing].[GetSubscriptionPlan]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[GetSubscriptionPlan]';


GO
PRINT N'Refreshing [Billing].[GetSubscriptionPlanPricesByOrg]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[GetSubscriptionPlanPricesByOrg]';


GO
PRINT N'Refreshing [Billing].[UpdateCustomerSubscription]...';


GO
EXECUTE sp_refreshsqlmodule N'[Billing].[UpdateCustomerSubscription]';


GO
-- Refactoring step to update target server with deployed transaction logs

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'e279e838-1485-4ffd-a449-403eddfd0075')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('e279e838-1485-4ffd-a449-403eddfd0075')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'd901f09c-0c58-4748-aecf-4ad1c229cec3')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('d901f09c-0c58-4748-aecf-4ad1c229cec3')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'ce51f5cb-9d8b-4a7b-853c-8fc8b2053742')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('ce51f5cb-9d8b-4a7b-853c-8fc8b2053742')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'd56982a6-c07f-4906-8384-9418fed309e1')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('d56982a6-c07f-4906-8384-9418fed309e1')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'fc38992e-deca-44cf-8bea-6d1e6d5ef351')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('fc38992e-deca-44cf-8bea-6d1e6d5ef351')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '3ad9644e-6ddb-42f8-bba9-4f37e5ead6eb')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('3ad9644e-6ddb-42f8-bba9-4f37e5ead6eb')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'e2d5f599-b8f0-487b-bfa7-d147f7a7ae21')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('e2d5f599-b8f0-487b-bfa7-d147f7a7ae21')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '9e6a781f-e569-4948-9876-383c50bc3183')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('9e6a781f-e569-4948-9876-383c50bc3183')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'a83ff01f-54e5-4ef4-9ae0-3530353217de')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('a83ff01f-54e5-4ef4-9ae0-3530353217de')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'a63a65fd-25c4-4564-9739-f4e8eed1b07c')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('a63a65fd-25c4-4564-9739-f4e8eed1b07c')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '19adb6b7-0b7b-4b3e-8b45-47068691e044')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('19adb6b7-0b7b-4b3e-8b45-47068691e044')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '6f07e028-5275-4b73-ac78-8eb031df5900')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('6f07e028-5275-4b73-ac78-8eb031df5900')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '6db4c03a-8567-487e-94ac-e4a03660f939')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('6db4c03a-8567-487e-94ac-e4a03660f939')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'cc86e587-721d-4924-b0ae-f9be368a3728')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('cc86e587-721d-4924-b0ae-f9be368a3728')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '7512f0b9-df37-401a-9910-7ed407fa20a3')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('7512f0b9-df37-401a-9910-7ed407fa20a3')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '096d1cbd-fc9b-435c-bddc-aa83acb563ae')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('096d1cbd-fc9b-435c-bddc-aa83acb563ae')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'bd89e74e-e33b-4308-9a39-377fb5b94c6f')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('bd89e74e-e33b-4308-9a39-377fb5b94c6f')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '211ebb96-ecbd-45d8-b3fe-81a703a6bb94')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('211ebb96-ecbd-45d8-b3fe-81a703a6bb94')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '7519bf08-52a9-4575-9f57-472044e93083')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('7519bf08-52a9-4575-9f57-472044e93083')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '966e63d4-8b9f-4ada-bf32-bbf044843d07')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('966e63d4-8b9f-4ada-bf32-bbf044843d07')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '5e68ff46-6f6e-4378-870e-4a1d83294bab')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('5e68ff46-6f6e-4378-870e-4a1d83294bab')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '2d66c2ed-1503-4a43-8c7b-85cd9db6afc3')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('2d66c2ed-1503-4a43-8c7b-85cd9db6afc3')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '28d87a44-ab6f-4518-9c2b-a91bbe5b8e6a')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('28d87a44-ab6f-4518-9c2b-a91bbe5b8e6a')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'b64b80b2-dd09-42e0-a52f-7a2b3eaa8c9d')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('b64b80b2-dd09-42e0-a52f-7a2b3eaa8c9d')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'e2ebc197-28fe-4f40-9283-03f3072cd7ef')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('e2ebc197-28fe-4f40-9283-03f3072cd7ef')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '8f896573-4e65-4726-9aa6-3f7001c87755')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('8f896573-4e65-4726-9aa6-3f7001c87755')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'f6a59a61-e520-4701-87f1-484ac030d378')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('f6a59a61-e520-4701-87f1-484ac030d378')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '55937b5b-a45f-47b9-8998-81639d42a514')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('55937b5b-a45f-47b9-8998-81639d42a514')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'ec6c346d-91ca-49ee-b20e-6d1e9e4973c7')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('ec6c346d-91ca-49ee-b20e-6d1e9e4973c7')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '18ebc0f1-fe47-4b2a-ae14-3d688ec0bf59')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('18ebc0f1-fe47-4b2a-ae14-3d688ec0bf59')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '012aa26a-a293-45ff-ab49-366c72759ae5')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('012aa26a-a293-45ff-ab49-366c72759ae5')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '7b8c941a-8ac3-41e7-b812-739d2cbff2e0')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('7b8c941a-8ac3-41e7-b812-739d2cbff2e0')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '87e3eb67-4a6f-4543-bc7a-c88d4ac50b66')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('87e3eb67-4a6f-4543-bc7a-c88d4ac50b66')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '5fdd2352-b43e-4123-b372-017df268fe9b')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('5fdd2352-b43e-4123-b372-017df268fe9b')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '73cc4553-2ee2-45df-8380-255b923f6254')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('73cc4553-2ee2-45df-8380-255b923f6254')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'abe3058c-2402-473d-884c-a2cc825de950')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('abe3058c-2402-473d-884c-a2cc825de950')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '8aefaa49-66c9-4154-8530-613aa19b1ac1')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('8aefaa49-66c9-4154-8530-613aa19b1ac1')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '36cecd4a-2d4a-4cbc-a037-f3c8e238d6f3')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('36cecd4a-2d4a-4cbc-a037-f3c8e238d6f3')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '5ae1c196-02d1-4722-944b-d00a840814b2')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('5ae1c196-02d1-4722-944b-d00a840814b2')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '9e7a3ff2-c3dd-41e0-95fa-9e73129c20ca')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('9e7a3ff2-c3dd-41e0-95fa-9e73129c20ca')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '2f8ee18d-4ed2-4fea-b930-654b1b29b393')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('2f8ee18d-4ed2-4fea-b930-654b1b29b393')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '173ca97f-7ea2-441a-be18-3cf8d69b6ac2')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('173ca97f-7ea2-441a-be18-3cf8d69b6ac2')

GO

GO
PRINT N'Checking existing data against newly created constraints';

GO
ALTER TABLE [Auth].[Logging] WITH CHECK CHECK CONSTRAINT [FK_Logging_User];

ALTER TABLE [Auth].[OrganizationUser] WITH CHECK CHECK CONSTRAINT [FK_OrganizationUser_User];

ALTER TABLE [Auth].[User] WITH CHECK CHECK CONSTRAINT [FK_User_Language];

ALTER TABLE [Auth].[User] WITH CHECK CHECK CONSTRAINT [FK_User_Organization];

ALTER TABLE [Billing].[BillingHistory] WITH CHECK CHECK CONSTRAINT [FK_BillingHistory_User];

ALTER TABLE [Billing].[SubscriptionUser] WITH CHECK CHECK CONSTRAINT [FK_SubscriptionUser_User];

ALTER TABLE [Expense].[ExpenseReport] WITH CHECK CHECK CONSTRAINT [FK_ExpenseReport_User];

ALTER TABLE [Pjm].[ProjectUser] WITH CHECK CHECK CONSTRAINT [FK_ProjectUser_User];

ALTER TABLE [TimeTracker].[TimeEntry] WITH CHECK CHECK CONSTRAINT [FK_TimeEntry_User];

ALTER TABLE [Pjm].[Project] WITH CHECK CHECK CONSTRAINT [FK_Project_Customer];

ALTER TABLE [Pjm].[ProjectUser] WITH CHECK CHECK CONSTRAINT [FK_ProjectUser_Project];

ALTER TABLE [TimeTracker].[TimeEntry] WITH CHECK CHECK CONSTRAINT [FK_TimeEntry_Project];

ALTER TABLE [TimeTracker].[Setting] WITH CHECK CHECK CONSTRAINT [FK_Settings_Organization];

ALTER TABLE [Billing].[StripeCustomerSubscriptionPlan] WITH CHECK CHECK CONSTRAINT [FK_StripeCustomerSubscriptionPlan_Organization];

ALTER TABLE [Billing].[StripeCustomerSubscriptionPlan] WITH CHECK CHECK CONSTRAINT [FK_StripeCustomerSubscriptionPlan_Product];

ALTER TABLE [Lookup].[OrganizationLocation] WITH CHECK CHECK CONSTRAINT [FK_OrganizationLocation_Address];

ALTER TABLE [Lookup].[OrganizationLocation] WITH CHECK CHECK CONSTRAINT [FK_OrganizationLocation_Organization];

ALTER TABLE [Billing].[Subscription] WITH CHECK CHECK CONSTRAINT [FK_Subscription_Organization];

ALTER TABLE [Billing].[SubscriptionUser] WITH CHECK CHECK CONSTRAINT [FK_SubscriptionUser_Subscription];

ALTER TABLE [Auth].[User] WITH CHECK CHECK CONSTRAINT [FK_User_Subscription];


GO
PRINT N'Update complete.';


GO

--need to add delete default payclass for orgId = 0

--Delete Project Defaults
alter table [Pjm].[Project]
nocheck constraint FK_Project_Customer

DELETE FROM [Pjm].[Project]
WHERE [CustomerId] = 0;

alter table [Pjm].[Project]
check constraint FK_Project_Customer

--Delete Customer Defaults
alter table [Crm].[Customer]
nocheck constraint FK_Customer_Organization

DELETE FROM [Crm].[Customer]
WHERE [CustomerId] = 0;

alter table [Crm].[Customer]
check constraint FK_Customer_Organization

alter table [Hrm].[Holiday]
nocheck constraint FK_Holiday_Organization

--Delete Holiday Defaults
DELETE FROM [Hrm].[Holiday]
WHERE [OrganizationId] = 0;

alter table [Hrm].[Holiday]
check constraint FK_Holiday_Organization

--Delete default pay classes 
DELETE FROM [Hrm].[PayClass]
WHERE [OrganizationId] = 0;

--Delete Default Organization
DELETE FROM [Auth].[Organization]
WHERE [OrganizationId] = 0;