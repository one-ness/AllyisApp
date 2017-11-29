CREATE PROCEDURE [Auth].[AcceptInvitation]
	@invitationId INT,
	@callingUserId INT
AS
BEGIN
	SET NOCOUNT ON;

	-- Retrieve the invitation information
	DECLARE @organizationId INT;
	DECLARE @organizationRole INT;
	DECLARE @email NVARCHAR(384);
	DECLARE @employeeId NVARCHAR(16);
	SELECT
		@organizationId = [OrganizationId],
		@organizationRole = [OrganizationRoleId],
		@email = [Email],
		@employeeId = [EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	WHERE [Invitation].[InvitationId] = @invitationId AND [Invitation].InvitationStatus = 1;

	IF @organizationId IS NOT NULL
	BEGIN -- Invitation found

		-- Retrieve invited user
		DECLARE @userId INT;
		SET @userId = (
			SELECT [UserId]
			FROM [Auth].[User] WITH (NOLOCK)
			WHERE [User].[Email] = @email
		)

		IF @userId IS NOT NULL AND @userId = @callingUserId
		BEGIN -- Invited user found and matches calling user id
			BEGIN TRANSACTION

			-- Add user to organization
			IF EXISTS (
				SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
				WHERE [OrganizationUser].[UserId] = @userId AND [OrganizationUser].[OrganizationId] = @organizationId
			)
			BEGIN -- User already in organization
				UPDATE [Auth].[OrganizationUser]
				SET [OrganizationRoleId] = @organizationRole,
					[EmployeeId] = @employeeId
				WHERE [UserId] = @userId AND 
					[OrganizationId] = @organizationId;
			END
			ELSE
			BEGIN -- User not in organization
				INSERT INTO [Auth].[OrganizationUser]  (
					[UserId], 
					[OrganizationId], 
					[OrganizationRoleId], 
					[EmployeeId]
				)
				VALUES (
					@userId, 
					@organizationId,
					@organizationRole, 
					@employeeId
				);
			END

			UPDATE [Auth].[Invitation]
			SET InvitationStatus = 2, DecisionDateUtc = GETUTCDATE()
			WHERE [InvitationId] = @invitationId;
			
			SELECT @@ROWCOUNT;

			COMMIT
		END
	END
END