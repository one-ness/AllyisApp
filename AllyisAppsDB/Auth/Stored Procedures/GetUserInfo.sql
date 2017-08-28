CREATE PROCEDURE [Auth].[GetUserInfo]
	@userId INT
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
		   [User].[IsEmailConfirmed],
		   [User].[EmailConfirmationCode],
		   [PreferredLanguageId]
	FROM [Auth].[User]
	WITH (NOLOCK)
	WHERE [UserId] = @userId;

	DECLARE @addressId INT
	SET @addressId = (SELECT U.AddressId
					 FROM [Auth].[User] AS U
					 WHERE [UserId] = @userId)
	if(@addressId IS NOT NULL)
	BEGIN
		SELECT [Address].[AddressId],
			   [Address].[Address1],
			   [Address].[City],
			   [State].[StateName] AS 'State',
			   [Address].[StateId],
			   [Address].[CountryCode],
			   [Country].[CountryName] AS 'Country',
			   [Address].[PostalCode]
		FROM [Lookup].[Address] AS [Address] WITH (NOLOCK)
		LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
		LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
		WHERE [AddressId] = @addressId
	END

END