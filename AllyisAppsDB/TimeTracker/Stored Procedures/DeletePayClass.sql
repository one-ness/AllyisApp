CREATE PROCEDURE [TimeTracker].[DeletePayClass]
	@Id INT
AS
	SET NOCOUNT ON;
	DELETE FROM [Hrm].[PayClass] WHERE [PayClassId] = @Id;
