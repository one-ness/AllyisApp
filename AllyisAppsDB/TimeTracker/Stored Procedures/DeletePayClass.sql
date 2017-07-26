CREATE PROCEDURE [TimeTracker].[DeletePayClass]
	@Id INT
AS
	SET NOCOUNT ON;
	DELETE FROM [TimeTracker].[PayClass] WHERE [PayClassId] = @Id;
