CREATE procedure Auth.GetMaxEmployeeId
	@organizationId int
as
begin
	set nocount on
	select top 1 EmployeeId from (select EmployeeId from Invitation where OrganizationId = @organizationId union select EmployeeId from OrganizationUser where OrganizationId = @organizationId) as EmployeeId
	order by EmployeeId COLLATE Latin1_General_BIN desc
end