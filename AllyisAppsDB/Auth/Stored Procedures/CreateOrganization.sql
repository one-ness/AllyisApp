CREATE PROCEDURE [Auth].[CreateOrganization]
	@organizationName NVARCHAR(100),
	@siteUrl NVARCHAR(100),
	@address NVARCHAR(100),
	@city NVARCHAR(100),
	@stateId int,
	@countryCode VARCHAR(8),
	@postalCode NVARCHAR(50),
	@phoneNumber VARCHAR(50),
	@faxNumber VARCHAR(50),
	@subdomain NVARCHAR(40)
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	declare @addressId int
	declare @ret int

	BEGIN TRANSACTION
		-- Create Address
		exec @addressId = [Lookup].[CreateAddress] @address, null, @city, @stateId, @postalCode, @countryCode
		-- Create org
		INSERT INTO [Auth].[Organization] 
				([OrganizationName],
				[SiteUrl],
				[AddressId],
				[PhoneNumber],
				[FaxNumber],
				[Subdomain])
		VALUES (@organizationName,
				@siteUrl,
				@addressId,
				@phoneNumber,
				@faxNumber,
				@subdomain);
		select @ret = SCOPE_IDENTITY();
	COMMIT TRANSACTION

	-- return the new organization id
	select @ret
END
