CREATE PROCEDURE [Auth].[GetUserFromEmail]
	@email NVARCHAR(384)
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
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [Email] = @email;
END