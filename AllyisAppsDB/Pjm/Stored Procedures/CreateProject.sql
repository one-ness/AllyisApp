CREATE PROCEDURE [Pjm].[CreateProject]
	@CustomerId INT,
	@Name NVARCHAR(MAX),
	@IsHourly BIT,
	@ProjectOrgId NVARCHAR(16),
	@StartingDate DATETIME2(0),
	@EndingDate DATETIME2(0),
    @retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	INSERT INTO [Pjm].[Project] ([CustomerId], [Name], [IsHourly], [ProjectOrgId], [StartUtc], [EndUtc])
	VALUES	(@CustomerId, @Name, @IsHourly, @ProjectOrgId, @StartingDate, @EndingDate);
	SET @retId = SCOPE_IDENTITY()
	COMMIT TRANSACTION
	SELECT SCOPE_IDENTITY();
END