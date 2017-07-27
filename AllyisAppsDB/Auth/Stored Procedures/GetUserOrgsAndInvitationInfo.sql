CREATE PROCEDURE [Auth].[GetUserOrgsAndInvitationInfo]
	@userId int
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
			[LanguagePreference]
	FROM [Auth].[User]
	WITH (NOLOCK)
	LEFT JOIN [Lookup].[Country]	WITH (NOLOCK) ON [Country].[CountryId] = [User].[Country]
	LEFT JOIN [Lookup].[State]		WITH (NOLOCK) ON [State].[StateId] = [User].[State]
	WHERE [UserId] = @userId;

	SELECT [Auth].[Organization].[OrganizationId],
		   [Organization].[Name],
		   [SiteUrl],
		   [Address1] AS 'Address',
		   [Address].[City],
		   [State].[Name] AS 'State',
		   [Country].[Name] AS 'Country',
		   [Address].[PostalCode],
		   [PhoneNumber],
		   [FaxNumber],
		   [Subdomain],
		   [Organization].[CreatedUtc]

	FROM [Auth].[Organization] WITH (NOLOCK)
	RIGHT JOIN [Auth].[OrganizationUser]	WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
	LEFT JOIN [Lookup].[Address]			WITH (NOLOCK) ON [Address].[AddressId] = [Organization].[AddressId]
	LEFT JOIN [Lookup].[Country]			WITH (NOLOCK) ON [Country].[CountryId] = [Address].[CountryId]
	LEFT JOIN [Lookup].[State]				WITH (NOLOCK) ON [State].[StateId] = [Address].[State]
	WHERE [OrganizationUser].[UserId] = @userId 
		  AND [Auth].[Organization].[IsActive] = 1
	ORDER BY [OrganizationUser].[OrgRoleId] DESC, [Organization].[Name]

	SELECT 
		[InvitationId], 
		[Invitation].[Email], 
		[Invitation].[FirstName], 
		[Invitation].[LastName], 
		[Invitation].[DateOfBirth], 
		[Invitation].[OrganizationId],
		[Organization].[Name] AS 'OrganizationName',
		[AccessCode], 
		[OrgRoleId],
		[EmployeeId] 
	FROM [Auth].[User] WITH (NOLOCK)
	LEFT JOIN [Auth].[Invitation] WITH (NOLOCK) ON [User].[Email] = [Invitation].[Email]
	LEFT JOIN [Auth].[Organization] WITH (NOLOCK) ON [Invitation].[OrganizationId] = [Organization].[OrganizationId]

	WHERE [User].[UserId] = @userId AND [Invitation].[IsActive] = 1
END