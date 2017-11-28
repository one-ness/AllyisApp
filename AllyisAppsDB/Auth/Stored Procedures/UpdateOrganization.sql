CREATE PROCEDURE [Auth].[UpdateOrganization]
	@organizationId INT,
	@organizationName NVARCHAR(100),
	@siteUrl NVARCHAR(100),
	@address1 NVARCHAR(100), 
	@city NVARCHAR(100), 
	@stateId int, 
	@countryCode VARCHAR(8), 
	@postalCode NVARCHAR(16),
	@phoneNumber VARCHAR (50),
	@faxNumber VARCHAR (50),
	@subdomainName NVARCHAR (40),
	@addressId INT
AS
BEGIN
	declare @temp int
	set @temp = @addressId
	begin tran t1
	if @addressId is not null
		begin
			-- update address
			exec [Lookup].UpdateAddress @temp, @address1, null, @city, @stateId, @postalCode, @countryCode
			if @@ERROR <> 0 
				goto _failure
		end
	else
		begin
			if(@address1 is not null  or @city is not null or @postalCode is not null or @stateId is not null or @countryCode is not null)
			  begin 
				exec @temp = [Lookup].CreateAddress @address1, null, @city, @stateId, @postalCode, @countryCode;
				if @@ERROR <> 0 
					goto _failure
			  end 
		end 

	UPDATE [Auth].[Organization]
	SET [OrganizationName] = @organizationName,
		[SiteUrl] = @siteUrl,
		[PhoneNumber] = @phoneNumber,
		[FaxNumber] = @faxNumber,
		[Subdomain] = @subdomainName,
		[AddressId] = @temp
	WHERE [OrganizationId] = @organizationId
	;
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