CREATE procedure [Lookup].[CreateAddress]
	@address1 nvarchar(64),
	@address2 nvarchar(64),
	@city nvarchar(32),
	@stateId smallint,
	@postalCode nvarchar(16),
	@countryCode varchar(8)
as
begin
	set nocount on
	insert into [Address] (Address1, Address2, City, StateId, PostalCode, CountryCode) values (@address1, @address2, @city, @stateId, @postalCode, @countryCode)
	return SCOPE_IDENTITY()
end