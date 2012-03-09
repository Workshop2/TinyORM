using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace TinyORM
{
    public class DbMaintenance
    {
        private Db DbConnection { get; set; }

        #region Constructor

        public DbMaintenance(Db dbConnection)
        {
            DbConnection = dbConnection;
        }

        #endregion

        #region PublicMethods

        private const string BackupDbsql =
            "BACKUP DATABASE @DBName TO DISK = @BackupLocation WITH FORMAT, NAME = @BackupDescription;";

        private const string RestoreDbsql =
            "RESTORE DATABASE @DBName FROM DISK = @BackupLocation WITH REPLACE, RECOVERY;";

        private const string ListAllDbsql =
            "SELECT name FROM master..sysdatabases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb') ORDER BY name ASC;";

        private const string VerifyDbsql = "RESTORE VERIFYONLY FROM DISK = @DBBackupLocation";
        private const string CurrentDbsql = "SELECT db_name()";

        public DbResultInfo BackupDatabase(string database, string backupLocation)
        {
            var killResult = KillAllProcesses(database);
            if (!killResult.Success)
                return killResult;

            //Change the DB to master
            using (new DbTempChange(DbConnection))
            {
                var paramList = new List<SqlParameter>
                {
                    new SqlParameter("@DBName", SqlDbType.NVarChar) {Value = database},
                    new SqlParameter("@BackupLocation", SqlDbType.NVarChar) {Value = backupLocation},
                    new SqlParameter("@BackupDescription", SqlDbType.NVarChar) {Value = "Full Backup of " + database}
                };

                return DbConnection.Execute(BackupDbsql, paramList);
            }
        }

        public DbResultInfo RestoreDatabase(string database, string backupLocation)
        {
            var killResult = KillAllProcesses(database);
            if (!killResult.Success)
                return killResult;

            //Change the DB to master
            using (new DbTempChange(DbConnection))
            {
                var paramList = new List<SqlParameter>
                {
                    new SqlParameter("@DBName", SqlDbType.NVarChar) {Value = database},
                    new SqlParameter("@BackupLocation", SqlDbType.NVarChar) {Value = backupLocation}
                };

                return DbConnection.Execute(RestoreDbsql, paramList);
            }
        }

        /// <summary>
        /// Kills all other processes for a given DB
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public DbResultInfo KillAllProcesses(string database)
        {
            //Change the DB to master
            using (new DbTempChange(DbConnection))
            {
                try
                {
                    //We store the SQl script in memory, this makes it easier to use and store
                    var sqlToRun = GetResourceFile("KillAllProcesses.sql");

                    var paramList = new List<SqlParameter>
                    {
                        new SqlParameter("@DBName", SqlDbType.NVarChar) {Value = database}
                    };

                    return DbConnection.Execute(sqlToRun, paramList);
                }
                catch (Exception e)
                {
                    return new DbResultInfo(e.Message);
                }
            }
        }

        public DbResultInfoRtn<List<string>> ListAllDatabases()
        {
            //Change the DB to master
            using (new DbTempChange(DbConnection))
                return DbConnection.Execute<List<String>>(ListAllDbsql);
        }

        public DbResultInfoRtn<string> DefaultBackupLocation()
        {
            try
            {
                //We store the SQl script in memory, this makes it easier to use and store
                string sqlToRun = GetResourceFile("DefaultBackupLocation.sql");

                return DbConnection.Execute<string>(sqlToRun);
            }
            catch (Exception e)
            {
                return new DbResultInfoRtn<string>(e.Message, null);
            }
        }

        public DbResultInfo CreateDatabase(string database)
        {
            //Change the DB to master
            using (new DbTempChange(DbConnection))
            {
                string sqlToRun = GetResourceFile("CreateDatabase.sql");

                var paramList = new List<SqlParameter>
                {
                    new SqlParameter("@DBName", SqlDbType.NVarChar) {Value = database}
                };

                return DbConnection.Execute(sqlToRun, paramList);
            }
        }

        public DbResultInfoRtn<bool> DoesFileExistOnServer(string fileName)
        {
            //Change the DB to master
            using (new DbTempChange(DbConnection))
            {
                try
                {
                    //We store the SQl script in memory, this makes it easier to use and store
                    string sqlToRun = GetResourceFile("DoesFileExist.sql");

                    var paramList = new List<SqlParameter>
                    {
                        new SqlParameter("@FileName", SqlDbType.NVarChar) {Value = fileName}
                    };

                    return DbConnection.Execute<bool>(sqlToRun, paramList);
                }
                catch (Exception e)
                {
                    return new DbResultInfoRtn<bool>(e.Message, null);
                }
            }
        }

        public DbResultInfo VerifyBackup(string backupLocation)
        {
            //Change the DB to master
            using (new DbTempChange(DbConnection))
            {
                var paramList = new List<SqlParameter>
                {
                    new SqlParameter("@DBBackupLocation", SqlDbType.NVarChar) {Value = backupLocation}
                };

                return DbConnection.Execute(VerifyDbsql, paramList);
            }
        }

        public DbResultInfoRtn<string> CurrentDatabase()
        {
            return DbConnection.Execute<string>(CurrentDbsql);
        }

        #endregion

        #region PrivateMethods

        private static string GetResourceFile(string filename)
        {
            //We store the SQl script in memory, this makes it easier to use and store
            var assembly = Assembly.GetExecutingAssembly();
            var foundStream = assembly.GetManifestResourceStream("eSightCSdb.SQLScripts." + filename);

            if (foundStream == null)
            {
                return string.Empty;
            }

            var textStreamReader = new StreamReader(foundStream);
            return textStreamReader.ReadToEnd();
        }

        #endregion
    }
}