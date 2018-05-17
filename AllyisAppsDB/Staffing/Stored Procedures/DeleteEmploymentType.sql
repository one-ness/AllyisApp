CREATE PROCEDURE [Staffing].[DeleteEmploymentType]
	@employmentTypeId INT
	
AS
BEGIN
	DELETE FROM [Staffing].[EmploymentType] WHERE [EmploymentTypeId] = @employmentTypeId
END