
CREATE PROCEDURE [Auth].[CreateUser]
	@email nvarchar(384),
	@passwordHash nvarchar(512) = null,
	@firstName nvarchar(32) = null,
	@lastName nvarchar(32) = null,
	@emailConfirmationCode uniqueidentifier,
	@dateOfBirth date = null,
	@phoneNumber varchar(16) = null,
	@preferredLanguageId varchar(16) = null,
	@addressId int = null,
	@loginProviderId int = 1
as
begin
	set nocount on
	insert into Auth.[User] (Email, PasswordHash, FirstName, LastName, EmailConfirmationCode, DateOfBirth, PhoneNumber, PreferredLanguageId, AddressId, LoginProviderId) values (@email, @passwordHash, @firstName, @lastName, @emailConfirmationCode, @dateOfBirth, @phoneNumber, @preferredLanguageId, @addressId, @loginProviderId)
	select SCOPE_IDENTITY() as 'UserId'
end
