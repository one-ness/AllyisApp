CREATE PROCEDURE [StaffingManager].[DeleteEmploymentType]
	@employmentTypeId INT
	
AS
BEGIN
	DELETE FROM [StaffingManager].[EmploymentType] WHERE [EmploymentTypeId] = @employmentTypeId
END