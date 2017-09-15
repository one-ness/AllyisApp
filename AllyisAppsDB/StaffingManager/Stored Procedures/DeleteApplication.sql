CREATE PROCEDURE [StaffingManager].[DeleteApplication]
	@applicationId INT
AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM [StaffingManager].[Application] WHERE [ApplicationId] = @applicationId
END
