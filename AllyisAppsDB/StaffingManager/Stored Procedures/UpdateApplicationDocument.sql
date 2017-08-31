CREATE PROCEDURE [StaffingManager].[UpdateApplicationDocument]
	@applicationDocumentId INT,
	@documentLink NVARCHAR (100),
	@documentName NVARCHAR (32)
AS
BEGIN
	UPDATE [StaffingManager].[ApplicationDocument] SET
		[DocumentLink] = @documentLink,
		[DocumentName] = @documentName
	WHERE [ApplicationDocumentId] = @applicationDocumentId
END
