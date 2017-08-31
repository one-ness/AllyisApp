CREATE PROCEDURE [StaffingManager].[DeleteApplicationDocument]
	@applicationDocumentId INT
AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM [StaffingManager].[ApplicationDocument] WHERE [ApplicationDocumentId] = @applicationDocumentId
END
