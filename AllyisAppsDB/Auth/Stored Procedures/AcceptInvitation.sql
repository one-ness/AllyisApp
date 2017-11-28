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
		DECLARE @userId INT = (
			SELECT [UserId]
			FROM [Auth].[User] WITH (NOLOCK)
			WHERE [Email] = @email
			AND [UserId] = @callingUserId
		)

		IF @userId IS NOT NULL
		BEGIN -- Invited user found and matches calling user id
			BEGIN TRANSACTION;

			WITH [NewUser] AS (SELECT
				[UserId] = @userId,
				[OrganizationId] = @organizationId,
				[OrganizationRoleId] = @organizationRole,
				[EmployeeId] = @employeeId
			)
			MERGE [Auth].[OrganizationUser] WITH (HOLDLOCK) AS [T]
			USING [NewUser] AS [S]
			ON [T].[OrganizationId] = [S].[OrganizationId]
			AND [T].[UserId] = [S].[UserId]
			WHEN MATCHED THEN UPDATE SET
				[T].[OrganizationRoleId] = [S].[OrganizationRoleId],
				[T].[EmployeeId] = [S].[EmployeeId]
			WHEN NOT MATCHED THEN
				INSERT (
					[UserId],
					[OrganizationId],
					[OrganizationRoleId],
					[EmployeeId])
				VALUES (
					[S].[UserId],
					[S].[OrganizationId],
					[S].[OrganizationRoleId],
					[S].[EmployeeId]
				);

			UPDATE [Auth].[Invitation]
			SET InvitationStatus = 2, DecisionDateUtc = GETUTCDATE()
			WHERE [InvitationId] = @invitationId;
			
			SELECT @@ROWCOUNT;

			COMMIT
		END
	END
END