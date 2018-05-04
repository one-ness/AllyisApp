
CREATE PROCEDURE [Auth].[CreateUser]
	@email nvarchar(384),
	@passwordHash nvarchar(512) = null,
	@firstName nvarchar(32) = null,
	@lastName nvarchar(32) = null,
	@emailConfirmationCode uniqueidentifier,
	@dateOfBirth date = null,
	@phoneNumber varchar(16) = null,
	@preferredLanguageId varchar(16) = null,
	@address1 nvarchar(64) = null,
	@address2 nvarchar(64) = null,
	@city nvarchar(32) = null,
	@stateId int = null,
	@postalCode nvarchar(16) = null,
	@countryCode varchar(8) = null,
	@loginProviderId int = 1
as
begin
	set nocount on
	declare @addressId int
	set @addressId = null
	declare @userId int
	set @userId = -1
	begin tran t1
	-- if any address value present, create address
	if (@address1 is not null or @address2 is not null or @city is not null or @postalCode is not null or @stateId is not null or @countryCode is not null)
		begin
			exec @addressId = [Lookup].CreateAddress @address1, @address2, @city, @stateId, @postalCode, @countryCode
			if @@ERROR <> 0 
				goto _failure
		end

	insert into Auth.[User] (Email, PasswordHash, FirstName, LastName, EmailConfirmationCode, DateOfBirth, PhoneNumber, PreferredLanguageId, AddressId, LoginProviderId) values (@email, @passwordHash, @firstName, @lastName, @emailConfirmationCode, @dateOfBirth, @phoneNumber, @preferredLanguageId, @addressId, @loginProviderId)
	if (@@ERROR <> 0)
		goto _failure

	select SCOPE_IDENTITY() as UserId

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
