CREATE PROCEDURE [Auth].[CreateOrganization]
	@name NVARCHAR(100),
	@siteUrl NVARCHAR(100),
	@address NVARCHAR(100),
	@city NVARCHAR(100),
	@state NVARCHAR(100),
	@country NVARCHAR(100),
	@postalCode NVARCHAR(50),
	@phoneNumber VARCHAR(50),
	@faxNumber VARCHAR(50),
	@subdomain NVARCHAR(40)
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRANSACTION
		-- Create Address
		INSERT INTO [Lookup].[Address]
				([Address1],
				[City],
				[StateId],
				[CountryId],
				[PostalCode])
		VALUES(@address,
				@city,
				(SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [Name] = @state),
				(SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [Name] = @country),
				@postalCode);

		-- Create org
		INSERT INTO [Auth].[Organization] 
				([Name],
				[SiteUrl],
				[AddressId],
				[PhoneNumber],
				[FaxNumber],
				[Subdomain])
		VALUES (@name,
				@siteUrl,
				SCOPE_IDENTITY(),
				@phoneNumber,
				@faxNumber,
				@subdomain);
	COMMIT TRANSACTION

	-- return the new organization id
	SELECT IDENT_CURRENT('[Auth].[Organization]');
END
