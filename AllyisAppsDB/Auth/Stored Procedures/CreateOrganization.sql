CREATE PROCEDURE [Auth].[CreateOrganization]
	@organizationName NVARCHAR(100),
	@siteUrl NVARCHAR(100),
	@address NVARCHAR(100),
	@city NVARCHAR(100),
	@stateID int,
	@countryCode NVARCHAR(8),
	@postalCode NVARCHAR(50),
	@phoneNumber VARCHAR(50),
	@faxNumber VARCHAR(50),
	@subdomain NVARCHAR(40)
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	declare @addressId int
	set @addressId = null

	BEGIN TRANSACTION
		-- Create Address
		if(@address is not null or @city is not null or @countryCode is not null or @postalCode is not null or @stateID is not null)
		Begin
			EXEC @addressId = [Lookup].CreateAddress @address, null, @city, @stateId, @postalCode, @countryCode
		end
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
	COMMIT TRANSACTION

	-- return the new organization id
	SELECT IDENT_CURRENT('[Auth].[Organization]');
END
