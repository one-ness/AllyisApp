CREATE PROCEDURE [Auth].[UpdateOrgUserEmployeeTypeId]
	@UserId INT,
	@OrganizationId INT,
	@EmployeeTypeId INT
AS
	BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[OrganizationUser] SET [EmployeeTypeId] = @EmployeeTypeId WHERE [UserId] = @UserId AND [OrganizationId] = @OrganizationId;
	END