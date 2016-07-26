CREATE PROCEDURE [Auth].[GetOrgRoleList]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [OrgRoleId], 
		[Name]
	FROM [Auth].[OrgRole]
	WITH (NOLOCK);
END