CREATE PROCEDURE [Crm].[UpdateCustomerInfo]
	@customerId INT,
	@customerName NVARCHAR(50),
	@contactEmail NVARCHAR(384),
	@addressId INT,
    @address NVARCHAR(100), 
    @city NVARCHAR(100), 
    @state NVARCHAR(100), 
    @country NVARCHAR(100), 
    @postalCode NVARCHAR(50),
    @contactPhoneNumber VARCHAR(50),
	@faxNumber VARCHAR(50),
	@website NVARCHAR(50),
	@eIN NVARCHAR(50),
	@orgId NVARCHAR(16),
	@retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS (
		SELECT * FROM [Crm].[Customer] WITH (NOLOCK)
		WHERE [CustomerOrgId] = @orgId
		AND [IsActive] = 1
		AND [CustomerId] != @customerId
	)
	BEGIN
		-- new CustomerOrgId is taken by a different Customer
		SET @retId = -1;
	END
	ELSE
	BEGIN
		BEGIN TRANSACTION
			UPDATE [Lookup].[Address]
			SET [Address1] = @address,
				[City] = @city,
				[StateId] = (SELECT [StateId] FROM [Lookup].[State] WHERE [StateName] = @state), 
				[CountryId] = (SELECT [CountryId] FROM [Lookup].[Country] WHERE [CountryName] = @country), 
				[PostalCode] = @postalCode
			WHERE [AddressId] = @addressId

			-- update customer
			UPDATE [Crm].[Customer]
			SET [CustomerName] = @customerName,
				[ContactEmail] = @contactEmail,
				[ContactPhoneNumber] = @contactPhoneNumber, 
				[FaxNumber] = @faxNumber,
				[Website] = @website,
				[EIN] = @eIN,
				[CustomerOrgId] = @orgId
			WHERE [CustomerId] = @customerId 
				AND [IsActive] = 1
			SET @retId = 1;
		COMMIT TRANSACTION		
	END
	SELECT @retId;
END