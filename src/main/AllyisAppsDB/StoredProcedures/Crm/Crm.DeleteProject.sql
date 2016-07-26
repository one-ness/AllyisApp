CREATE PROCEDURE [Crm].[DeleteProject]
	@ProjectId INT
AS
	SET NOCOUNT ON;
	UPDATE [Crm].[Project]
	SET [IsActive] = 0
	WHERE [ProjectId] = @ProjectId

	UPDATE [Crm].[ProjectUser] SET [IsActive] = 0
	WHERE [ProjectUser].[ProjectId] IN (SELECT [ProjectId] FROM [Crm].[Project] WHERE [IsActive] = 0);
