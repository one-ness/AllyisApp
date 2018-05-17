CREATE PROCEDURE [Staffing].[GetEmploymentTypeById]
	@employmentTypeId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [EmploymentTypeId],
		[OrganizationId],
		[EmploymentTypeName]
	FROM [Staffing].[EmploymentType]
	WHERE [EmploymentType].[OrganizationId] = @employmentTypeId
END