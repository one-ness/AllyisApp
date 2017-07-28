CREATE PROCEDURE [Hrm].[GetEmployeeTypeId]
	@EmployeeType NVARCHAR(32)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [EmployeeType].[EmployeeTypeId]
	FROM [Hrm].[EmployeeType]
	WITH (NOLOCK)
	WHERE [EmployeeType].[Name] = @EmployeeType
END