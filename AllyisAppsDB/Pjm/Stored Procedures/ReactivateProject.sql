CREATE PROCEDURE [Pjm].[ReactivateProject]
	@ProjectId INT
AS
	SET NOCOUNT ON;
	UPDATE [Pjm].[Project]
	SET [IsActive] = 1, [EndUtc] = NULL
	WHERE [ProjectId] = @ProjectId

	UPDATE [Pjm].[ProjectUser] SET [IsActive] = 1
	WHERE [ProjectUser].[ProjectId] IN (SELECT [ProjectId] FROM [Pjm].[Project] WHERE [IsActive] = 1)