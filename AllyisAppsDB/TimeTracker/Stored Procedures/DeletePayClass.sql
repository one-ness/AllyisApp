CREATE PROCEDURE [Hrm].[DeletePayClass]
	@id INT
AS
	SET NOCOUNT ON;
	DELETE FROM [Hrm].[PayClass] WHERE [PayClassId] = @id;
