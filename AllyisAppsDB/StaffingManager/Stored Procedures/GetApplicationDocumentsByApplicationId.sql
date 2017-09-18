CREATE PROCEDURE [StaffingManager].[GetApplicationDocumentsByApplicationId]
	@applicationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [StaffingManager].[ApplicationDocument] WHERE [ApplicationId] = @applicationId
END
