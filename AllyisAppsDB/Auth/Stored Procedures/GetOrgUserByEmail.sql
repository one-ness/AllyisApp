CREATE PROCEDURE [Auth].[GetOrgUserByEmail]
	@email NVARCHAR(384), 
	@orgId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [User].[FirstName]
	FROM [Auth].[OrganizationUser]
	WITH (NOLOCK)
	INNER JOIN [Auth].[User] WITH (NOLOCK)  
		ON [User].[UserId] = [OrganizationUser].[UserId]
	WHERE [OrganizationUser].[OrganizationId] = @orgId 
		AND [User].[Email] = @email;
END