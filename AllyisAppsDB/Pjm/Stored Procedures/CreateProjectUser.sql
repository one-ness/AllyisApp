CREATE PROCEDURE [Pjm].[CreateProjectUser]
	@projectId INT,
	@userId INT
AS
BEGIN 
	SET NOCOUNT ON;
	UPDATE [Pjm].[ProjectUser] SET [IsActive] = 1
	WHERE [ProjectId] = @projectId AND [UserId] = @userId;
	IF @@ROWCOUNT < 1
		INSERT INTO [Pjm].[ProjectUser] ([ProjectId], [UserId], [IsActive])
		VALUES(@projectId, @userId, 1);
END