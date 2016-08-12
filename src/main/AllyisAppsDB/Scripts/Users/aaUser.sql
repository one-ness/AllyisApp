
IF NOT EXISTS (SELECT name from [sys].[database_principals] WHERE name = 'aaUser')
BEGIN
CREATE USER aaUser
	FOR LOGIN aaUser
	WITH DEFAULT_SCHEMA = dbo

-- Add user to the database owner role
EXEC sp_addrolemember N'db_owner', N'aaUser'
END
