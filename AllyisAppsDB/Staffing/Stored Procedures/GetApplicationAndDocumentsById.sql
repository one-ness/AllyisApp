CREATE PROCEDURE [Staffing].[GetApplicationAndDocumentsById]
	@applicationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Staffing].[Application] WHERE [ApplicationId] = @applicationId
	SELECT * FROM [Staffing].[ApplicationDocument] WHERE [ApplicationId] = @applicationId
END
