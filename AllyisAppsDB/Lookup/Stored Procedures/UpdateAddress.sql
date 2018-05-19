CREATE procedure [Lookup].[UpdateAddress]
	@addressId int,
	@address1 nvarchar(64),
	@address2 nvarchar(64),
	@city nvarchar(32),
	@stateId smallint,
	@postalCode nvarchar(16),
	@countryCode varchar(8)
as
begin
	set nocount on
	update [Address] set Address1 = @address1, Address2 = @address2, City = @city, StateId = @stateId, CountryCode = @countryCode, PostalCode = @postalCode
	where AddressId = @addressId
	select @@ROWCOUNT
end