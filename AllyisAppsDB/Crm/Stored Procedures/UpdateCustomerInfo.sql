CREATE PROCEDURE [Crm].[UpdateCustomerInfo]
	@customerId INT,
	@customerName NVARCHAR(50),
	@contactEmail NVARCHAR(384),
	@addressId INT,
    @address NVARCHAR(100), 
    @city NVARCHAR(100), 
    @stateId NVARCHAR(100), 
    @countryCode VARCHAR(8), 
    @postalCode NVARCHAR(50),
    @contactPhoneNumber VARCHAR(50),
	@faxNumber VARCHAR(50),
	@website NVARCHAR(50),
	@eIN NVARCHAR(50),
	@isActive BIT,
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
			declare @temp int
			set @temp = @addressId
			begin tran t1
			
				if @addressId is not null
					begin
				-- update address
						exec [Lookup].UpdateAddress @temp, @address, null, @city, @stateId, @postalCode, @countryCode
							if @@ERROR <> 0 
								goto _failure
					end
				else
					begin
						if(@address is not null or @city is not null or @postalCode is not null or @stateId is not null or @countryCode is not null)
							begin 
					-- create address
							exec @temp =  [Lookup].CreateAddress @address, null, @city, @stateId, @postalCode, @countryCode;
							if @@ERROR <> 0 
								goto _failure
							end 
					end 
				-- update customer
				UPDATE [Crm].[Customer]
				SET [CustomerName] = @customerName,
					[ContactEmail] = @contactEmail,
					[ContactPhoneNumber] = @contactPhoneNumber, 
					[FaxNumber] = @faxNumber,
					[Website] = @website,
					[EIN] = @eIN,
					[CustomerOrgId] = @orgId,
					[AddressId] =  @temp,
					IsActive = @isActive
				WHERE [CustomerId] = @customerId
			SET @retId = 1;
			SELECT @retId;
			_success:
				begin
					commit tran t1
					return
				end

			_failure:
				begin
					rollback tran t1
					return
				end
		END
END
