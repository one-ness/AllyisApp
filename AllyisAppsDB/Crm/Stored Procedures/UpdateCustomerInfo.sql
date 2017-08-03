CREATE PROCEDURE [Crm].[UpdateCustomerInfo]
	@CustomerId INT,
	@CustomerName NVARCHAR(50),
	@ContactEmail NVARCHAR(384),
	@AddressId INT,
    @Address NVARCHAR(100), 
    @City NVARCHAR(100), 
    @State NVARCHAR(100), 
    @Country NVARCHAR(100), 
    @PostalCode NVARCHAR(50),
    @ContactPhoneNumber VARCHAR(50),
	@FaxNumber VARCHAR(50),
	@Website NVARCHAR(50),
	@EIN NVARCHAR(50),
	@OrgId NVARCHAR(16),
	@retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS (
		SELECT * FROM [Crm].[Customer] WITH (NOLOCK)
		WHERE [CustomerOrgId] = @OrgId
		AND [IsActive] = 1
		AND [CustomerId] != @CustomerId
	)
	BEGIN
		-- new CustomerOrgId is taken by a different Customer
		SET @retId = -1;
	END
	ELSE
	BEGIN
		BEGIN TRANSACTION
			UPDATE [Lookup].[Address]
			SET [Address1] = @Address,
				[City] = @City,
				[StateId] = (SELECT [StateId] FROM [Lookup].[State] WHERE [StateName] = @State), 
				[CountryId] = (SELECT [CountryId] FROM [Lookup].[Country] WHERE [CountryName] = @Country), 
				[PostalCode] = @PostalCode
			WHERE [AddressId] = @AddressId

			-- update customer
			UPDATE [Crm].[Customer]
			SET [CustomerName] = @CustomerName,
				[ContactEmail] = @ContactEmail,
				[ContactPhoneNumber] = @ContactPhoneNumber, 
				[FaxNumber] = @FaxNumber,
				[Website] = @Website,
				[EIN] = @EIN,
				[CustomerOrgId] = @OrgId
			WHERE [CustomerId] = @CustomerId 
				AND [IsActive] = 1
			SET @retId = 1;
		COMMIT TRANSACTION		
	END
	SELECT @retId;
END