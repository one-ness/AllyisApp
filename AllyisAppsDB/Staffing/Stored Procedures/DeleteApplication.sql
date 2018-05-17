CREATE PROCEDURE [Staffing].[DeleteApplication]
	@applicationId INT
AS
BEGIN
	SET NOCOUNT ON
	DELETE FROM [Staffing].[Application] WHERE [ApplicationId] = @applicationId
END
