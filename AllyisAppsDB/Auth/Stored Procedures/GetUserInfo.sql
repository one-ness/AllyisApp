﻿CREATE PROCEDURE [Auth].[GetUserInfo]
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [User].[UserId],
		   [User].[FirstName],
		   [User].[LastName],
		   [User].[DateOfBirth],
		   [User].[Address],
		   [User].[City],
		   [State].[Name] AS 'State',
		   [Country].[Name] AS 'Country',
		   [User].[PostalCode],
		   [User].[Email],
		   [User].[PhoneNumber],
		   [User].[LastSubscriptionId],
		   [User].[ActiveOrganizationId],
		   [User].[EmailConfirmed],
		   [User].[EmailConfirmationCode],
		   [LanguagePreference]
	FROM [Auth].[User]
	WITH (NOLOCK)
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [User].[Country]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [User].[State]
	WHERE [UserId] = @UserId;
END