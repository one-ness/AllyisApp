create PROCEDURE [Auth].[CreateUser]
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
	@PasswordHash NVARCHAR(MAX),
	@EmailConfirmationCode UNIQUEIdENTIFIER,
	@TwoFactorEnabled BIT,
	@LockoutEnabled BIT,
	@LockoutEndDateUtc DATE,
	@LanguageId INT

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
		@PasswordHash, 
		@EmailConfirmationCode,
		0,
		COALESCE(@TwoFactorEnabled,0),
		0,
		COALESCE(@LockoutEnabled,0),
		COALESCE(@LockoutEndDateUtc,NULL),
		@LanguageId);

	SELECT SCOPE_IDENTITY();
END