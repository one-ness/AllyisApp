CREATE PROCEDURE [Auth].[UpdateOrganization]
	@organizationId INT,
	@organizationName NVARCHAR(100),
	@siteUrl NVARCHAR(100),
	@address NVARCHAR(100), 
	@city NVARCHAR(100), 
	@state NVARCHAR(100), 
	@country NVARCHAR(100), 
	@postalCode NVARCHAR(50),
	@phoneNumber VARCHAR (50),
	@faxNumber VARCHAR (50),
	@subdomainName NVARCHAR (40),
	@addressId INT
AS
BEGIN
	UPDATE [Lookup].[Address]
	SET [Address1] = @address,
		[City] = @city,
		[StateId] = (SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [StateName] = @state),
		[CountryId] = (SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [CountryName] = @country),
		[PostalCode] = @postalCode
	WHERE [AddressId] = @addressId

	UPDATE [Auth].[Organization]
	SET [OrganizationName] = @organizationName,
		[SiteUrl] = @siteUrl,
		[PhoneNumber] = @phoneNumber,
		[FaxNumber] = @faxNumber,
		[Subdomain] = @subdomainName
	WHERE [OrganizationId] = @organizationId
END