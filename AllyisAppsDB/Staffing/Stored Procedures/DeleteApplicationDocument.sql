CREATE PROCEDURE [Staffing].[DeleteApplicationDocument]
	@applicationDocumentId INT
AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM [Staffing].[ApplicationDocument] WHERE [ApplicationDocumentId] = @applicationDocumentId
END
