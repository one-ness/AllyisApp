CREATE PROCEDURE [Crm].[UpdateProject]
	@ProjectId INT,
	@Name NVARCHAR(MAX),
	@PriceType NVARCHAR(20),
    @StartingDate DATE,
    @EndingDate DATE
AS
	SET NOCOUNT ON;
	UPDATE [Crm].[Project]
	SET 
		[Name] = @Name,
		[Type] = @PriceType,
		[StartUTC] = @StartingDate,
		[EndUTC] = @EndingDate

	WHERE [ProjectId] = @ProjectId