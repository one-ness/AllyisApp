CREATE PROCEDURE [Auth].[AcceptInvitation]
	@InvitationId INT,
	@CallingUserId INT
AS
BEGIN
	SET NOCOUNT ON;

	-- Retrieve the invitation information
	DECLARE @OrganizationId INT;
	DECLARE @OrganizationRole INT;
	DECLARE @Email NVARCHAR(384);
	DECLARE @EmployeeId NVARCHAR(16);
	SELECT
		@OrganizationId = [OrganizationId],
		@OrganizationRole = [OrganizationRoleId],
		@Email = [Email],
		@EmployeeId = [EmployeeId]
	FROM [Auth].[Invitation] WITH (NOLOCK)
	WHERE [Invitation].[InvitationId] = @InvitationId AND [Invitation].[IsActive] = 1

	IF @OrganizationId IS NOT NULL
	BEGIN -- Invitation found

		-- Retrieve invited user
		DECLARE @UserId INT;
		SET @UserId = (
			SELECT [UserId]
			FROM [Auth].[User] WITH (NOLOCK)
			WHERE [User].[Email] = @Email
		)

		IF @UserId IS NOT NULL AND @UserId = @CallingUserId
		BEGIN -- Invited user found and matches calling user id
			BEGIN TRANSACTION

			-- Add user to organization
			IF EXISTS (
				SELECT * FROM [Auth].[OrganizationUser] WITH (NOLOCK)
				WHERE [OrganizationUser].[UserId] = @UserId AND [OrganizationUser].[OrganizationId] = @OrganizationId
			)
			BEGIN -- User already in organization
				UPDATE [Auth].[OrganizationUser]
				SET [OrganizationRoleId] = @OrganizationRole,
					[EmployeeId] = @EmployeeId
				WHERE [UserId] = @UserId AND 
					[OrganizationId] = @OrganizationId;
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
					@UserId, 
					@OrganizationId,
					@OrganizationRole, 
					@EmployeeId
				);
			END

			DELETE FROM [Auth].[Invitation]
			WHERE [InvitationId] = @InvitationId
			
			-- On success, return name of organization and role
			SELECT [Organization].[Name]
			FROM [Auth].[Organization]
			WHERE [Organization].[OrganizationId] = @OrganizationId

			SELECT [OrganizationRole].[Name]
			FROM [Auth].[OrganizationRole]
			WHERE [OrganizationRole].[OrganizationRoleId] = @OrganizationRole

			COMMIT
		END
	END
END