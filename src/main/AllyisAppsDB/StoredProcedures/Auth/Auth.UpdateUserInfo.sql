CREATE PROCEDURE [Auth].[UpdateUserInfo]
	@Id INT,
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
		[Address] = @Address,
		[City] = @City,
		[State] = (SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [Name] = @State),
		[Country] = (SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [Name] = @Country),
		[PostalCode] = @PostalCode,
		[PhoneNumber] = @PhoneNumber,
		[DateOfBirth] = @DateOfBirth
	WHERE [UserId] = @Id
END
