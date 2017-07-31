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
	@FaxNumber VARCHAR (50),
	@SubdomainName NVARCHAR (40),
	@AddressId INT
AS
BEGIN
	IF EXISTS (
		SELECT * FROM [Auth].[Organization] WITH (NOLOCK)
		WHERE [Subdomain] = @SubdomainName AND [OrganizationId] != @Id
	)
	BEGIN
		-- Subdomain is not unique
		SELECT 0;
	END
	ELSE
	BEGIN

		SET NOCOUNT ON;
		UPDATE [Auth].[Organization]
		SET [Name] = @Name,
			[SiteUrl] = @SiteUrl,
			[PhoneNumber] = @PhoneNumber,
			[FaxNumber] = @FaxNumber,
			[Subdomain] = @SubdomainName
		WHERE [OrganizationId] = @Id

		UPDATE [Lookup].[Address]
		SET [Address1] = @Address,
			[City] = @City,
			[State] = (SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [Name] = @State),
			[CountryId] = (SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [Name] = @Country),
			[PostalCode] = @PostalCode
		WHERE [AddressId] = @AddressId
		
		SELECT 1;
	END	
END