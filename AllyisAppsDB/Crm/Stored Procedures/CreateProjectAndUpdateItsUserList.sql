CREATE PROCEDURE [Crm].[CreateProjectAndUpdateItsUserList]
	@CustomerId INT,
	@Name NVARCHAR(MAX),
	@PriceType NVARCHAR(20),
	@ProjectOrgId NVARCHAR(16),
	@StartingDate DATETIME2(0),
	@EndingDate DATETIME2(0),
	@UserIDs [Auth].[UserTable] READONLY,
    @retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS (
		SELECT * FROM [Crm].[Project] WITH (NOLOCK)
		WHERE [ProjectOrgId] = @ProjectOrgId
		AND [CustomerId] = @CustomerId
	)
		BEGIN
			-- ProjectOrgId is not unique
			SET @retId = -1;
		END
	ELSE
		BEGIN
			BEGIN TRANSACTION
				-- Create the new project in Project table
				INSERT INTO [Crm].[Project] ([CustomerId], [Name], [Type], [ProjectOrgId], [StartUTC], [EndUTC])
				VALUES	(@CustomerId, @Name, @PriceType, @ProjectOrgId, @StartingDate, @EndingDate);
				SET @retId = SCOPE_IDENTITY()

				/* Update new users that used to be users at some point */
				UPDATE [Crm].[ProjectUser] SET IsActive = 1
				WHERE [ProjectUser].[ProjectId] = @retId 
					AND [ProjectUser].[UserId] IN (SELECT userId FROM @UserIDs) 
					AND [ProjectUser].[IsActive] = 0

				/* Add new users that have never been on the project */
				INSERT INTO [Crm].[ProjectUser] ([ProjectId], [UserId], [IsActive])
				SELECT @retId, userId, 1
				FROM @UserIDs
				WHERE userId NOT IN
					(SELECT [ProjectUser].[UserId]
					FROM [Crm].[ProjectUser] WITH (NOLOCK)
					WHERE [ProjectUser].[ProjectId] = @retId)

				/* Set inactive existing users that are not in the updated users list */
				UPDATE [Crm].[ProjectUser] SET IsActive = 0
				WHERE [ProjectUser].[ProjectId] = @retId
					AND [ProjectUser].[UserId] NOT IN (SELECT userId FROM @UserIDs) 
					AND [ProjectUser].[IsActive] = 1

			COMMIT TRANSACTION		
		END
	SELECT @retId;
END