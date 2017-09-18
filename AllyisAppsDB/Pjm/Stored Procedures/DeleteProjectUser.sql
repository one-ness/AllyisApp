CREATE PROCEDURE [Pjm].[DeleteProjectUser]
	@projectId INT,
	@userId INT,
	@ret INT OUTPUT
AS
BEGIN 
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	UPDATE [Pjm].[ProjectUser] SET [IsActive] = 0 
	WHERE [ProjectId] = @projectId 
	AND [UserId] = @userId;
	SET @ret = 1
	COMMIT TRANSACTION
END