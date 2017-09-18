CREATE PROCEDURE [Hrm].[CreatePayClass]
	@payClassName NVARCHAR(50),
	@organizationId INT
AS
	SET NOCOUNT ON;
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId])
	VALUES (@payClassName, @organizationId);
	SELECT SCOPE_IDENTITY();
