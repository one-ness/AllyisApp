CREATE PROCEDURE [Lookup].[GetAddress]
	@addressId int
AS
BEGIN
	set nocount on
	select * from [Address] with (nolock)
	where AddressId = @addressId
END
