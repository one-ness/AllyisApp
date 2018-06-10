CREATE PROCEDURE [Auth].[UpdateOrganization]
	@organizationId INT,
	@organizationName NVARCHAR(100),
	@siteUrl NVARCHAR(100),
	@phoneNumber VARCHAR (50),
	@faxNumber VARCHAR (50),
	@subdomainName NVARCHAR (40),
	@addressId int
AS
BEGIN
	set nocount on
	UPDATE [Auth].[Organization]
	SET [OrganizationName] = @organizationName,
		[SiteUrl] = @siteUrl,
		[PhoneNumber] = @phoneNumber,
		[FaxNumber] = @faxNumber,
		[Subdomain] = @subdomainName,
		[AddressId] = @addressId
	WHERE [OrganizationId] = @organizationId
END