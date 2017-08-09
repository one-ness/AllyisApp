CREATE procedure Auth.GetUserProfile
	@userId int
as
begin
	set nocount on
	select u.*, a.*, s.StateName, c.CountryName from [User] u with (nolock)
	inner join [Lookup].[Address] a with (nolock) on a.AddressId = u.AddressId
	inner join [Lookup].[State] s with (nolock) on s.StateId = a.StateId
	inner join [Lookup].[Country] c with (nolock) on c.CountryCode = a.CountryCode
	where u.UserId = @userId
end