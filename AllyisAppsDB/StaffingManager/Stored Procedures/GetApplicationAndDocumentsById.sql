CREATE PROCEDURE [StaffingManager].[GetApplicationAndDocumentsById]
	@applicationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [StaffingManager].[Application] WHERE [ApplicationId] = @applicationId
	SELECT * FROM [StaffingManager].[ApplicationDocument] WHERE [ApplicationId] = @applicationId
END
