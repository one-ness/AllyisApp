CREATE PROCEDURE [Auth].[GetOrgFromAccessCode]
	@AccessCode VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Invitation].[OrganizationId] 
	FROM [Auth].[Invitation]
	WITH (NOLOCK) 
	WHERE [AccessCode] = @AccessCode 
		AND [IsActive] = 1
END
