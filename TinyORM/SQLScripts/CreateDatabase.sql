﻿DECLARE @SQL NVARCHAR(300)
SET @SQL = REPLACE('CREATE DATABASE {DBName}; ALTER DATABASE {DBName} SET ONLINE;', '{DBName}', @DBName);
EXECUTE sp_executesql @SQL