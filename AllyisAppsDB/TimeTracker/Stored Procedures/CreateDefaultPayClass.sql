CREATE PROCEDURE [Hrm].[CreateDefaultPayClass]
	@organizationId INT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId], [BuiltInPayClassId]) VALUES ('Regular',           @organizationId, 1);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId], [BuiltInPayClassId]) VALUES ('Paid Time Off',     @organizationId, 2);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId], [BuiltInPayClassId]) VALUES ('Unpaid Time Off',   @organizationId, 3);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId], [BuiltInPayClassId]) VALUES ('Holiday',           @organizationId, 4);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId], [BuiltInPayClassId]) VALUES ('Overtime',          @organizationId, 5);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId], [BuiltInPayClassId]) VALUES ('Bereavement Leave', @organizationId, 0);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId], [BuiltInPayClassId]) VALUES ('Jury Duty',         @organizationId, 0);
	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId], [BuiltInPayClassId]) VALUES ('Other Leave',       @organizationId, 0);
END
