CREATE PROCEDURE [StaffingManager].[GetApplicationDocumentsById]
	@applicationDocumentId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [StaffingManager].[ApplicationDocument] WHERE [ApplicationDocumentId] = @applicationDocumentId
END
