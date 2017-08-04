CREATE PROCEDURE [Auth].[CreateUser]
	@firstName NVARCHAR(32),
	@lastName NVARCHAR(32),
    @address NVARCHAR(100), 
    @city NVARCHAR(32), 
    @state NVARCHAR(32), 
    @country NVARCHAR(32), 
    @postalCode NVARCHAR(16),
	@email NVARCHAR(256), 
    @phoneNumber VARCHAR(32),
	@dateOfBirth DATETIME2(0),
	@passwordHash NVARCHAR(MAX),
	@emailConfirmationCode UNIQUEIdENTIFIER,
	@isTwoFactorEnabled BIT,
	@isLockoutEnabled BIT,
	@lockoutEndDateUtc DATE,
	@languageId INT

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
		(@address,
		@city,
		(SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [StateName] = @state),
		(SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [CountryName] = @country),
		@postalCode);

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
		(@firstName, 
		@lastName,
		SCOPE_IDENTITY(),
		@email,
		@phoneNumber,
		@dateOfBirth, 
		@passwordHash, 
		@emailConfirmationCode,
		0,
		COALESCE(@isTwoFactorEnabled,0),
		0,
		COALESCE(@isLockoutEnabled,0),
		COALESCE(@lockoutEndDateUtc,NULL),
		@languageId);

	SELECT SCOPE_IDENTITY();
END
