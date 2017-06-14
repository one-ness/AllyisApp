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
	@OrganizationID INT,
	@CustomerOrgId NVARCHAR(16),
	@retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS (
		SELECT * FROM [Crm].[Customer] WITH (NOLOCK)
		WHERE [CustomerOrgId] = @CustomerOrgId
		AND [IsActive] = 1
	)
	BEGIN
		-- CustomerOrgId is not unique
		SET @retId = -1;
	END
	ELSE
	BEGIN
		BEGIN TRANSACTION
			-- Create customer
			INSERT INTO [Crm].[Customer] 
				([Name], 
				[Address], 
				[City], 
				[State], 
				[Country], 
				[PostalCode], 
				[ContactEmail], 
				[ContactPhoneNumber], 
				[FaxNumber], 
				[Website], 
				[EIN], 
				[OrganizationId], 
				[CustomerOrgId])
			VALUES (@Name, 
				@Address, 
				@City,
				(SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [Name] = @State),
				(SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [Name] = @Country),
				@PostalCode, 
				@ContactEmail, 
				@ContactPhoneNumber, 
				@FaxNumber, 
				@Website, 
				@EIN, 
				@OrganizationID, 
				@CustomerOrgId);
			SET @retId = SCOPE_IDENTITY();
		COMMIT TRANSACTION		
	END
	SELECT @retId;
END