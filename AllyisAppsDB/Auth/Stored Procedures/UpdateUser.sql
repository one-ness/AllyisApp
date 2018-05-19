create procedure [Auth].[UpdateUser]
	@userId int,
	@firstName nvarchar(32),
	@lastName nvarchar(32),
	@dateOfBirth date = null,
	@phoneNumber varchar(16) = null,
	@addressId int = null
as
begin
	set nocount on
	update [User] set AddressId = @addressId, FirstName = @firstName, LastName = @lastName, PhoneNumber = @phoneNumber, DateOfBirth = @dateOfBirth
	where UserId = @userId
end