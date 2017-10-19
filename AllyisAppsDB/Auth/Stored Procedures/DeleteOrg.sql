CREATE PROCEDURE [Auth].[DeleteOrg]
	@orgId INT
AS 
BEGIN
		UPDATE [Auth].[Organization]
		SET [IsActive] = 0
		WHERE [OrganizationId] = @orgId;
END