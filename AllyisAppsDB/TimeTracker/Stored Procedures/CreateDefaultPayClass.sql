CREATE PROCEDURE [Hrm].[CreateDefaultPayClass]
	@organizationId INT
AS
	SET NOCOUNT ON;

	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId])
	VALUES ('Regular', @OrganizationId);

	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId])
	VALUES ('Paid Time Off', @OrganizationId);

	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId])
	VALUES ('Unpaid Time Off', @OrganizationId);

	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId])
	VALUES ('Holiday', @OrganizationId);

	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId])
	VALUES ('Bereavement Leave', @OrganizationId);

	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId])
	VALUES ('Jury Duty', @OrganizationId);

	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId])
	VALUES ('Overtime', @OrganizationId);

	INSERT INTO [Hrm].[PayClass] ([PayClassName], [OrganizationId])
	VALUES ('Other Leave', @OrganizationId);

	SELECT SCOPE_IDENTITY();
