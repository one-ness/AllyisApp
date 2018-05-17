CREATE PROCEDURE [Staffing].[GetApplicationById]
	@applicationId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Staffing].[Application] WHERE [ApplicationId] = @applicationId
END
