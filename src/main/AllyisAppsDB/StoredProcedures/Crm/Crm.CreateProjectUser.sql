CREATE PROCEDURE [Crm].[CreateProjectUser]
	@ProjectId INT,
	@UserId INT
AS
BEGIN 
	SET NOCOUNT ON;
	UPDATE [Crm].[ProjectUser] SET [IsActive] = 1
	WHERE [ProjectId] = @ProjectId AND [UserId] = @UserId;
	IF @@ROWCOUNT < 1
		INSERT INTO [Crm].[ProjectUser] ([ProjectId], [UserId], [IsActive])
		VALUES(@ProjectId, @UserId, 1);
END