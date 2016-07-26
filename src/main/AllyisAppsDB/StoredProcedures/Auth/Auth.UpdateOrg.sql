CREATE PROCEDURE [Auth].[UpdateOrg]
	@Id INT,
	@Name NVARCHAR(100),
	@SiteUrl NVARCHAR(100),
    @Address NVARCHAR(100), 
    @City NVARCHAR(100), 
    @State NVARCHAR(100), 
    @Country NVARCHAR(100), 
    @PostalCode NVARCHAR(50),
	@PhoneNumber VARCHAR (50),
	@FaxNumber VARCHAR (50)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[Organization]
	SET [Name] = @Name,
		[SiteUrl] = @SiteUrl,
		[Address] = @Address,
		[City] = @City,
		[State] = (SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [Name] = @State),
		[Country] = (SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [Name] = @Country),
		[PostalCode] = @PostalCode,
		[PhoneNumber] = @PhoneNumber,
		[FaxNumber] = @FaxNumber
	WHERE [OrganizationId] = @Id	
END
