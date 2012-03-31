using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TinyORM.Connection;
using TinyORM.ConnectionSetup;
using TinyORM.Exceptions;
using TinyORM.Mapping;

namespace TinyORM
{
    public class Db : IDisposable
    {
        public string SqlConnectionTestScript = "SELECT Db_NAME() AS DataBaseName";
        private IConnectionSetup ConnectionSetup { get; set; }
        public ITinyConnection DbConnection { get; set; }
        public TimeSpan CommandTimeout { get; set; }
        public IMapper Mapper { get; set; }

        #region Constructors

        public void Initlialise(IConnectionSetup connectionSetup)
        {
            ConnectionSetup = connectionSetup;
            CommandTimeout = ConnectionSetup.ConnectionTimemout;

            if (ConnectionSetup != null && ConnectionSetup.HasEnoughInformation())
            {
                DbConnection = ConnectionSetup.GetConnection();
                var connectionRes = CheckConnection(true);

                if (!connectionRes.Success)
                    throw connectionRes.AsException;

                //Setup the default mapper
                if (Mapper == null)
                    Mapper = new TinyMapper();
            }
        }

        #endregion

        #region ProcessRequest

        private DbResultInfoRtn<T> ProcessRequest<T>(string commandText, List<SqlParameter> parameters, SqlTransaction transaction)
        {
            DbResultInfoRtn<T> rtnResult;

            if (DbConnection == null)
                return new DbResultInfoRtn<T>("eSightCSDb.Db Connection not initialized", null);
            if (string.IsNullOrEmpty(commandText))
                return new DbResultInfoRtn<T>("No command text found, unable to execute command", null);
            if (Mapper == null)
                return new DbResultInfoRtn<T>("Mapper is not defined", null);

            try
            {
                var connectionRes = CheckConnection();
                if (!connectionRes.Success)
                    throw connectionRes.AsException;

                //Execute the SQL
                var rtnObj = ExecuteSql(commandText, parameters, transaction);

                //Map the db result to the correct type of object
                rtnResult = MapResult<T>(rtnObj);
            }
            catch (Exception e)
            {
                rtnResult = new DbResultInfoRtn<T>(e.Message, e, null);
            }

            return rtnResult;
        }

        #endregion

        #region DbConnectionAndMapping

        private DbResultInfoRtn<T> MapResult<T>(object rtnObj)
        {
            try
            {
                rtnObj = Mapper.Map<T>(rtnObj);
                return new DbResultInfoRtn<T>(string.Empty, rtnObj);
            }
            catch (Exception e)
            {
                //Wrap with a nice error
                throw new TinyMapperException("An error occured while mapping the db value to an object: " + e.Message, e);
            }
        }

        private object ExecuteSql(string commandText, List<SqlParameter> parameters, SqlTransaction transaction)
        {
            try
            {
                return DbConnection.ExecuteScalar(transaction, commandText, parameters, CommandTimeout);
            }
            catch (Exception e)
            {
                //Wrap with a nice error
                throw new TinyDbException("An error occured while executing SQL: " + e.Message, e);
            }
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
            var rtnVal = ProcessRequest<object>(commandText, parameters, transaction);

            //Convert the response into a return value that only conatins the response message
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
            return ProcessRequest<T>(commandText, parameters, transaction);
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
            Exception excptn = null;

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
                    ExecuteSql(SqlConnectionTestScript, null, null);

                    errorMsg = string.Empty;
                    break;
                }
                catch (SqlException e)
                {
                    errorMsg = e.Message;
                    excptn = e;
                }
            }

            return new DbResultInfo(errorMsg, excptn);
        }

        #endregion

        #region IDispose

        public void Dispose()
        {
            try
            {
                if (DbConnection != null)
                    DbConnection.Dispose();
            }
            catch { }
        }

        #endregion
    }
}