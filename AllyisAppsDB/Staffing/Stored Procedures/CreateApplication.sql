CREATE PROCEDURE [Staffing].[CreateApplication]
	@applicantId INT,
	@positionId INT,
	@applicationStatusId TINYINT,
	@notes NVARCHAR (MAX)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION
		INSERT INTO [Staffing].[Application]
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
