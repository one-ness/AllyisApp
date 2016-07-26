CREATE PROCEDURE [Crm].[UpdateCustomerInfo]
	@CustomerId INT,
	@Name NVARCHAR(50),
	@ContactEmail NVARCHAR(384),
    @Address NVARCHAR(100), 
    @City NVARCHAR(100), 
    @State NVARCHAR(100), 
    @Country NVARCHAR(100), 
    @PostalCode NVARCHAR(50),
    @ContactPhoneNumber VARCHAR(50),
	@FaxNumber VARCHAR(50),
	@Website NVARCHAR(50),
	@EIN NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [Crm].[Customer]
	SET [Name] = @Name,
		[ContactEmail] = @ContactEmail,
		[Address] = @Address,
		[City] = @City, 
		[State] = (SELECT [StateId] FROM [Lookup].[State] WHERE [Name] = @State), 
		[Country] = (SELECT [CountryId] FROM [Lookup].[Country] WHERE [Name] = @Country), 
		[PostalCode] = @PostalCode, 
		[ContactPhoneNumber] = @ContactPhoneNumber, 
		[FaxNumber] = @FaxNumber,
		[Website] = @Website,
		[EIN] = @EIN
	WHERE [CustomerId] = @CustomerId 
		AND [IsActive] = 1
END
