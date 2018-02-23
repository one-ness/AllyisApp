create PROCEDURE [Auth].[GetInvitationsByEmail]
	@email nvarchar(384),
	@statusMask INT
AS

	SET NOCOUNT ON;

	SELECT * FROM [Invitation] WITH (NOLOCK)
	WHERE Email = @email
	AND ([InvitationStatus] & @statusMask) > 0