CREATE procedure [Lookup].[CreateAddress]
	@address1 nvarchar(64),
	@address2 nvarchar(64),
	@city nvarchar(32),
	@stateId smallint,
	@postalCode nvarchar(16),
	@countryCode varchar(8)
as
begin
	declare @ret int
	set @ret = null
	if @address1 is not null or @city is not null or @stateId is not null or @postalCode is not null or @countryCode is not null
	begin
		set nocount on
		declare @cID int
		Select @cID = Country.CountryId From Country WHERE @countryCode = Country.CountryCode
		insert into [Address] (Address1, Address2, City, StateId, PostalCode, CountryCode, CountryId) values (@address1, @address2, @city, @stateId, @postalCode, @countryCode,@cID)
		select @ret = SCOPE_IDENTITY()
	end
	return @ret
end