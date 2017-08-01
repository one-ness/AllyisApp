CREATE PROCEDURE [Auth].[GetUserFromEmail]
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
		,[State].[Name] AS 'State'
		,[Country].[Name] AS 'Country'
		,[PostalCode]
		,[Email]
		,[PhoneNumber]
		,[PhoneExtension]
		,[LastUsedSubscriptionId]
		,[LastUsedOrganizationId]
		,[PasswordHash]
		,[LanguagePreference]
	FROM [Auth].[User]
	WITH (NOLOCK)
	LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [User].[AddressId]
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[State]
	WHERE [Email] = @Email;
END