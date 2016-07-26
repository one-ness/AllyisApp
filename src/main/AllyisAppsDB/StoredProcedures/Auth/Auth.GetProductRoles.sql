CREATE PROCEDURE [Auth].[GetProductRoles]
	@ProductName NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [ProductRoleId],
		[ProductId],
		[Name],
		[PermissionAdmin]
	FROM [Auth].[ProductRole] 
	WITH (NOLOCK)
	WHERE [ProductId] = 
		(SELECT [ProductId] 
		FROM [Billing].[Product] 
		WITH (NOLOCK)
		WHERE [Name] = @ProductName);
END