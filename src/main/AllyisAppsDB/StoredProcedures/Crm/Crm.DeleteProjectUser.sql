CREATE PROCEDURE [Crm].[DeleteProjectUser]
	@ProjectId INT,
	@UserId INT,
	@ret INT OUTPUT
AS
BEGIN 
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	UPDATE [Crm].[ProjectUser] SET [IsActive] = 0 
	WHERE [ProjectId] = @ProjectId 
	AND [UserId] = @UserId;
	SET @ret = 1
	COMMIT TRANSACTION
END