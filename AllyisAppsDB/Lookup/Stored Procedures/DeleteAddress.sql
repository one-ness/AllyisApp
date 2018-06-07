CREATE procedure [Lookup].[DeleteAddress]
	@addressId int
as
begin
	set nocount on
	delete [Address] where AddressId = @addressId
	select @@ROWCOUNT
end