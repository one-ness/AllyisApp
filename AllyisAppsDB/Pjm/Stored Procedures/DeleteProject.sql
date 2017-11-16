CREATE PROCEDURE [Pjm].[DeleteProject]
	@projectId INT
AS
	DELETE FROM [Pjm].[Project] WHERE [ProjectId] = @projectId
