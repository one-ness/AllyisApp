IF NOT EXISTS(SELECT [name] from [master].[sys].[server_principals] WHERE [name] = 'aaUser')
BEGIN
	CREATE LOGIN aaUser WITH PASSWORD = 'BlueSky23#', CHECK_EXPIRATION=OFF, DEFAULT_DATABASE=[AllyisAppsDB];
END
GO