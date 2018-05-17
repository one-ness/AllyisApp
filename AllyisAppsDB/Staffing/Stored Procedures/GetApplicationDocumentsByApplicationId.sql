CREATE PROCEDURE [Staffing].[GetApplicationDocumentsByApplicationId]
	@applicationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Staffing].[ApplicationDocument] WHERE [ApplicationId] = @applicationId
END
