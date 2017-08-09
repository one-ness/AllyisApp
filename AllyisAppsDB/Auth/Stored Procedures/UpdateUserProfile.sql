﻿create procedure Auth.UpdateUserProfile
	@userId int,
	@firstName nvarchar(32),
	@lastName nvarchar(32),
	@dateOfBirth date = null,
	@phoneNumber varchar(16) = null,
	@addressId int = null,
	@address1 nvarchar(64) = null,
	@address2 nvarchar(64) = null,
	@city nvarchar(32) = null,
	@stateId int = null,
	@postalCode nvarchar(16) = null,
	@countryCode varchar(8) = null
as
begin
	set nocount on
	declare @temp int
	set @temp = @addressId
	begin tran t1

	-- create or update address
	if @addressId is not null
		begin
			-- update address
			exec [Lookup].UpdateAddress @temp, @address1, @address2, @city, @stateId, @postalCode, @countryCode
			if @@ERROR <> 0 
				goto _failure
		end
	else
		begin
			-- create address
			exec @temp =  [Lookup].CreateAddress @address1, @address2, @city, @stateId, @postalCode, @countryCode
			if @@ERROR <> 0 
				goto _failure
		end

	update [User] set FirstName = @firstName, LastName = @lastName, PhoneNumber = @phoneNumber, DateOfBirth = @dateOfBirth
	where UserId = @userId

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
end