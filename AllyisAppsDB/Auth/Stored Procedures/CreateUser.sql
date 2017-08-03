CREATE PROCEDURE [Auth].[CreateUser]
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
	@IsTwoFactorEnabled BIT,
	@IsLockoutEnabled BIT,
	@LockoutEndDateUtc DATE,
	@LanguageId INT,
	@Address_Identity INT
AS

BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Lookup].[Address]
		([Address1],
		[City],
		[StateId],
		[CountryId],
		[PostalCode])
	VALUES
		(@Address,
		@City,
		(SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [Name] = @State),
		(SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [Name] = @Country),
		@PostalCode);

	SET @Address_Identity = SCOPE_IDENTITY()

	INSERT INTO [Auth].[User] 
		([FirstName], 
		[LastName], 
		[AddressId],
		[Email], 
		[PhoneNumber], 
		[DateOfBirth],
		[PasswordHash],
		[EmailConfirmationCode],
		[IsEmailConfirmed],
		[IsTwoFactorEnabled],
		[AccessFailedCount],
		[IsLockoutEnabled],
		[LockoutEndDateUtc],
		[PreferredLanguageId])
	VALUES 
		(@FirstName, 
		@LastName,
		@Address_Identity,
		@Email,
		@PhoneNumber,
		@DateOfBirth, 
		@PasswordHash, 
		@EmailConfirmationCode,
		0,
		COALESCE(@IsTwoFactorEnabled,0),
		0,
		COALESCE(@IsLockoutEnabled,0),
		COALESCE(@LockoutEndDateUtc,NULL),
		@LanguageId);

	SELECT SCOPE_IDENTITY();
END