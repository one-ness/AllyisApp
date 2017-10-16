CREATE PROCEDURE [Auth].[GetInvitations]
	@organizationId INT,
	@statusMask int
AS
begin
	SET NOCOUNT ON;
	select * from Invitation with (nolock)
	where OrganizationId = @organizationId
	and (InvitationStatus & @statusMask) > 0
end