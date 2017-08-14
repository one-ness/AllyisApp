CREATE PROCEDURE [StaffingManager].[CreateApplication]
	@applicantId INT,
	@positionId INT,
	@applicationStatusId TINYINT,
	@notes NVARCHAR (MAX)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION
		INSERT INTO [StaffingManager].[Application]
			([ApplicantId],
			[PositionId],
			[ApplicationStatusId],
			[Notes])
		VALUES
			(@applicantId,
			@positionId,
			@applicationStatusId,
			@notes);

		SELECT SCOPE_IDENTITY();
	COMMIT TRANSACTION
END
