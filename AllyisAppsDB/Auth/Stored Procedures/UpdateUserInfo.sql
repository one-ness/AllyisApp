CREATE PROCEDURE [Auth].[UpdateUserInfo]
	@Id INT,
	@AddressId INT,
	@FirstName NVARCHAR(32),
	@LastName NVARCHAR(32),
    @Address NVARCHAR(32), 
    @City NVARCHAR(32), 
    @State NVARCHAR(100), 
    @Country NVARCHAR(100), 
    @PostalCode NVARCHAR(16),
    @PhoneNumber VARCHAR(16),
	@DateOfBirth DATE
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[User]
	SET [FirstName] = @FirstName,
		[LastName] = @LastName,
		[PhoneNumber] = @PhoneNumber,
		[DateOfBirth] = @DateOfBirth
	WHERE [UserId] = @Id

	UPDATE [Lookup].[Address]
	SET [Address1] = @Address,
		[City] = @City,
		[StateId] = (SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [StateName] = @State),
		[CountryId] = (SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [CountryName] = @Country),
		[PostalCode] = @PostalCode
	WHERE [AddressId] = @AddressId
END