﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.ApplicationBlocks.Data;
using TinyORM.Utils;

namespace TinyORM
{
    public class Db
        : IDisposable
    {
        private DbConnectionInfo DbInfo { get; set; }
		private bool IsStartup { get; set; }
        public SqlConnection DbConnection { get; private set; }
        public TimeSpan CommandTimeout { get; set; }

        #region Constructors

        public Db(DbConnectionInfo dbInfo)
        {
			//This stops connection tests from retrying
            IsStartup = true;
            DbInfo = dbInfo;
            CommandTimeout = DbInfo.ConnectionTimemout;

            if (DbInfo != null && DbInfo.HasEnoughInformation())
            {
                DbConnection = DbInfo.GetConnection();
                var connectionRes = CheckConnection();

                if (!connectionRes.Success)
                    throw connectionRes.AsException;
            }

            //This stops connection tests from retrying
            IsStartup = false;
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

            var connectionRes = CheckConnection();
            if (!connectionRes.Success)
                throw connectionRes.AsException;

            if (parameters == null)
                parameters = new List<SqlParameter>();

            try
            {
                rtnObj = IsGenericACollection<T>()
                             ? ExecuteScalarDataset<T>(commandText, parameters, transaction)
                             : ExecuteScalarRegular(commandText, parameters, transaction);
            }
            catch (SqlException ex)
            {
                errorMsg = ex.Message;
                rtnObj = null;
            }

            //Seperate catches as the casting in the next part may fail, but we want to return the error back in a DbResultInfoRtn
            try
            {
                rtnObj = ProcessSqlValue<T>(rtnObj);
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

        private object ExecuteScalarRegular(string commandText, List<SqlParameter> parameters, SqlTransaction transaction)
        {
            object rtnObj;

            if (transaction == null)
                rtnObj = SqlHelper.ExecuteScalar(DbConnection, SqlCommandOrUsp(commandText), commandText, parameters.ToArray(), (int)CommandTimeout.TotalSeconds);
            else
                rtnObj = SqlHelper.ExecuteScalar(transaction, SqlCommandOrUsp(commandText), commandText, parameters.ToArray(), (int)CommandTimeout.TotalSeconds);

            return rtnObj;
        }

        private object ExecuteScalarDataset<T>(string commandText, List<SqlParameter> parameters, SqlTransaction transaction)
        {
            object rtnObj;

            if (transaction == null)
                rtnObj = SqlHelper.ExecuteDataset(DbConnection, SqlCommandOrUsp(commandText), commandText, parameters.ToArray(), (int)CommandTimeout.TotalSeconds);
            else
                rtnObj = SqlHelper.ExecuteDataset(transaction, SqlCommandOrUsp(commandText), commandText, parameters.ToArray(), (int)CommandTimeout.TotalSeconds);

            if (DbUtils.IsSubclassOfRawGeneric(typeof(DataSet), typeof(T)))
                return rtnObj;

            if (DbUtils.IsSubclassOfRawGeneric(typeof(DataTable), typeof(T)))
                return GetDataTable(rtnObj);

            if (DbUtils.IsSubclassOfRawGeneric(typeof(List<>), typeof(T)))
                return ExecuteScalarProcessList<T>(rtnObj);

            if (!typeof(T).IsValueType)
            {
                //Here, we assume we want to try and use reflection to setup a new object with the parameters set
                //from the 1st datatable. Assumed to be a single object, otherwise it would be encapsulated in a list
                var dt = (DataTable)GetDataTable(rtnObj);

                if (dt.Rows.Count > 0)
                    return ConvertDataRowToObject<T>(dt.Columns, dt.Rows[0]);

                //TODO: Work out what to do here
                throw new NotImplementedException();
            }

            return rtnObj;
        }

        //TODO: Refactor into classes
        private object ExecuteScalarProcessList<T>(object rtnObj)
        {
            var dt = (DataTable)GetDataTable(rtnObj);
            var objType = typeof(T).GetGenericArguments()[0];
            var lst = (IList)Activator.CreateInstance((typeof(List<>).MakeGenericType(objType)));

            if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
            {
                var method = GetType().GetMethod("ConvertDataRowToObject", BindingFlags.NonPublic | BindingFlags.Instance);
                var t = method.MakeGenericMethod(new[] { objType });

                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    if (IsValueType(objType))
                        lst.Add(ProcessSqlValue(dt.Rows[i][0]));
                    else
                        lst.Add(t.Invoke(this, new object[] { dt.Columns, dt.Rows[i] }));
                }
            }

            rtnObj = lst;
            return rtnObj;
        }

        #endregion

        #region InternalMethods

        //TODO: Work this out in a standard way
        private const string StoredProcedurePrefix = "usp_";

        private static bool IsGenericACollection<T>()
        {
            if (IsValueType<T>())
                return false;

            //Is it a List?
            if (DbUtils.IsSubclassOfRawGeneric(typeof(List<>), typeof(T)))
                return true;

            //Is it a Datatable?
            if (DbUtils.IsSubclassOfRawGeneric(typeof(DataTable), typeof(T)))
                return true;

            //Is it a Dataset?
            return DbUtils.IsSubclassOfRawGeneric(typeof(DataSet), typeof(T));
        }

        private static bool IsValueType<T>()
        {
            return IsValueType(typeof(T));
        }

        private static bool IsValueType(Type T)
        {
            if (T.IsValueType)
                return true;
            if (T == typeof(string))
                return true;
            if (T == typeof(String))
                return true;

            return false;
        }

        private static object GetDataTable(object rtnObj)
        {
            if (rtnObj != null)
            {
                var tmpDs = (DataSet)rtnObj;
                return tmpDs.Tables.Count > 0 ? tmpDs.Tables[0] : new DataTable();
            }
            return new DataTable();
        }

        private static CommandType SqlCommandOrUsp(string strSql)
        {
            //It's a stored proc with no parameters (separated by a space from the sp name) added on.
            if (strSql.StartsWith(StoredProcedurePrefix, StringComparison.CurrentCultureIgnoreCase) && (!strSql.Contains(" ")))
                return CommandType.StoredProcedure;

            return CommandType.Text;
        }

        private static object ProcessSqlValue(object value)
        {
            if (value is DBNull)
                return null;

            return value;
        }

        private static object ProcessSqlValue<T>(object value)
        {
            if (value is DBNull)
                return null;

            if (typeof(T) == typeof(bool))
            {
                //TODO: Refactor
                if (value is int || value is float || value is double)
                    return ProcessIntAsBool(value);

                if (value is string)
                    return ProcessStringAsBool(value);
            }

            return value;
        }

        private static object ProcessStringAsBool(object value)
        {
            var strVal = (string)value;
            int intParse;

            if (int.TryParse(strVal, out intParse))
                return ProcessIntAsBool(intParse);

            return false;
        }

        private static object ProcessIntAsBool(object value)
        {
            return (int)value == 1;
        }

        private static T ConvertDataRowToObject<T>(DataColumnCollection columnDefs, DataRow dr)
        {
            var t = typeof(T);
            var newObject = (T)FormatterServices.GetUninitializedObject(typeof(T));

            for (var i = 0; i <= columnDefs.Count - 1; i++)
            {
                t.InvokeMember(columnDefs[i].ColumnName,
                               BindingFlags.SetProperty, null,
                               newObject,
                               new[] { InitialiseObject(dr, columnDefs[i].ColumnName) }
                    );
            }

            return newObject;
        }

        private static object InitialiseObject(DataRow dr, String columnName)
        {
            return dr[columnName] == DBNull.Value ? null : dr[columnName];
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
            string errorMsg = string.Empty;

            //Attempt to connect twice (if the connection is severed, try connecting once more)
            for (var i = 0; i < 2; i++)
            {
                //If the application is testing the connection for the first time
                //dont try the connection more than once
                if (IsStartup && i > 0)
                    break;

                try
                {
                    if (DbConnection.State != ConnectionState.Open)
                        DbConnection.Open();

                    //This will cause the Db to execute Sql and test the connection
                    SqlHelper.ExecuteScalar(DbConnection, CommandType.Text, "SELECT Db_NAME() AS DataBaseName");
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
            catch
            {
            }
        }

        #endregion
    }
}