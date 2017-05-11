CREATE PROCEDURE [Auth].[GetEmployeeTypeId]
	@EmployeeType NVARCHAR(32)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [EmployeeType].[EmployeeTypeId]
	FROM [Auth].[EmployeeType]
	WITH (NOLOCK)
	WHERE [EmployeeType].[Name] = @EmployeeType
END