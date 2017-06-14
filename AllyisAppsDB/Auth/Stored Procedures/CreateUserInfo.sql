CREATE PROCEDURE [Auth].[CreateUserInfo]
	@FirstName NVARCHAR(32),
	@LastName NVARCHAR(32),
    @Address NVARCHAR(100), 
    @City NVARCHAR(32), 
    @State NVARCHAR(32), 
    @Country NVARCHAR(32), 
    @PostalCode NVARCHAR(16),
	@Email NVARCHAR(256), 
    @PhoneNumber VARCHAR(32),
	@DateOfBirth DATETIME2(0),
	@UserName NVARCHAR(256),
	@PasswordHash NVARCHAR(MAX),
	@EmailConfirmationCode UNIQUEIDENTIFIER,
	@TwoFactorEnabled BIT,
	@LockoutEnabled BIT,
	@LockoutEndDateUtc DATE,
	@LanguageID INT

AS

BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Auth].[User] 
		([FirstName], 
		[LastName], 
		[Address], 
		[City], 
		[State], 
		[Country], 
		[PostalCode], 
		[Email], 
		[PhoneNumber], 
		[DateOfBirth],
		[UserName],
		[PasswordHash],
		[EmailConfirmationCode],
		[EmailConfirmed],
		[TwoFactorEnabled],
		[AccessFailedCount],
		[LockoutEnabled],
		[LockoutEndDateUtc],
		[LanguagePreference])
	VALUES 
		(@FirstName, 
		@LastName, 
		@Address, 
		@City, 
		(SELECT [StateId]	FROM [Lookup].[State] WITH (NOLOCK) WHERE [Name] = @State),
		(SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [Name] = @Country),
		@PostalCode, 
		@Email, 
		@PhoneNumber,
		@DateOfBirth, 
		@Email, 
		@PasswordHash, 
		@EmailConfirmationCode,
		0,
		COALESCE(@TwoFactorEnabled,0),
		0,
		COALESCE(@LockoutEnabled,0),
		COALESCE(@LockoutEndDateUtc,NULL),
		@LanguageID);

	SELECT SCOPE_IDENTITY();

	SELECT COUNT(*)
	FROM [Auth].[Invitation] 
	WHERE [Invitation].[Email] = @Email
END