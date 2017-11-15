
create procedure Auth.[GetOrgInvitationCount]
	@orgId int,
	@statusMask int
as
begin
	set nocount on
	select count(InvitationId) from Invitation with (nolock)
	where OrganizationId = @orgId and (InvitationStatus & @statusMask) > 0
end