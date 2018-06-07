CREATE PROCEDURE [Auth].[CreateOrganization]
	@organizationName NVARCHAR(64),
	@organizationStatus int,
	@siteUrl NVARCHAR(384) = null,
	@phoneNumber VARCHAR(16) = null,
	@faxNumber VARCHAR(16) = null,
	@subdomain NVARCHAR(32) = null,
	@addressId int = null
AS
BEGIN
	SET NOCOUNT ON;
		INSERT INTO [Auth].[Organization] 
				([OrganizationName],
				[OrganizationStatus],
				[SiteUrl],
				[PhoneNumber],
				[FaxNumber],
				[Subdomain],
				[AddressId])
		VALUES (@organizationName,
				@organizationStatus,
				@siteUrl,
				@phoneNumber,
				@faxNumber,
				@subdomain,
				@addressId);
		select SCOPE_IDENTITY()
END
