CREATE PROCEDURE [Pjm].[ReactivateProject]
	@projectId INT
AS
	SET NOCOUNT ON;
	UPDATE [Pjm].[Project]
	SET [IsActive] = 1, [EndUtc] = NULL
	WHERE [ProjectId] = @projectId

	/*UPDATE [Pjm].[ProjectUser] SET [IsActive] = 1
	WHERE [ProjectUser].[ProjectId] = @projectId */