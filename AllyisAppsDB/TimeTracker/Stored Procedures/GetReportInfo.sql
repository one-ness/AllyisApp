CREATE PROCEDURE [TimeTracker].[GetReportInfo]
    @orgId INT,
    @subscriptionId INT
AS

SET NOCOUNT ON

   SELECT [Customer].[CustomerId],
          [Customer].[CustomerName],
          [Customer].[AddressId],
          [Address1] AS 'Address',
          [City],
          [State].[StateName] AS 'State',
          [State].[StateId],
          [Country].[CountryName] AS 'Country',
          [Country].[CountryCode],
          [PostalCode],
          [Customer].[ContactEmail],
          [Customer].[ContactPhoneNumber],
          [Customer].[FaxNumber],
          [Customer].[Website],
          [Customer].[EIN],
          [Customer].[CustomerCreatedUtc],
          [Customer].[CustomerCode],
          [Customer].[OrganizationId],
          [Customer].[IsActive],
		  [Customer].[ActiveProjectCount],
		  [Customer].[ProjectCount]
     FROM [Crm].[Customer]   WITH (NOLOCK)
LEFT JOIN [Lookup].[Address] WITH (NOLOCK) ON [Address].[AddressId] = [Customer].[AddressId]
LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
LEFT JOIN [Lookup].[State]   WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
    WHERE [Customer].[OrganizationId] = @orgId
 ORDER BY [Customer].[CustomerName]

SELECT [Project].[ProjectId],
       [Project].[CustomerId],
       [Customer].[OrganizationId],
       [Project].[ProjectCreatedUtc],
       [Project].[ProjectName] AS [ProjectName],
       [Project].[ProjectCode],
       [Organization].[OrganizationName] AS [OrganizationName],
       [Customer].[CustomerName] AS [CustomerName],
       [Customer].[CustomerCode],
       [Customer].[IsActive] AS [IsCustomerActive],
       [Project].[IsHourly] AS [IsHourly]
  FROM [Auth].[Organization] WITH (NOLOCK) 
  JOIN [Crm].[Customer]      WITH (NOLOCK) ON ([Customer].[OrganizationId] = [Organization].[OrganizationId])
  JOIN [Pjm].[Project]       WITH (NOLOCK) ON [Project].[CustomerId] = [Customer].[CustomerId]
 WHERE [Organization].[OrganizationId] = @orgId

SELECT [User].[UserId],
       [User].[FirstName],
       [User].[LastName],
       [SubscriptionUser].[ProductRoleId]
  FROM [Auth].[User]                WITH (NOLOCK)
  JOIN [Auth].[OrganizationUser]    WITH (NOLOCK) ON [OrganizationUser].[UserId] = [User].[UserId]
  JOIN [Billing].[SubscriptionUser] WITH (NOLOCK) ON [SubscriptionUser].[UserId] = [User].[UserId]
 WHERE [OrganizationUser].[OrganizationId] = @orgId
   AND [SubscriptionUser].[SubscriptionId] = @subscriptionId
   AND [SubscriptionUser].[ProductRoleId] IS NOT NULL