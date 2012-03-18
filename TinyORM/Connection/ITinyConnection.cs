using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TinyORM.Connection
{
    public interface ITinyConnection : IDbConnection, IDisposable
    {
        void Initialize(SqlConnection connection);
        object ExecuteScalar(SqlTransaction transaction, string commandText, List<SqlParameter> parameters, TimeSpan timeout);
    }
}