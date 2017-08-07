CREATE PROCEDURE [Auth].[GetOrgUserEmployeeId]
	@userId INT,
	@organizationId INT
AS

BEGIN
	SET NOCOUNT ON;

	SELECT [OrganizationUser].[EmployeeId] 
	FROM [Auth].[OrganizationUser]
	WITH (NOLOCK)
	WHERE [UserId] = @userId 
		AND [OrganizationId] = @organizationId;
END