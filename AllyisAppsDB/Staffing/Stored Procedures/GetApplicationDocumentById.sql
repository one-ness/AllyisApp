CREATE PROCEDURE [Staffing].[GetApplicationDocumentsById]
	@applicationDocumentId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Staffing].[ApplicationDocument] WHERE [ApplicationDocumentId] = @applicationDocumentId
END
