﻿DECLARE @SPId int
DECLARE @SQL nvarchar(100)

DECLARE my_cursor CURSOR FAST_FORWARD FOR
SELECT SPId FROM MASTER..SysProcesses
WHERE DBId = DB_ID(@DBName) AND SPId <> @@SPId

OPEN my_cursor
FETCH NEXT FROM my_cursor INTO @SPId

WHILE @@FETCH_STATUS = 0
BEGIN
SET @SQL = 'KILL ' + CAST(@SPId as nvarchar(10))
print @SQL
EXEC sp_executeSQL @SQL

FETCH NEXT FROM my_cursor INTO @SPId
END

CLOSE my_cursor
DEALLOCATE my_cursor