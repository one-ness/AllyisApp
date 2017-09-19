CREATE PROCEDURE [Auth].[GetOrgUserMaxAmount]
	@userId INT, 
	@orgId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [MaxAmount]
	FROM [Auth].[OrganizationUser]
	WITH (NOLOCK)
	WHERE [OrganizationId] = @orgId AND [UserId] = @userId;
END