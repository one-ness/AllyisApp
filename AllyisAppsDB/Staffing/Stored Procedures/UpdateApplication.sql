CREATE PROCEDURE [Staffing].[UpdateApplication]
	@applicationId INT,
	@applicationStatusId TINYINT,
	@notes NVARCHAR (MAX)
AS
BEGIN
	UPDATE [Staffing].[Application] SET
		[ApplicationStatusId] = @applicationStatusId,
		[Notes] = @notes
	WHERE [ApplicationId] = @applicationId
END
