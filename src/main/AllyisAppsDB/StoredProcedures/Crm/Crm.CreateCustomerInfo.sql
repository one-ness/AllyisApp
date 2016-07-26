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
	@retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [Crm].[Customer] ([Name], [Address], [City], [State], [Country], [PostalCode], [ContactEmail], [ContactPhoneNumber], [FaxNumber], [Website], [EIN], [OrganizationId])
	VALUES (@Name, @Address, @City,
		(SELECT [StateId] FROM [Lookup].[State] WITH (NOLOCK) WHERE [Name] = @State),
		(SELECT [CountryId] FROM [Lookup].[Country] WITH (NOLOCK) WHERE [Name] = @Country),
		@PostalCode, @ContactEmail, @ContactPhoneNumber, @FaxNumber, @Website, @EIN, @OrganizationID);
	SET @retId = SCOPE_IDENTITY();
	SELECT SCOPE_IDENTITY();
END
