CREATE PROCEDURE [Pjm].[GetDefaultProject]
	@OrgId int
AS
	SELECT [ProjectId] FROM [Pjm].[Project] AS [P]
	JOIN [Crm].[Customer] AS [C] ON [C].[CustomerId] = [P].[CustomerId] AND [C].[OrganizationId] = @OrgId
	WHERE [IsDefault] = 1

