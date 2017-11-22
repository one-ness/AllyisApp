CREATE PROCEDURE [Auth].[GetProductRoles]
	@orgId INT,
	@productId INT
AS
BEGIN
	SET NOCOUNT ON
	-- NOTE: IGNORE orgId for now, but later we need to use it

	SELECT *
	FROM [ProductRole] WITH (NOLOCK)
	WHERE ProductId = @productId
END