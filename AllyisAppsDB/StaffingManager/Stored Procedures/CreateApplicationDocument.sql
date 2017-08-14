CREATE PROCEDURE [StaffingManager].[CreateApplicationDocument]
	@applicationId INT,
	@documentLink NVARCHAR (100),
	@documentName NVARCHAR (32)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION
		INSERT INTO [StaffingManager].[ApplicationDocument]
			([ApplicationId],
			[DocumentLink],
			[DocumentName])
		VALUES
			(@applicationId,
			@documentLink,
			@documentName)

		SELECT SCOPE_IDENTITY();
	COMMIT TRANSACTION
END
