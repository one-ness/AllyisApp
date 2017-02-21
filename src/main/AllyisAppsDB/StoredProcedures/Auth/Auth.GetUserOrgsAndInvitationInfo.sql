CREATE PROCEDURE [Auth].[GetUserOrgsAndInvitationInfo]
	@userId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [User].[UserId],
			[User].[FirstName],
			[User].[LastName],
			[User].[UserName],
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
	WHERE [UserId] = @UserId;

	SELECT [Auth].[Organization].[OrganizationId]
		  ,[Organization].[Name]
		  ,[SiteUrl]
		  ,[Address]
		  ,[City]
		  ,[State].[Name] AS 'State'
		  ,[Country].[Name] AS 'Country'
		  ,[PostalCode]
		  ,[PhoneNumber]
		  ,[FaxNumber]
		  ,[Subdomain]
		  ,[Organization].[CreatedUTC]
	FROM [Auth].[Organization] WITH (NOLOCK)
	RIGHT JOIN [Auth].[OrganizationUser]	WITH (NOLOCK) ON [OrganizationUser].[OrganizationId] = [Organization].[OrganizationId]
	LEFT JOIN [Lookup].[Country]			WITH (NOLOCK) ON [Country].[CountryId] = [Organization].[Country]
	LEFT JOIN [Lookup].[State]				WITH (NOLOCK) ON [State].[StateId] = [Organization].[State]
	WHERE [OrganizationUser].[UserId] = @UserId 
		  AND [Auth].[Organization].[IsActive] = 1
	ORDER BY [OrganizationUser].[OrgRoleId] DESC, [Organization].[Name]

	SELECT 
		[InvitationId], 
		[Invitation].[Email], 
		[Invitation].[FirstName], 
		[Invitation].[LastName], 
		[Invitation].[DateOfBirth], 
		[OrganizationId], 
		[AccessCode], 
		[OrgRole],
		[ProjectId],
		[EmployeeId] 
	FROM [Auth].[User] WITH (NOLOCK)
	LEFT JOIN [Auth].[Invitation] WITH (NOLOCK) ON [User].[Email] = [Invitation].[Email]

	WHERE [User].[UserId] = @userId AND [IsActive] = 1
END
