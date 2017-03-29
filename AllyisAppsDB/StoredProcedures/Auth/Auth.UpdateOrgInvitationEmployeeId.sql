CREATE PROCEDURE [Auth].[UpdateOrgInvitationEmployeeId]
	@InvitationId INT,
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
	UPDATE [Auth].[Invitation] SET [EmployeeId] = @EmployeeId WHERE [InvitationId] = @InvitationId AND [OrganizationId] = @OrganizationId
	SELECT 2; -- Indicates employee id change was successful
	END