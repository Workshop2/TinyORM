// Type: Microsoft.ApplicationBlocks.Data.SqlHelper
// Assembly: Microsoft.ApplicationBlocks.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: F:\My Documents\Visual Studio 2010\Projects\TinyORM\Microsoft.ApplicationBlocks.Data.dll

using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Microsoft.ApplicationBlocks.Data
{
  public sealed class SqlHelper
  {
    private const int DEFAULT_SQL_TIMEOUT = 90;

    private SqlHelper()
    {
    }

    private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
    {
      if (command == null)
        throw new ArgumentNullException("command");
      if (commandParameters == null)
        return;
      SqlParameter[] sqlParameterArray = commandParameters;
      int index = 0;
      while (index < sqlParameterArray.Length)
      {
        SqlParameter sqlParameter = sqlParameterArray[index];
        if (sqlParameter != null)
        {
          if ((sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Input) && sqlParameter.Value == null || false)
            sqlParameter.Value = (object) DBNull.Value;
          command.Parameters.Add(sqlParameter);
        }
        checked { ++index; }
      }
    }

    private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
    {
      if ((commandParameters == null || dataRow == null) && true)
        return;
      SqlParameter[] sqlParameterArray = commandParameters;
      int index = 0;
      while (index < sqlParameterArray.Length)
      {
        SqlParameter sqlParameter = sqlParameterArray[index];
        int num;
        if ((sqlParameter.ParameterName == null || sqlParameter.ParameterName.Length <= 1) && true)
          throw new Exception(string.Format("Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: ' {1}' .", (object) num, (object) sqlParameter.ParameterName));
        if (dataRow.Table.Columns.IndexOf(sqlParameter.ParameterName.Substring(1)) != -1)
          sqlParameter.Value = RuntimeHelpers.GetObjectValue(dataRow[sqlParameter.ParameterName.Substring(1)]);
        checked { ++num; }
        checked { ++index; }
      }
    }

    private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
    {
      if (commandParameters == null && parameterValues == null || false)
        return;
      if (commandParameters.Length != parameterValues.Length)
        throw new ArgumentException("Parameter count does not match Parameter Value count.");
      int num1 = checked (commandParameters.Length - 1);
      int num2 = 0;
      int num3 = num1;
      int index = num2;
      while (index <= num3)
      {
        if (parameterValues[index] is IDbDataParameter)
        {
          IDbDataParameter dbDataParameter = (IDbDataParameter) parameterValues[index];
          commandParameters[index].Value = dbDataParameter.Value != null ? RuntimeHelpers.GetObjectValue(dbDataParameter.Value) : (object) DBNull.Value;
        }
        else
          commandParameters[index].Value = parameterValues[index] != null ? RuntimeHelpers.GetObjectValue(parameterValues[index]) : (object) DBNull.Value;
        checked { ++index; }
      }
    }

    private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, ref SqlParameter[] commandParameters, ref bool mustCloseConnection, int sqlTimeout = 90)
    {
      if (command == null)
        throw new ArgumentNullException("command");
      if ((commandText == null || commandText.Length == 0) && true)
        throw new ArgumentNullException("commandText");
      if (connection.State != ConnectionState.Open)
      {
        connection.Open();
        mustCloseConnection = true;
      }
      else
        mustCloseConnection = false;
      command.Connection = connection;
      command.CommandText = commandText;
      if (transaction != null)
      {
        if (transaction.Connection == null)
          throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        command.Transaction = transaction;
      }
      command.CommandType = commandType;
      command.CommandTimeout = sqlTimeout;
      if (commandParameters == null)
        return;
      SqlHelper.AttachParameters(command, commandParameters);
    }

    public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
    {
      return SqlHelper.ExecuteNonQuery(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      SqlConnection connection = (SqlConnection) null;
      try
      {
        connection = new SqlConnection(connectionString);
        connection.Open();
        return SqlHelper.ExecuteNonQuery(connection, commandType, commandText, commandParameters, 90);
      }
      finally
      {
        if (connection != null)
          connection.Dispose();
      }
    }

    public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if ((parameterValues == null || parameterValues.Length <= 0) && !false)
        return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
    {
      return SqlHelper.ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[]) null, 90);
    }

    public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters, int sqlTimeout = 90)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      SqlCommand command = new SqlCommand();
      bool mustCloseConnection = false;
      SqlHelper.PrepareCommand(command, connection, (SqlTransaction) null, commandType, commandText, ref commandParameters, ref mustCloseConnection, sqlTimeout);
      int num = command.ExecuteNonQuery();
      command.Parameters.Clear();
      if (mustCloseConnection)
        connection.Close();
      return num;
    }

    public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if ((parameterValues == null || parameterValues.Length <= 0) && !false)
        return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet, 90);
    }

    public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
    {
      SqlTransaction transaction1 = transaction;
      int num = (int) commandType;
      string commandText1 = commandText;
      SqlParameter[] sqlParameterArray = (SqlParameter[]) null;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      SqlParameter[]& commandParameters = @sqlParameterArray;
      int sqlTimeout = 90;
      return SqlHelper.ExecuteNonQuery(transaction1, (CommandType) num, commandText1, commandParameters, sqlTimeout);
    }

    public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, ref SqlParameter[] commandParameters, int sqlTimeout = 90)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      SqlCommand command = new SqlCommand();
      bool mustCloseConnection = false;
      SqlHelper.PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, ref commandParameters, ref mustCloseConnection, sqlTimeout);
      int num = command.ExecuteNonQuery();
      command.Parameters.Clear();
      return num;
    }

    public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if ((parameterValues == null || parameterValues.Length <= 0) && !false)
        return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, ref spParameterSet, 90);
    }

    public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, int sqlTimeout = 90)
    {
      return SqlHelper.ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[]) null, sqlTimeout);
    }

    public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, SqlParameter[] commandParameters, int sqlTimeout = 90)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      SqlConnection connection = (SqlConnection) null;
      try
      {
        connection = new SqlConnection(connectionString);
        connection.Open();
        return SqlHelper.ExecuteDataset(connection, commandType, commandText, commandParameters, sqlTimeout);
      }
      finally
      {
        if (connection != null)
          connection.Dispose();
      }
    }

    public static DataSet ExecuteDataset(string connectionString, string spName, object[] parameterValues, int sqlTimeout = 90)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if ((parameterValues == null || parameterValues.Length <= 0) && !false)
        return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, sqlTimeout);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, spParameterSet, sqlTimeout);
    }

    public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, int sqlTimeout = 90)
    {
      return SqlHelper.ExecuteDataset(connection, commandType, commandText, (SqlParameter[]) null, sqlTimeout);
    }

    public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters, int sqlTimeout = 90)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      SqlCommand sqlCommand = new SqlCommand();
      DataSet dataSet = new DataSet();
      SqlDataAdapter sqlDataAdapter = (SqlDataAdapter) null;
      bool mustCloseConnection = false;
      SqlHelper.PrepareCommand(sqlCommand, connection, (SqlTransaction) null, commandType, commandText, ref commandParameters, ref mustCloseConnection, sqlTimeout);
      try
      {
        sqlDataAdapter = new SqlDataAdapter(sqlCommand);
        sqlDataAdapter.Fill(dataSet);
        sqlCommand.Parameters.Clear();
      }
      finally
      {
        if (sqlDataAdapter != null)
          sqlDataAdapter.Dispose();
      }
      if (mustCloseConnection)
        connection.Close();
      return dataSet;
    }

    public static DataSet ExecuteDataset(SqlConnection connection, string spName, object[] parameterValues, int sqlTimeout = 90)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if ((parameterValues == null || parameterValues.Length <= 0) && !false)
        return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, 90);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet, sqlTimeout);
    }

    public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, int sqlTimeout = 90)
    {
      return SqlHelper.ExecuteDataset(transaction, commandType, commandText, (SqlParameter[]) null, sqlTimeout);
    }

    public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, int sqlTimeout = 90)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      SqlCommand sqlCommand = new SqlCommand();
      DataSet dataSet = new DataSet();
      SqlDataAdapter sqlDataAdapter = (SqlDataAdapter) null;
      bool mustCloseConnection = false;
      SqlHelper.PrepareCommand(sqlCommand, transaction.Connection, transaction, commandType, commandText, ref commandParameters, ref mustCloseConnection, sqlTimeout);
      try
      {
        sqlDataAdapter = new SqlDataAdapter(sqlCommand);
        sqlDataAdapter.Fill(dataSet);
        sqlCommand.Parameters.Clear();
      }
      finally
      {
        if (sqlDataAdapter != null)
          sqlDataAdapter.Dispose();
      }
      return dataSet;
    }

    public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, object[] parameterValues, int sqlTimeout = 90)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if ((parameterValues == null || parameterValues.Length <= 0) && !false)
        return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, sqlTimeout);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet, sqlTimeout);
    }

    private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlHelper.SqlConnectionOwnership connectionOwnership, int sqlTimeout = 90)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      bool mustCloseConnection = false;
      SqlCommand command = new SqlCommand();
      try
      {
        SqlHelper.PrepareCommand(command, connection, transaction, commandType, commandText, ref commandParameters, ref mustCloseConnection, sqlTimeout);
        SqlDataReader sqlDataReader = connectionOwnership != SqlHelper.SqlConnectionOwnership.External ? command.ExecuteReader(CommandBehavior.CloseConnection) : command.ExecuteReader();
        bool flag = true;
        try
        {
          foreach (SqlParameter sqlParameter in command.Parameters)
          {
            if (sqlParameter.Direction != ParameterDirection.Input)
              flag = false;
          }
        }
        finally
        {
          IEnumerator enumerator;
          if (enumerator is IDisposable)
            (enumerator as IDisposable).Dispose();
        }
        if (flag)
          command.Parameters.Clear();
        return sqlDataReader;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        if (mustCloseConnection)
          connection.Close();
        throw;
      }
    }

    public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
    {
      return SqlHelper.ExecuteReader(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      SqlConnection connection = (SqlConnection) null;
      try
      {
        connection = new SqlConnection(connectionString);
        connection.Open();
        return SqlHelper.ExecuteReader(connection, (SqlTransaction) null, commandType, commandText, commandParameters, SqlHelper.SqlConnectionOwnership.Internal, 90);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        if (connection != null)
          connection.Dispose();
        throw;
      }
    }

    public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if ((parameterValues == null || parameterValues.Length <= 0) && !false)
        return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
    {
      return SqlHelper.ExecuteReader(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
      return SqlHelper.ExecuteReader(connection, (SqlTransaction) null, commandType, commandText, commandParameters, SqlHelper.SqlConnectionOwnership.External, 90);
    }

    public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if ((parameterValues == null || parameterValues.Length <= 0) && !false)
        return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
    {
      return SqlHelper.ExecuteReader(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      else
        return SqlHelper.ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlHelper.SqlConnectionOwnership.External, 90);
    }

    public static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if ((parameterValues == null || parameterValues.Length <= 0) && !false)
        return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
    {
      return SqlHelper.ExecuteScalar(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      SqlConnection connection = (SqlConnection) null;
      try
      {
        connection = new SqlConnection(connectionString);
        connection.Open();
        return SqlHelper.ExecuteScalar(connection, commandType, commandText, commandParameters, 90);
      }
      finally
      {
        if (connection != null)
          connection.Dispose();
      }
    }

    public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if ((parameterValues == null || parameterValues.Length <= 0) && !false)
        return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
    {
      return SqlHelper.ExecuteScalar(connection, commandType, commandText, (SqlParameter[]) null, 90);
    }

    public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters, int sqlTimeout = 90)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      SqlCommand command = new SqlCommand();
      bool mustCloseConnection = false;
      SqlHelper.PrepareCommand(command, connection, (SqlTransaction) null, commandType, commandText, ref commandParameters, ref mustCloseConnection, sqlTimeout);
      object objectValue = RuntimeHelpers.GetObjectValue(command.ExecuteScalar());
      command.Parameters.Clear();
      if (mustCloseConnection)
        connection.Close();
      return objectValue;
    }

    public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if ((parameterValues == null || parameterValues.Length <= 0) && !false)
        return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet, 90);
    }

    public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
    {
      return SqlHelper.ExecuteScalar(transaction, commandType, commandText, (SqlParameter[]) null, 90);
    }

    public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, int sqlTimeout = 90)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      SqlCommand command = new SqlCommand();
      bool mustCloseConnection = false;
      SqlHelper.PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, ref commandParameters, ref mustCloseConnection, sqlTimeout);
      object objectValue = RuntimeHelpers.GetObjectValue(command.ExecuteScalar());
      command.Parameters.Clear();
      return objectValue;
    }

    public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if ((parameterValues == null || parameterValues.Length <= 0) && !false)
        return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet, 90);
    }

    public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
    {
      return SqlHelper.ExecuteXmlReader(connection, commandType, commandText, (SqlParameter[]) null, 90);
    }

    public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters, int sqlTimeout = 90)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      SqlCommand command = new SqlCommand();
      bool mustCloseConnection = false;
      try
      {
        SqlHelper.PrepareCommand(command, connection, (SqlTransaction) null, commandType, commandText, ref commandParameters, ref mustCloseConnection, sqlTimeout);
        XmlReader xmlReader = command.ExecuteXmlReader();
        command.Parameters.Clear();
        return xmlReader;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        if (mustCloseConnection)
          connection.Close();
        throw;
      }
    }

    public static XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if ((parameterValues == null || parameterValues.Length <= 0) && !false)
        return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, spParameterSet, 90);
    }

    public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
    {
      return SqlHelper.ExecuteXmlReader(transaction, commandType, commandText, (SqlParameter[]) null, 90);
    }

    public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, int sqlTimeout = 90)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      SqlCommand command = new SqlCommand();
      bool mustCloseConnection = false;
      SqlHelper.PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, ref commandParameters, ref mustCloseConnection, sqlTimeout);
      XmlReader xmlReader = command.ExecuteXmlReader();
      command.Parameters.Clear();
      return xmlReader;
    }

    public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if ((parameterValues == null || parameterValues.Length <= 0) && !false)
        return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, spParameterSet, 90);
    }

    public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      if (dataSet == null)
        throw new ArgumentNullException("dataSet");
      SqlConnection connection = (SqlConnection) null;
      try
      {
        connection = new SqlConnection(connectionString);
        connection.Open();
        SqlHelper.FillDataset(connection, commandType, commandText, dataSet, tableNames);
      }
      finally
      {
        if (connection != null)
          connection.Dispose();
      }
    }

    public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      if (dataSet == null)
        throw new ArgumentNullException("dataSet");
      SqlConnection connection = (SqlConnection) null;
      try
      {
        connection = new SqlConnection(connectionString);
        connection.Open();
        SqlHelper.FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
      }
      finally
      {
        if (connection != null)
          connection.Dispose();
      }
    }

    public static void FillDataset(string connectionString, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      if (dataSet == null)
        throw new ArgumentNullException("dataSet");
      SqlConnection connection = (SqlConnection) null;
      try
      {
        connection = new SqlConnection(connectionString);
        connection.Open();
        SqlHelper.FillDataset(connection, spName, dataSet, tableNames, parameterValues);
      }
      finally
      {
        if (connection != null)
          connection.Dispose();
      }
    }

    public static void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
    {
      SqlHelper.FillDataset(connection, commandType, commandText, dataSet, tableNames, (SqlParameter[]) null);
    }

    public static void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
    {
      SqlHelper.FillDataset(connection, (SqlTransaction) null, commandType, commandText, dataSet, tableNames, commandParameters, 90);
    }

    public static void FillDataset(SqlConnection connection, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (dataSet == null)
        throw new ArgumentNullException("dataSet");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if (parameterValues != null && parameterValues.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
        SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
        SqlHelper.FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, spParameterSet);
      }
      else
        SqlHelper.FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
    }

    public static void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
    {
      SqlHelper.FillDataset(transaction, commandType, commandText, dataSet, tableNames, (SqlParameter[]) null);
    }

    public static void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      SqlHelper.FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters, 90);
    }

    public static void FillDataset(SqlTransaction transaction, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if (dataSet == null)
        throw new ArgumentNullException("dataSet");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      if (parameterValues != null && parameterValues.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
        SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
        SqlHelper.FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, spParameterSet);
      }
      else
        SqlHelper.FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
    }

    private static void FillDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, SqlParameter[] commandParameters, int sqlTimeout = 90)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (dataSet == null)
        throw new ArgumentNullException("dataSet");
      SqlCommand sqlCommand = new SqlCommand();
      bool mustCloseConnection = false;
      SqlHelper.PrepareCommand(sqlCommand, connection, transaction, commandType, commandText, ref commandParameters, ref mustCloseConnection, sqlTimeout);
      SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
      try
      {
        if (tableNames != null && tableNames.Length > 0 || false)
        {
          string sourceTable = "Table";
          int num1 = 0;
          int num2 = checked (tableNames.Length - 1);
          int index = num1;
          while (index <= num2)
          {
            if ((tableNames[index] == null || tableNames[index].Length == 0) && true)
              throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
            sqlDataAdapter.TableMappings.Add(sourceTable, tableNames[index]);
            sourceTable = sourceTable + checked (index + 1).ToString();
            checked { ++index; }
          }
        }
        sqlDataAdapter.Fill(dataSet);
        sqlCommand.Parameters.Clear();
      }
      finally
      {
        if (sqlDataAdapter != null)
          sqlDataAdapter.Dispose();
      }
      if (!mustCloseConnection)
        return;
      connection.Close();
    }

    public static void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
    {
      if (insertCommand == null)
        throw new ArgumentNullException("insertCommand");
      if (deleteCommand == null)
        throw new ArgumentNullException("deleteCommand");
      if (updateCommand == null)
        throw new ArgumentNullException("updateCommand");
      if (dataSet == null)
        throw new ArgumentNullException("dataSet");
      if ((tableName == null || tableName.Length == 0) && true)
        throw new ArgumentNullException("tableName");
      SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
      try
      {
        sqlDataAdapter.UpdateCommand = updateCommand;
        sqlDataAdapter.InsertCommand = insertCommand;
        sqlDataAdapter.DeleteCommand = deleteCommand;
        sqlDataAdapter.Update(dataSet, tableName);
        dataSet.AcceptChanges();
      }
      finally
      {
        if (sqlDataAdapter != null)
          sqlDataAdapter.Dispose();
      }
    }

    public static SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      SqlCommand command = new SqlCommand(spName, connection);
      command.CommandType = CommandType.StoredProcedure;
      if (sourceColumns != null && sourceColumns.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
        int num1 = 0;
        int num2 = checked (sourceColumns.Length - 1);
        int index = num1;
        while (index <= num2)
        {
          spParameterSet[index].SourceColumn = sourceColumns[index];
          checked { ++index; }
        }
        SqlHelper.AttachParameters(command, spParameterSet);
      }
      return command;
    }

    public static int ExecuteNonQueryTypedParams(string connectionString, string spName, DataRow dataRow)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      int num;
      if (dataRow != null && dataRow.ItemArray.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
        SqlHelper.AssignParameterValues(spParameterSet, dataRow);
        num = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
      }
      else
        num = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
      return num;
    }

    public static int ExecuteNonQueryTypedParams(SqlConnection connection, string spName, DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      int num;
      if (dataRow != null && dataRow.ItemArray.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
        SqlHelper.AssignParameterValues(spParameterSet, dataRow);
        num = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet, 90);
      }
      else
        num = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
      return num;
    }

    public static int ExecuteNonQueryTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      int num;
      if (dataRow != null && dataRow.ItemArray.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
        SqlHelper.AssignParameterValues(spParameterSet, dataRow);
        num = SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, ref spParameterSet, 90);
      }
      else
        num = SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
      return num;
    }

    public static DataSet ExecuteDatasetTypedParams(string connectionString, string spName, DataRow dataRow)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      DataSet dataSet;
      if (dataRow != null && dataRow.ItemArray.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
        SqlHelper.AssignParameterValues(spParameterSet, dataRow);
        dataSet = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, spParameterSet, 90);
      }
      else
        dataSet = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, 90);
      return dataSet;
    }

    public static DataSet ExecuteDatasetTypedParams(SqlConnection connection, string spName, DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      DataSet dataSet;
      if (dataRow != null && dataRow.ItemArray.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
        SqlHelper.AssignParameterValues(spParameterSet, dataRow);
        dataSet = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet, 90);
      }
      else
        dataSet = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, 90);
      return dataSet;
    }

    public static DataSet ExecuteDatasetTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      DataSet dataSet;
      if (dataRow != null && dataRow.ItemArray.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
        SqlHelper.AssignParameterValues(spParameterSet, dataRow);
        dataSet = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet, 90);
      }
      else
        dataSet = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, 90);
      return dataSet;
    }

    public static SqlDataReader ExecuteReaderTypedParams(string connectionString, string spName, DataRow dataRow)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      SqlDataReader sqlDataReader;
      if (dataRow != null && dataRow.ItemArray.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
        SqlHelper.AssignParameterValues(spParameterSet, dataRow);
        sqlDataReader = SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
      }
      else
        sqlDataReader = SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
      return sqlDataReader;
    }

    public static SqlDataReader ExecuteReaderTypedParams(SqlConnection connection, string spName, DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      SqlDataReader sqlDataReader;
      if (dataRow != null && dataRow.ItemArray.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
        SqlHelper.AssignParameterValues(spParameterSet, dataRow);
        sqlDataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
      }
      else
        sqlDataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName);
      return sqlDataReader;
    }

    public static SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      SqlDataReader sqlDataReader;
      if (dataRow != null && dataRow.ItemArray.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
        SqlHelper.AssignParameterValues(spParameterSet, dataRow);
        sqlDataReader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
      }
      else
        sqlDataReader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
      return sqlDataReader;
    }

    public static object ExecuteScalarTypedParams(string connectionString, string spName, DataRow dataRow)
    {
      if ((connectionString == null || connectionString.Length == 0) && true)
        throw new ArgumentNullException("connectionString");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      object objectValue;
      if (dataRow != null && dataRow.ItemArray.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
        SqlHelper.AssignParameterValues(spParameterSet, dataRow);
        objectValue = RuntimeHelpers.GetObjectValue(SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, spParameterSet));
      }
      else
        objectValue = RuntimeHelpers.GetObjectValue(SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName));
      return objectValue;
    }

    public static object ExecuteScalarTypedParams(SqlConnection connection, string spName, DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      object objectValue;
      if (dataRow != null && dataRow.ItemArray.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
        SqlHelper.AssignParameterValues(spParameterSet, dataRow);
        objectValue = RuntimeHelpers.GetObjectValue(SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet, 90));
      }
      else
        objectValue = RuntimeHelpers.GetObjectValue(SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName));
      return objectValue;
    }

    public static object ExecuteScalarTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      object objectValue;
      if (dataRow != null && dataRow.ItemArray.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
        SqlHelper.AssignParameterValues(spParameterSet, dataRow);
        objectValue = RuntimeHelpers.GetObjectValue(SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet, 90));
      }
      else
        objectValue = RuntimeHelpers.GetObjectValue(SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName));
      return objectValue;
    }

    public static XmlReader ExecuteXmlReaderTypedParams(SqlConnection connection, string spName, DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      XmlReader xmlReader;
      if (dataRow != null && dataRow.ItemArray.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
        SqlHelper.AssignParameterValues(spParameterSet, dataRow);
        xmlReader = SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, spParameterSet, 90);
      }
      else
        xmlReader = SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
      return xmlReader;
    }

    public static XmlReader ExecuteXmlReaderTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null || false)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if ((spName == null || spName.Length == 0) && true)
        throw new ArgumentNullException("spName");
      XmlReader xmlReader;
      if (dataRow != null && dataRow.ItemArray.Length > 0 || false)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
        SqlHelper.AssignParameterValues(spParameterSet, dataRow);
        xmlReader = SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, spParameterSet, 90);
      }
      else
        xmlReader = SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
      return xmlReader;
    }

    private enum SqlConnectionOwnership
    {
      Internal,
      External,
    }
  }
}
