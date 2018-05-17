CREATE PROCEDURE [Staffing].[UpdateApplicationDocument]
	@applicationDocumentId INT,
	@documentLink NVARCHAR (100),
	@documentName NVARCHAR (32)
AS
BEGIN
	UPDATE [Staffing].[ApplicationDocument] SET
		[DocumentLink] = @documentLink,
		[DocumentName] = @documentName
	WHERE [ApplicationDocumentId] = @applicationDocumentId
END
