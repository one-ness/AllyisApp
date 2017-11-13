CREATE PROCEDURE [Pjm].[UpdateProjectAndUsers]
	@projectId INT,
	@projectName NVARCHAR(MAX),
	@orgId NVARCHAR(16),
	@isHourly BIT,
    @startingDate DATE,
    @endingDate DATE,
	@userIds [Auth].[UserTable] READONLY,
	@isActive BIT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
		
		/* Update new users that used to be users at some point */
		UPDATE [Pjm].[ProjectUser] SET IsActive = 1
		WHERE [ProjectUser].[ProjectId] = @projectId 
			AND [ProjectUser].[UserId] IN (SELECT userId FROM @userIds) 
			AND [ProjectUser].[IsActive] = 0

		/* Add new users that have never been on the project */
		INSERT INTO [Pjm].[ProjectUser] ([ProjectId], [UserId], [IsActive])
		SELECT @projectId, userId, 1
		FROM @userIds
		WHERE userId NOT IN
			(SELECT [ProjectUser].[UserId]
			FROM [Pjm].[ProjectUser] WITH (NOLOCK)
			WHERE [ProjectUser].[ProjectId] = @projectId)

		/* Set inactive existing users that are not in the updated users list */
		UPDATE [Pjm].[ProjectUser] SET IsActive = 0
		WHERE [ProjectUser].[ProjectId] = @projectId
			AND [ProjectUser].[UserId] NOT IN (SELECT userId FROM @userIds) 
			AND [ProjectUser].[IsActive] = 1

		/* Update other project properties */
		UPDATE [Pjm].[Project]
		SET 
			[ProjectName] = @projectName,
			[ProjectOrgId] = @orgId,
			[IsHourly] = @isHourly,
			[StartUtc] = @startingDate,
			[EndUtc] = @endingDate,
			[IsActive] = @isActive
		WHERE [ProjectId] = @projectId

	COMMIT TRANSACTION
END