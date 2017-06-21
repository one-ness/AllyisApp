CREATE PROCEDURE [Auth].[GetOrgUserEmployeeId]
	@UserId INT,
	@OrganizationId INT
AS

BEGIN
	SET NOCOUNT ON;

	SELECT [OrganizationUser].[EmployeeId] 
	FROM [Auth].[OrganizationUser]
	WITH (NOLOCK)
	WHERE [UserId] = @UserId 
		AND [OrganizationId] = @OrganizationId;
END