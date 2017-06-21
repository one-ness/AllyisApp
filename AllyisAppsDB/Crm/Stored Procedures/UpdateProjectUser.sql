CREATE PROCEDURE [Crm].[UpdateProjectUser]
	@ProjectId INT,
	@UserId INT,
	@IsActive BIT,
	@RowsUpdated INT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
		UPDATE [Crm].[ProjectUser]
		SET [IsActive] = @IsActive
		WHERE [ProjectUser].[ProjectId] = @ProjectId AND [ProjectUser].[UserId] = @UserId
		
		SELECT @RowsUpdated = @@ROWCOUNT

	COMMIT TRANSACTION

	SELECT @RowsUpdated
END