using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using TinyORM.Utils;

namespace TinyORM.Connection
{
    public class TinySqlConnection : ITinyConnection
    {
        private SqlConnection DbConnection { get; set; }

        public void Initialize(SqlConnection connection)
        {
            DbConnection = connection;
        }

        public string Database
        {
            get { return DbConnection.Database; }
        }

        public ConnectionState State
        {
            get
            {
                return DbConnection != null ? DbConnection.State : ConnectionState.Broken;
            }
        }

        public IDbCommand CreateCommand()
        {
            return DbConnection != null ? DbConnection.CreateCommand() : null;
        }

        public void Open()
        {
            if (DbConnection != null)
                DbConnection.Open();
        }

        public string ConnectionString
        {
            get
            {
                return DbConnection != null ? DbConnection.ConnectionString : null;
            }
            set {
                if (DbConnection != null)
                    DbConnection.ConnectionString = value;
            }
        }

        public int ConnectionTimeout
        {
            get { return DbConnection != null ? DbConnection.ConnectionTimeout : 0; }
        }

        public void Close()
        {
            if (DbConnection != null)
                DbConnection.Close();
        }

        public object ExecuteScalar(SqlTransaction transaction, string commandText, List<SqlParameter> parameters, TimeSpan timeout)
        {
            if (parameters == null)
                parameters = new List<SqlParameter>();

            if (DbConnection != null)
                return transaction == null ? //TODO: SqlCommandOrUsp needs to be made more generic
                           SqlHelper.ExecuteScalar(DbConnection, DbUtils.SqlCommandOrUsp(commandText), commandText, parameters.ToArray(), (int)timeout.TotalSeconds) :
                           SqlHelper.ExecuteScalar(transaction, DbUtils.SqlCommandOrUsp(commandText), commandText, parameters.ToArray(), (int)timeout.TotalSeconds);

            return null;
        }

        public IDbTransaction BeginTransaction()
        {
            return DbConnection != null ? DbConnection.BeginTransaction() : null;
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return DbConnection == null ? null : DbConnection.BeginTransaction(il);
        }

        public void ChangeDatabase(string dbName)
        {
            if(DbConnection != null)
                DbConnection.ChangeDatabase(dbName);
        }

        public void Dispose()
        {
            if (DbConnection == null) return;

            if (DbConnection.State == ConnectionState.Open)
                Close();

            DbConnection.Dispose();
            DbConnection = null;
        }
    }
}