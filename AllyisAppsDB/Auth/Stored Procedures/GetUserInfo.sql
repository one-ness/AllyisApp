CREATE PROCEDURE [Auth].[GetUserInfo]
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [User].[UserId],
		   [User].[FirstName],
		   [User].[LastName],
		   [User].[DateOfBirth],
		   [User].[AddressId],
		   [Address].[Address1] AS 'Address',
		   [Address].[City],
		   [State].[Name] AS 'State',
		   [Country].[Name] AS 'Country',
		   [Address].[PostalCode],
		   [User].[Email],
		   [User].[PhoneNumber],
		   [User].[LastUsedSubscriptionId],
		   [User].[LastUsedOrganizationId],
		   [User].[EmailConfirmed],
		   [User].[EmailConfirmationCode],
		   [LanguagePreference]
	FROM [Auth].[User]
	WITH (NOLOCK)
	JOIN [Lookup].[Address]		WITH (NOLOCK) ON [Address].[AddressId] = [User].[AddressId]
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[State]
	WHERE [UserId] = @UserId;
END