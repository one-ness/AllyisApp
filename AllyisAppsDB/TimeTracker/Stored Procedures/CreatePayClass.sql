CREATE PROCEDURE [Hrm].[CreatePayClass]
	@payClassName NVARCHAR(32),
	@organizationId int,
	@builtinPayClassId int
AS
begin
	SET NOCOUNT ON;
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId], [BuiltInPayClassId])
	VALUES (@payClassName, @organizationId, @builtinPayClassId);
	select SCOPE_IDENTITY();
end
