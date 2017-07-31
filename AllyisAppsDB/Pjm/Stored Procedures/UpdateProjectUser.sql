CREATE PROCEDURE [Pjm].[UpdateProjectUser]
	@ProjectId INT,
	@UserId INT,
	@IsActive BIT,
	@RowsUpdated INT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
		UPDATE [Pjm].[ProjectUser]
		SET [IsActive] = @IsActive
		WHERE [ProjectUser].[ProjectId] = @ProjectId AND [ProjectUser].[UserId] = @UserId
		
		SELECT @RowsUpdated = @@ROWCOUNT

	COMMIT TRANSACTION

	SELECT @RowsUpdated
END