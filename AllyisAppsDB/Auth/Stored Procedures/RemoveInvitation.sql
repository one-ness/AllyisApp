CREATE PROCEDURE [Auth].[RemoveInvitation]
	@invitationId INT,
	@callingUserId INT
AS
BEGIN
	SET NOCOUNT ON;

	-- Retrieve the invitation information
	DECLARE @organizationId INT;
	DECLARE @email NVARCHAR(384);

	SELECT
		@organizationId = [OrganizationId],
		@email = [Email]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	WHERE [Invitation].[InvitationId] = @invitationId AND [Invitation].[IsActive] = 1

	IF @organizationId IS NOT NULL
	BEGIN -- Invitation found

		-- Retrieve invited user
		DECLARE @userId INT;
		SET @userId = (
			SELECT [UserId]
			FROM [Auth].[User] WITH (NOLOCK)
			WHERE [User].[Email] = @email
		)

		IF (@userId IS NOT NULL AND @userId = @callingUserId) OR @callingUserId = -1
		BEGIN -- Invited user found and matches calling user
			BEGIN TRANSACTION

			DELETE FROM [Auth].[Invitation]
			WHERE [InvitationId] = @invitationId

			-- Success indication
			SELECT 1

			COMMIT
		END
		ELSE
		BEGIN
			SELECT 0
		END
	END
	ELSE
	BEGIN
		SELECT 0
	END
END