CREATE PROCEDURE [Auth].[GetUserOrgsAndInvitationInfo]
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
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [UserId] = @userId;

	SELECT [Auth].[Organization].[OrganizationId],
		   [Organization].[OrganizationName],
		   [SiteUrl],
		   [Address1] AS 'Address',
		   [Organization].[AddressId],
		   [City],
		   [Country].[CountryName] AS 'CountryName',
		   [State].[StateName] AS 'StateName',
		   [PostalCode],
		   [PhoneNumber],
		   [FaxNumber],
		   [Subdomain],
		   [Organization].[OrganizationCreatedUtc]
	FROM [Auth].[Organization] WITH (NOLOCK)
	RIGHT JOIN [Auth].[OrganizationUser]	WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
	LEFT JOIN [Lookup].[Address]	WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryCode] = [Address].[CountryCode]
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

	DECLARE @addressId INT
	SET @addressId = (SELECT m.AddressId
				FROM [Auth].[User] AS m
				WHERE [UserId] = @userId)

	SELECT [Address].[Address1],
		   [Address].[City],
		   [State].[StateName] AS 'State',
		   [Country].[CountryName] AS 'Country',
		   [Address].[PostalCode]
	FROM [Lookup].[Address] AS [Address] WITH (NOLOCK)
	LEFT JOIN [Lookup].[Country] WITH (NOLOCK) ON [Country].CountryCode = [Address].CountryCode
	LEFT JOIN [Lookup].[State] WITH (NOLOCK) ON [State].[StateId] = [Address].[StateId]
	WHERE [AddressId] = @addressId
END