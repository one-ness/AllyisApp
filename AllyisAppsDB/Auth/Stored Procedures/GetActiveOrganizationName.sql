create procedure Auth.GetActiveOrganizationName
	@orgId int
as
begin
	set nocount on
	select OrganizationName from Organization with (nolock)
	where OrganizationId = @orgId and IsActive = 1
end