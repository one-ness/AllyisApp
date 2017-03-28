CREATE PROCEDURE [Auth].[UpdateOrgUserEmployeeId]
	@UserId INT,
	@OrganizationId INT,
	@EmployeeId NVARCHAR(100)
AS
	IF EXISTS (
			SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrganizationId AND [EmployeeId] = @EmployeeId
		) OR EXISTS (
			SELECT * FROM [Auth].[Invitation] WITH (NOLOCK)
			WHERE [OrganizationId] = @OrganizationId AND [IsActive] = 1 AND [EmployeeId] = @EmployeeId
		)
		BEGIN
		SELECT 1;
		END
	ELSE
	BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[OrganizationUser] SET [EmployeeId] = @EmployeeId WHERE [UserId] = @UserId AND [OrganizationId] = @OrganizationId;
	SELECT 2;
	END