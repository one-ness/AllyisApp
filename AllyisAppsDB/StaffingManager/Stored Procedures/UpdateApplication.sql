CREATE PROCEDURE [StaffingManager].[UpdateApplication]
	@applicationId INT,
	@applicationStatusId TINYINT,
	@notes NVARCHAR (MAX)
AS
BEGIN
	UPDATE [StaffingManager].[Application] SET
		[ApplicationStatusId] = @applicationStatusId,
		[Notes] = @notes
	WHERE [ApplicationId] = @applicationId
END
