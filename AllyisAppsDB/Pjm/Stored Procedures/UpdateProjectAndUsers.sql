CREATE PROCEDURE [Pjm].[UpdateProjectAndUsers]
	@ProjectId INT,
	@Name NVARCHAR(MAX),
	@OrgId NVARCHAR(16),
	@IsHourly BIT,
    @StartingDate DATE,
    @EndingDate DATE,
	@UserIds [Auth].[UserTable] READONLY
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
		
		/* Update new users that used to be users at some point */
		UPDATE [Pjm].[ProjectUser] SET IsActive = 1
		WHERE [ProjectUser].[ProjectId] = @ProjectId 
			AND [ProjectUser].[UserId] IN (SELECT userId FROM @UserIds) 
			AND [ProjectUser].[IsActive] = 0

		/* Add new users that have never been on the project */
		INSERT INTO [Pjm].[ProjectUser] ([ProjectId], [UserId], [IsActive])
		SELECT @ProjectId, userId, 1
		FROM @UserIds
		WHERE userId NOT IN
			(SELECT [ProjectUser].[UserId]
			FROM [Pjm].[ProjectUser] WITH (NOLOCK)
			WHERE [ProjectUser].[ProjectId] = @ProjectId)

		/* Set inactive existing users that are not in the updated users list */
		UPDATE [Pjm].[ProjectUser] SET IsActive = 0
		WHERE [ProjectUser].[ProjectId] = @ProjectId
			AND [ProjectUser].[UserId] NOT IN (SELECT userId FROM @UserIds) 
			AND [ProjectUser].[IsActive] = 1

		/* Update other project properties */
		UPDATE [Pjm].[Project]
		SET 
			[Name] = @Name,
			[ProjectOrgId] = @OrgId,
			[IsHourly] = @IsHourly,
			[StartUtc] = @StartingDate,
			[EndUtc] = @EndingDate
		WHERE [ProjectId] = @ProjectId

	COMMIT TRANSACTION
END