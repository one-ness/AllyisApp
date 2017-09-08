CREATE PROCEDURE [Crm].[CreateCustomer]
	@customerName NVARCHAR(32),
    @addressId INT,
	@contactEmail NVARCHAR(384), 
    @contactPhoneNumber VARCHAR(16),
	@faxNumber VARCHAR(16),
	@website NVARCHAR(128),
	@eIN NVARCHAR(16),
	@organizationId INT,
	@customerOrgId NVARCHAR(16)
AS
BEGIN
	SET NOCOUNT ON;
		INSERT INTO [Crm].[Customer] 
			([CustomerName], 
			[AddressId],
			[ContactEmail], 
			[ContactPhoneNumber], 
			[FaxNumber], 
			[Website], 
			[EIN], 
			[OrganizationId], 
			[CustomerOrgId])
		VALUES (@customerName, 
			@addressId,
			@contactEmail, 
			@contactPhoneNumber, 
			@faxNumber, 
			@website, 
			@eIN, 
			@organizationId, 
			@customerOrgId);
	
	SELECT
		SCOPE_IDENTITY();
END