CREATE PROCEDURE [Pjm].[CreateProject]
	@customerId INT,
	@projectName NVARCHAR(MAX),
	@isHourly BIT,
	@projectOrgId NVARCHAR(16),
	@startingDate DATETIME2(0),
	@endingDate DATETIME2(0),
    @retId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	INSERT INTO [Pjm].[Project] ([CustomerId], [ProjectName], [IsHourly], [ProjectOrgId], [StartUtc], [EndUtc])
	VALUES	(@customerId, @projectName, @isHourly, @projectOrgId, @startingDate, @endingDate);
	SET @retId = SCOPE_IDENTITY()
	COMMIT TRANSACTION
	SELECT SCOPE_IDENTITY();
END