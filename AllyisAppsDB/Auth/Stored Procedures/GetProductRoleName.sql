CREATE PROCEDURE [Auth].[GetProductRoleName]
	@subscriptionId int,
	@productRoleId int
AS
	SELECT [ProductRoleShortName]
	FROM [Auth].[ProductRole]
	WHERE [OrgOrSubId] = @subscriptionId
	AND [ProductRoleId] = @productRoleId