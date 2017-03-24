CREATE PROCEDURE [Auth].[UpdateOrgInvitationEmployeeId]
	@InvitationId INT,
	@OrganizationId INT,
	@EmployeeId NVARCHAR(100)
AS
	SET NOCOUNT ON;
	UPDATE [Auth].[Invitation] SET [EmployeeId] = @EmployeeId WHERE [InvitationId] = @InvitationId AND [OrganizationId] = @OrganizationId;
