create procedure Auth.UpdateEmployeeIdAndOrgRole
	@orgId int,
	@userId int,
	@employeeId nvarchar(16),
	@orgRoleId int
as
begin
	set nocount on
	Update OrganizationUser set EmployeeId = @employeeId, OrganizationRoleId = @orgRoleId
	where OrganizationId = @orgId and UserId = @userId
	select @@ROWCOUNT
end