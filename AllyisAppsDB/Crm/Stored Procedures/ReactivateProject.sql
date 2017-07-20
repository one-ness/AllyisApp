CREATE PROCEDURE [Crm].[ReactivateProject]
	@ProjectId INT
AS
	SET NOCOUNT ON;
	UPDATE [Crm].[Project]
	SET [IsActive] = 1, [EndUTC] = NULL
	WHERE [ProjectId] = @ProjectId

	UPDATE [Crm].[ProjectUser] SET [IsActive] = 1
	WHERE [ProjectUser].[ProjectId] IN (SELECT [ProjectId] FROM [Crm].[Project] WHERE [IsActive] = 1)