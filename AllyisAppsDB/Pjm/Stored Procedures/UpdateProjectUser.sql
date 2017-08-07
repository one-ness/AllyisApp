CREATE PROCEDURE [Pjm].[UpdateProjectUser]
	@projectId INT,
	@userId INT,
	@isActive BIT,
	@rowsUpdated INT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
		UPDATE [Pjm].[ProjectUser]
		SET [IsActive] = @isActive
		WHERE [ProjectUser].[ProjectId] = @projectId AND [ProjectUser].[UserId] = @userId
		
		SELECT @rowsUpdated = @@rOWCOUNT

	COMMIT TRANSACTION

	SELECT @rowsUpdated
END