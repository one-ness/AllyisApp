CREATE PROCEDURE [Auth].[GetUserFromEmail]
	@Email NVARCHAR(384)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [UserId]
		,[FirstName]
		,[LastName]
		,[DateOfBirth]
		,[Address]
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
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [User].[Country]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [User].[State]
	WHERE [Email] = @Email;
END