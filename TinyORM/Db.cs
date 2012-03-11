using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using TinyORM.Mapping;
using TinyORM.Utils;

namespace TinyORM
{
    public class Db : IDisposable
    {
        private DbConnectionInfo DbInfo { get; set; }
        public SqlConnection DbConnection { get; private set; }
        public TimeSpan CommandTimeout { get; set; }
        public IMapper Mapper { get; set; }

        #region Constructors

        public Db(DbConnectionInfo dbInfo)
        {
            DbInfo = dbInfo;
            CommandTimeout = DbInfo.ConnectionTimemout;

            if (DbInfo != null && DbInfo.HasEnoughInformation())
            {
                DbConnection = DbInfo.GetConnection();
                var connectionRes = CheckConnection(true);

                if (!connectionRes.Success)
                    throw connectionRes.AsException;

                //Setup the default mapper
                if(Mapper == null)
                    Mapper = new TinyMapper();
            }
        }

        #endregion

        #region SqlAccess

        private DbResultInfoRtn<T> ExecuteScalar<T>(string commandText, List<SqlParameter> parameters, SqlTransaction transaction)
        {
            object rtnObj;
            var errorMsg = string.Empty;
            DbResultInfoRtn<T> rtnResult;

            if (DbConnection == null)
                return new DbResultInfoRtn<T>("eSightCSDb.Db Connection not initialized", null);
            if (string.IsNullOrEmpty(commandText))
                return new DbResultInfoRtn<T>("No command text found, unable to execute command", null);
            if (Mapper == null)
                return new DbResultInfoRtn<T>("Mapper is not defined", null);

            var connectionRes = CheckConnection();
            if (!connectionRes.Success)
                throw connectionRes.AsException;

            try
            {
                //Execute the SQL
                rtnObj = ExecuteSql(commandText, parameters, transaction);
            }
            catch (SqlException ex)
            {
                errorMsg = ex.Message;
                rtnObj = null;
            }

            //Seperate catches as the casting in the next part may fail, but we want to return the error back in a DbResultInfoRtn
            try
            {
                rtnObj = Mapper.Map<T>(rtnObj);
                rtnResult = new DbResultInfoRtn<T>(errorMsg, rtnObj);
            }
            catch (Exception e)
            {
                rtnResult = string.IsNullOrEmpty(errorMsg) ? 
                                    new DbResultInfoRtn<T>(e.Message, null) : 
                                    new DbResultInfoRtn<T>(errorMsg, e.Message, null);
            }

            return rtnResult;
        }

        private object ExecuteSql(string commandText, List<SqlParameter> parameters, SqlTransaction transaction)
        {
            if (parameters == null)
                parameters = new List<SqlParameter>();

            return transaction == null ? 
                SqlHelper.ExecuteScalar(DbConnection, DbUtils.SqlCommandOrUsp(commandText), commandText, parameters.ToArray(), (int)CommandTimeout.TotalSeconds) : 
                SqlHelper.ExecuteScalar(transaction, DbUtils.SqlCommandOrUsp(commandText), commandText, parameters.ToArray(), (int)CommandTimeout.TotalSeconds);
        }

        #endregion


        #region ExecuteNoReturn

        public DbResultInfo Execute(string commandText)
        {
            return Execute(commandText, null, null);
        }

        public DbResultInfo Execute(string commandText, SqlTransaction transaction)
        {
            return Execute(commandText, null, transaction);
        }

        public DbResultInfo Execute(string commandText, List<SqlParameter> parameters)
        {
            return Execute(commandText, parameters, null);
        }

        public DbResultInfo Execute(string commandText, List<SqlParameter> parameters, SqlTransaction transaction)
        {
            DbResultInfoRtn<object> rtnVal = ExecuteScalar<object>(commandText, parameters, transaction);

            return new DbResultInfo(rtnVal.ErrorMessage);
        }

        #endregion

        #region ExecuteReturn

        public DbResultInfoRtn<T> Execute<T>(string commandText)
        {
            return Execute<T>(commandText, null, null);
        }

        public DbResultInfoRtn<T> Execute<T>(string commandText, SqlTransaction transaction)
        {
            return Execute<T>(commandText, null, transaction);
        }

        public DbResultInfoRtn<T> Execute<T>(string commandText, List<SqlParameter> parameters)
        {
            return Execute<T>(commandText, parameters, null);
        }

        public DbResultInfoRtn<T> Execute<T>(string commandText, List<SqlParameter> parameters, SqlTransaction transaction)
        {
            return ExecuteScalar<T>(commandText, parameters, transaction);
        }

        #endregion

        #region Connection

        /// <summary>
        /// This function ensures the current connection is connected and alive.
        /// If there is a SqlException it will attempt to reconnect once, if there are
        /// still errors then the function will return Success == false
        /// </summary>
        /// <returns></returns>
        public DbResultInfo CheckConnection()
        {
            return CheckConnection(false);
        }

        /// <summary>
        /// This function ensures the current connection is connected and alive.
        /// If there is a SqlException it will attempt to reconnect once, if there are
        /// still errors then the function will return Success == false
        /// </summary>
        /// <returns></returns>
        public DbResultInfo CheckConnection(bool isStartup)
        {
            var errorMsg = string.Empty;

            //Attempt to connect twice (if the connection is severed, try connecting once more)
            for (var i = 0; i < 2; i++)
            {
                //If the application is testing the connection for the first time
                //dont try the connection more than once
                if (isStartup && i > 0)
                    break;

                try
                {
                    if (DbConnection.State != ConnectionState.Open)
                        DbConnection.Open();

                    //This will cause the Db to execute Sql and test the connection
                    ExecuteSql("SELECT Db_NAME() AS DataBaseName", null, null);
                    
                    errorMsg = string.Empty;
                    break;
                }
                catch (SqlException e)
                {
                    errorMsg = e.Message;
                }
            }

            return new DbResultInfo(errorMsg);
        }

        #endregion

        #region IDispose

        public void Dispose()
        {
            try
            {
                DbConnection.Close();
                DbConnection.Dispose();
                DbConnection = null;
            }
            catch{}
        }

        #endregion
    }
}