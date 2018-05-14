CREATE PROCEDURE [Hrm].[AddOrgPayClassesToEmployeeType]
	@employeeeTypeId int,
	@organizaionId int 
AS
begin
	set nocount on
	INSERT INTO [Hrm].[EmployeeTypePayClass] 
	SELECT @employeeeTypeId, [Hrm].[PayClass].PayClassId
	FROM [Hrm].[PayClass] WHERE [Payclass].OrganizationId = @organizaionId;
end

	
