using System;
using System.Data;

namespace TinyORM.Maintenance
{
    /// <summary>
    /// This class will change the current DB in-use, and then when the class is disposed it will restore it to the
    /// original DB
    /// SC
    /// </summary>
    public class DbTempChange
        : IDisposable
    {
        private const string Master = "MASTER";
        private Db DbConnection { get; set; }
        private string RestoreDbName { get; set; }
        private TimeSpan? CommandTimeout { get; set; }

        #region Constructors

        /// <summary>
        /// Change the DB to master
        /// </summary>
        /// <param name="dbConnection"></param>
        public DbTempChange(Db dbConnection)
        {
            DbConnection = dbConnection;
            ChangeDb(string.Empty);
        }

        /// <summary>
        /// Change the DB to dbName
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbName"></param>
        public DbTempChange(Db dbConnection, string dbName)
        {
            DbConnection = dbConnection;
            ChangeDb(dbName);
        }

        public DbTempChange(Db dbConnection, string dbName, TimeSpan timeout)
        {
            DbConnection = dbConnection;
            ChangeDb(dbName);
            ChangeTimeout(timeout);
        }

        public DbTempChange(Db dbConnection, TimeSpan timeout)
        {
            DbConnection = dbConnection;
            ChangeTimeout(timeout);
        }

        #endregion

        #region ChangeDB

        private void ChangeDb(string dbName)
        {
            if (string.IsNullOrEmpty(DbConnection.DbConnection.Database))
            {
                var currentDbResult = new DbMaintenance(DbConnection).CurrentDatabase();
                if (currentDbResult.Success)
                {
                    if (!string.IsNullOrEmpty(currentDbResult.Value))
                        RestoreDbName = currentDbResult.Value;
                    else
                        throw new Exception("No DB name found");
                }
                else
                    throw currentDbResult.AsException;
            }
            else
                RestoreDbName = DbConnection.DbConnection.Database;

            //If the string is empty, set the DB to master
            SetDb(string.IsNullOrEmpty(dbName) ? Master : dbName);
        }

        private void SetDb(string dbName)
        {
            try
            {
                if (dbName.ToUpper() != DbConnection.DbConnection.Database.ToUpper())
                {
                    DbConnection.CheckConnection();

                    if (DbConnection.DbConnection.State == ConnectionState.Open)
                        DbConnection.DbConnection.ChangeDatabase(dbName);
                }
            }
            catch
            {
            }
        }

        #endregion

        #region ChangeTimeout

        private void ChangeTimeout(TimeSpan time)
        {
            if (!CommandTimeout.HasValue)
                CommandTimeout = DbConnection.CommandTimeout;

            DbConnection.CommandTimeout = time;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            if (RestoreDbName != null)
            {
                //If the connection isn't open, then open it to make sure we set the default connection DB back to what is was
                SetDb(RestoreDbName);
            }

            if (CommandTimeout.HasValue)
                ChangeTimeout(CommandTimeout.Value);
        }

        #endregion
    }
}