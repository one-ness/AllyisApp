CREATE PROCEDURE [Auth].[RemoveInvitation]
	@InvitationId INT,
	@CallingUserId INT
AS
BEGIN
	SET NOCOUNT ON;

	-- Retrieve the invitation information
	DECLARE @OrganizationId INT;
	DECLARE @Email NVARCHAR(384);

	SELECT
		@OrganizationId = [OrganizationId],
		@Email = [Email]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	WHERE [Invitation].[InvitationId] = @InvitationId AND [Invitation].[IsActive] = 1

	IF @OrganizationId IS NOT NULL
	BEGIN -- Invitation found

		-- Retrive invited user
		DECLARE @UserId INT;
		SET @UserId = (
			SELECT [UserId]
			FROM [Auth].[User] WITH (NOLOCK)
			WHERE [User].[Email] = @Email
		)

		IF (@UserId IS NOT NULL AND @UserId = @CallingUserId) OR @CallingUserId = -1
		BEGIN -- Invited user found and matches calling user
			BEGIN TRANSACTION

			DELETE FROM [Auth].[Invitation]
			WHERE [InvitationId] = @InvitationId

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