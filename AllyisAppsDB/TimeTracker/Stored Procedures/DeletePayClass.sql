CREATE PROCEDURE [Hrm].[DeletePayClass]
	@Id INT
AS
	SET NOCOUNT ON;
	DELETE FROM [Hrm].[PayClass] WHERE [PayClassId] = @Id;
