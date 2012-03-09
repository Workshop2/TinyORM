DECLARE @File_Exists int
EXEC Master.dbo.xp_fileexist @FileName, @File_Exists OUT 
SELECT @File_Exists