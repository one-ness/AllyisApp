CREATE PROCEDURE [Auth].[GetUserList]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [UserId], [FirstName], [LastName], [DateOfBirth], [Address], [City], [State], [PostalCode], [Email],
		[Email], [PhoneNumber], [PhoneExtension], [LastSubscriptionId], [ActiveOrganizationId], 
		[CreatedUtc], [ModifiedUtc], [PasswordHash], [EmailConfirmed], [TwoFactorEnabled],
		[AccessFailedCount], [LockoutEnabled], [LockoutEndDateUtc], [UserName], [LanguagePreference]
	FROM [Auth].[User]
	WITH (NOLOCK)
	ORDER BY [User].[LastName]
END
