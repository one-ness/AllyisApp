CREATE PROCEDURE [Crm].[CreateCustomerInfo]
	@customerName NVARCHAR(32),
    @address NVARCHAR(100),
    @city NVARCHAR(100), 
    @state NVARCHAR(100), 
    @country NVARCHAR(100), 
    @postalCode NVARCHAR(50),
	@contactEmail NVARCHAR(384), 
    @contactPhoneNumber VARCHAR(50),
	@faxNumber VARCHAR(50),
	@website NVARCHAR(50),
	@eIN NVARCHAR(50),
	@organizationId INT,
	@customerOrgId NVARCHAR(16),
	@retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @addressId INT

	IF EXISTS (
		SELECT * FROM [Crm].[Customer] WITH (NOLOCK)
		WHERE [CustomerOrgId] = @customerOrgId
	)
	BEGIN
		-- CustomerOrgId is not unique
		SET @retId = -1;
	END
	ELSE
	BEGIN
		BEGIN TRANSACTION
			INSERT INTO [Lookup].[Address]
				([Address1],
				[City],
				[StateId],
				[CountryId],
				[PostalCode])
			VALUES (@address,
				@city,
				(SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [StateName] = @state),
				(SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [CountryName] = @country),
				@postalCode)

			SET @addressId = SCOPE_IDENTITY();

			-- Create customer
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
			SET @retId = SCOPE_IDENTITY();
		COMMIT TRANSACTION		
	END
	SELECT @retId;
END