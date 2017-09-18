CREATE PROCEDURE [StaffingManager].[GetApplicationById]
	@applicationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [StaffingManager].[Application] WHERE [ApplicationId] = @applicationId
END
