CREATE PROCEDURE [TimeTracker].[DeletePayClass]
	@ID INT
AS
	SET NOCOUNT ON;
	DELETE FROM [TimeTracker].[PayClass] WHERE [PayClassId] = @ID;
