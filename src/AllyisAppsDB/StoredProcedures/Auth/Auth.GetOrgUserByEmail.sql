CREATE PROCEDURE [Auth].[GetOrgUserByEmail]
	@Email NVARCHAR(384), 
	@OrgId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [User].[FirstName]
	FROM [Auth].[OrganizationUser]
	WITH (NOLOCK)
	INNER JOIN [Auth].[User] WITH (NOLOCK)  
		ON [User].[UserId] = [OrganizationUser].[UserId]
	WHERE [OrganizationUser].[OrganizationId] = @OrgId 
		AND [User].[Email] = @Email;
END

