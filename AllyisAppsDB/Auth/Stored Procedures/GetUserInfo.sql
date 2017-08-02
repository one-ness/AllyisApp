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
		   [State].[Name] AS 'State',
		   [Country].[Name] AS 'CountryId',
		   [Address].[PostalCode]
	FROM [Lookup].[Address] AS [Address] WITH (NOLOCK)
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [AddressId] = @AddressId
END