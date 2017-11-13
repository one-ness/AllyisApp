CREATE PROCEDURE [Crm].[CreateCustomerInfo]
	@customerName NVARCHAR(32),
    @address NVARCHAR(100),
    @city NVARCHAR(100), 
    @stateId int, 
    @countryCode VARCHAR(8), 
    @postalCode NVARCHAR(50),
	@contactEmail NVARCHAR(384), 
    @contactPhoneNumber VARCHAR(50),
	@faxNumber VARCHAR(50),
	@website NVARCHAR(50),
	@eIN NVARCHAR(50),
	@isActive BIT,
	@organizationId INT,
	@customerOrgId NVARCHAR(16),
	@retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @addressId INT

	IF EXISTS (
		SELECT * FROM [Crm].[Customer] WITH (NOLOCK)
		WHERE [CustomerOrgId] = @customerOrgId AND [OrganizationId] = @organizationId
	)
	BEGIN
		-- CustomerOrgId is not unique
		SET @retId = -1;
	END
	ELSE
	BEGIN

		begin tran t1

			if (@address is not null  or @city is not null or @postalCode is not null or @stateId is not null or @countryCode is not null)
			begin
				exec @addressId = [Lookup].CreateAddress @address, null, @city, @stateId, @postalCode, @countryCode
				if @@ERROR <> 0 
					goto _failure
			end

			-- Create customer
			INSERT INTO [Crm].[Customer] 
				([CustomerName], 
				[AddressId],
				[ContactEmail], 
				[ContactPhoneNumber], 
				[FaxNumber], 
				[Website], 
				[EIN], 
				[IsActive],
				[OrganizationId], 
				[CustomerOrgId])
			VALUES (@customerName, 
				@addressId,
				@contactEmail, 
				@contactPhoneNumber, 
				@faxNumber, 
				@website, 
				@eIN, 
				@isActive,
				@organizationId, 
				@customerOrgId);
			SET @retId = SCOPE_IDENTITY();
			if (@@ERROR <> 0)
					goto _failure
	SELECT @retId;
		_success:
			begin
				commit tran t1
				return @retId;
			end

		_failure:
			begin
				rollback tran t1
				return @retId;
			end		
	END
END