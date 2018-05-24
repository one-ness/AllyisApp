create procedure [Auth].[UpdateOrganizationStatus]
	@orgId int,
	@orgStatus int
as
begin
	set nocount on
	update [Auth].[Organization] set OrganizationStatus = @orgStatus
	where OrganizationId = @orgId
end