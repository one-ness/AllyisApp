﻿CREATE PROCEDURE [Auth].[GetUserInvitationsByUserData]
	@Email NVARCHAR(384)
	
AS
	SET NOCOUNT ON;
SELECT 
	[InvitationId], 
	[Email], 
	[FirstName], 
	[LastName], 
	[DateOfBirth], 
	[OrganizationId], 
	[AccessCode], 
	[OrganizationRoleId],
	[EmployeeId] 
FROM [Auth].[Invitation]
WITH (NOLOCK)
WHERE [Email] = @Email AND [IsActive] = 1