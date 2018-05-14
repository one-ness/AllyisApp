CREATE PROCEDURE [Hrm].[CreatePayClass]
	@payClassName NVARCHAR(50),
	@organizationId INT,
	@builtinPayClassId int
AS
begin
	SET NOCOUNT ON;
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId], [BuiltInPayClassId])
	VALUES (@payClassName, @organizationId, @builtinPayClassId);
	return SCOPE_IDENTITY();
end
