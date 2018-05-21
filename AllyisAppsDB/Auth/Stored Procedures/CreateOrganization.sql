CREATE PROCEDURE [Auth].[CreateOrganization]
	@organizationName NVARCHAR(100),
	@siteUrl NVARCHAR(100),
	@phoneNumber VARCHAR(50),
	@faxNumber VARCHAR(50),
	@subdomain NVARCHAR(40),
	@addressId int = null
AS
BEGIN
	SET NOCOUNT ON;
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
		select SCOPE_IDENTITY()
END
