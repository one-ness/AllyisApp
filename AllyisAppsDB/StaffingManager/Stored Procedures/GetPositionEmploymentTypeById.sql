CREATE PROCEDURE [StaffingManager].[GetEmploymentTypeById]
	@employmentTypeId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [EmploymentTypeId],
		[OrganizationId],
		[EmploymentTypeName]
	FROM [StaffingManager].[EmploymentType]
	WHERE [EmploymentType].[OrganizationId] = @employmentTypeId
END