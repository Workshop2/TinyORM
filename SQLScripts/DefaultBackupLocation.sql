declare @rc int,
@dir nvarchar(4000)

exec @rc = master.dbo.xp_instance_regread
N'HKEY_LOCAL_MACHINE',N'Software\Microsoft\MSSQLServer\MSSQLServer',N'BackupDirectory',
@dir output, 'no_output'
SELECT @dir