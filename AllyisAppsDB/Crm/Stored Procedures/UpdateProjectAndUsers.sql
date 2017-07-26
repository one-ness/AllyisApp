CREATE PROCEDURE [Crm].[UpdateProjectAndUsers]
	@ProjectId INT,
	@Name NVARCHAR(MAX),
	@OrgId NVARCHAR(16),
	@PriceType NVARCHAR(20),
    @StartingDate DATE,
    @EndingDate DATE,
	@UserIDs [Auth].[UserTable] READONLY
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
		
		/* Update new users that used to be users at some point */
		UPDATE [Crm].[ProjectUser] SET IsActive = 1
		WHERE [ProjectUser].[ProjectId] = @ProjectId 
			AND [ProjectUser].[UserId] IN (SELECT userId FROM @UserIDs) 
			AND [ProjectUser].[IsActive] = 0

		/* Add new users that have never been on the project */
		INSERT INTO [Crm].[ProjectUser] ([ProjectId], [UserId], [IsActive])
		SELECT @ProjectId, userId, 1
		FROM @UserIDs
		WHERE userId NOT IN
			(SELECT [ProjectUser].[UserId]
			FROM [Crm].[ProjectUser] WITH (NOLOCK)
			WHERE [ProjectUser].[ProjectId] = @ProjectId)

		/* Set inactive existing users that are not in the updated users list */
		UPDATE [Crm].[ProjectUser] SET IsActive = 0
		WHERE [ProjectUser].[ProjectId] = @ProjectId
			AND [ProjectUser].[UserId] NOT IN (SELECT userId FROM @UserIDs) 
			AND [ProjectUser].[IsActive] = 1

		/* Update other project properties */
		UPDATE [Crm].[Project]
		SET 
			[Name] = @Name,
			[ProjectOrgId] = @OrgId,
			[Type] = @PriceType,
			[StartUtc] = @StartingDate,
			[EndUtc] = @EndingDate
		WHERE [ProjectId] = @ProjectId

	COMMIT TRANSACTION
END