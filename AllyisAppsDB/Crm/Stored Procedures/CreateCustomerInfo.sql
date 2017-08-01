CREATE PROCEDURE [Crm].[CreateCustomerInfo]
	@Name NVARCHAR(50),
    @Address NVARCHAR(100),
    @City NVARCHAR(100), 
    @State NVARCHAR(100), 
    @Country NVARCHAR(100), 
    @PostalCode NVARCHAR(50),
	@ContactEmail NVARCHAR(384), 
    @ContactPhoneNumber VARCHAR(50),
	@FaxNumber VARCHAR(50),
	@Website NVARCHAR(50),
	@EIN NVARCHAR(50),
	@OrganizationId INT,
	@CustomerOrgId NVARCHAR(16),
	@retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @AddressId INT

	IF EXISTS (
		SELECT * FROM [Crm].[Customer] WITH (NOLOCK)
		WHERE [CustomerOrgId] = @CustomerOrgId
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
			VALUES (@Address,
				@City,
				(SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [Name] = @State),
				(SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [Name] = @Country),
				@PostalCode)

			SET @AddressId = SCOPE_IDENTITY();

			-- Create customer
			INSERT INTO [Crm].[Customer] 
				([Name], 
				[AddressId],
				[ContactEmail], 
				[ContactPhoneNumber], 
				[FaxNumber], 
				[Website], 
				[EIN], 
				[OrganizationId], 
				[CustomerOrgId])
			VALUES (@Name, 
				@AddressId,
				@ContactEmail, 
				@ContactPhoneNumber, 
				@FaxNumber, 
				@Website, 
				@EIN, 
				@OrganizationId, 
				@CustomerOrgId);
			SET @retId = SCOPE_IDENTITY();
		COMMIT TRANSACTION		
	END
	SELECT @retId;
END