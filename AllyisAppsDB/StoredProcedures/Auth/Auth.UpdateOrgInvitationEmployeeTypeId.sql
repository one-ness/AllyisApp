CREATE PROCEDURE [Auth].[UpdateOrgInvitationEmployeeTypeId]
	@invitationId INT,
	@OrganizationId INT,
	@EmployeeTypeId INT
AS
	BEGIN
	SET NOCOUNT ON;
	UPDATE [Auth].[Invitation] SET [EmployeeType] = @EmployeeTypeId WHERE [InvitationId] = @invitationId AND [OrganizationId] = @OrganizationId;
	END