CREATE PROCEDURE [Pjm].[CreateProjectUser]
	@ProjectId INT,
	@UserId INT
AS
BEGIN 
	SET NOCOUNT ON;
	UPDATE [Pjm].[ProjectUser] SET [IsActive] = 1
	WHERE [ProjectId] = @ProjectId AND [UserId] = @UserId;
	IF @@ROWCOUNT < 1
		INSERT INTO [Pjm].[ProjectUser] ([ProjectId], [UserId], [IsActive])
		VALUES(@ProjectId, @UserId, 1);
END