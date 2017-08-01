CREATE PROCEDURE [TimeTracker].[CreatePayClass]
	@Name NVARCHAR(50),
	@OrganizationId INT
AS
	SET NOCOUNT ON;
	INSERT INTO [Hrm].[PayClass] ([Name], [OrganizationId])
	VALUES (@Name, @OrganizationId);
	SELECT SCOPE_IDENTITY();
