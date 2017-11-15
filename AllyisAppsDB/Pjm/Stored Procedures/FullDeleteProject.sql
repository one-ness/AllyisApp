CREATE PROCEDURE [Pjm].[FullDeleteProject]
	@projectId INT
AS
	DELETE FROM [Pjm].[Project] WHERE [ProjectId] = @projectId
