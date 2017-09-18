CREATE PROCEDURE [Pjm].[CreateProjectAndUpdateItsUserList]
	@customerId INT,
	@projectName NVARCHAR(MAX),
	@isHourly BIT,
	@projectOrgId NVARCHAR(16),
	@startingDate DATETIME2(0),
	@endingDate DATETIME2(0),
	@userIds [Auth].[UserTable] READONLY,
    @retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS (
		SELECT * FROM [Pjm].[Project] WITH (NOLOCK)
		WHERE [ProjectOrgId] = @projectOrgId
		AND [CustomerId] = @customerId
	)
		BEGIN
			-- ProjectOrgId is not unique
			SET @retId = -1;
		END
	ELSE
		BEGIN
			BEGIN TRANSACTION
				-- Create the new project in Project table
				INSERT INTO [Pjm].[Project] ([CustomerId], [ProjectName], [IsHourly], [ProjectOrgId], [StartUtc], [EndUtc])
				VALUES	(@customerId, @projectName, @isHourly, @projectOrgId, @startingDate, @endingDate);
				SET @retId = SCOPE_IDENTITY()

				/* Update new users that used to be users at some point */
				UPDATE [Pjm].[ProjectUser] SET IsActive = 1
				WHERE [ProjectUser].[ProjectId] = @retId 
					AND [ProjectUser].[UserId] IN (SELECT userId FROM @userIds) 
					AND [ProjectUser].[IsActive] = 0

				/* Add new users that have never been on the project */
				INSERT INTO [Pjm].[ProjectUser] ([ProjectId], [UserId], [IsActive])
				SELECT @retId, userId, 1
				FROM @userIds
				WHERE userId NOT IN
					(SELECT [ProjectUser].[UserId]
					FROM [Pjm].[ProjectUser] WITH (NOLOCK)
					WHERE [ProjectUser].[ProjectId] = @retId)

				/* Set inactive existing users that are not in the updated users list */
				UPDATE [Pjm].[ProjectUser] SET IsActive = 0
				WHERE [ProjectUser].[ProjectId] = @retId
					AND [ProjectUser].[UserId] NOT IN (SELECT userId FROM @userIds) 
					AND [ProjectUser].[IsActive] = 1

			COMMIT TRANSACTION		
		END
	SELECT @retId;
END