CREATE PROCEDURE [Auth].[GetProductRoleList]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [ProductRoleId], 
		[ProductId], 
		[Name], 
		[PermissionAdmin]
	FROM [Auth].[ProductRole]
	WITH (NOLOCK);
END