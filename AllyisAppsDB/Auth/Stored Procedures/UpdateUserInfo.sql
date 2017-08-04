CREATE PROCEDURE [Auth].[UpdateUserInfo]
	@userId INT,
	@addressId INT,
	@firstName NVARCHAR(32),
	@lastName NVARCHAR(32),
	@address NVARCHAR(32),
	@city NVARCHAR(32),
	@state NVARCHAR(100),
	@country NVARCHAR(100),
	@postalCode NVARCHAR(16),
	@phoneNumber VARCHAR(16),
	@dateOfBirth DATE
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[User]
	SET [FirstName] = @firstName,
		[LastName] = @lastName,
		[PhoneNumber] = @phoneNumber,
		[DateOfBirth] = @dateOfBirth
	WHERE [UserId] = @userId

	UPDATE [Lookup].[Address]
	SET [Address1] = @address,
		[City] = @city,
		[StateId] = (SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [StateName] = @state),
		[CountryId] = (SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [CountryName] = @country),
		[PostalCode] = @postalCode
	WHERE [AddressId] = @addressId
END
