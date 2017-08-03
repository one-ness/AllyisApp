CREATE PROCEDURE [Hrm].[CreatePayClass]
	@PayClassName NVARCHAR(50),
	@OrganizationId INT
AS
	SET NOCOUNT ON;
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId])
	VALUES (@PayClassName, @OrganizationId);
	SELECT SCOPE_IDENTITY();
