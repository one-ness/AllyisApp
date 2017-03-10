CREATE PROCEDURE [Auth].[UpdateOrgUserEmployeeId]
	@UserId INT,
	@OrganizationId INT,
	@EmployeeId NVARCHAR(100)
AS
	SET NOCOUNT ON;
	UPDATE [Auth].[OrganizationUser] SET [EmployeeId] = @EmployeeId WHERE [UserId] = @UserId AND [OrganizationId] = @OrganizationId;