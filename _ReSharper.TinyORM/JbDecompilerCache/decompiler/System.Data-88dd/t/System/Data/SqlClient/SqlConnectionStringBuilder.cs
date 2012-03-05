// Type: System.Data.SqlClient.SqlConnectionStringBuilder
// Assembly: System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v2.0.50727\System.Data.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

namespace System.Data.SqlClient
{
  /// <summary>
  /// Provides a simple way to create and manage the contents of connection strings used by the <see cref="T:System.Data.SqlClient.SqlConnection"/> class.
  /// 
  /// </summary>
  /// <filterpriority>2</filterpriority>
  [TypeConverter(typeof (SqlConnectionStringBuilder.SqlConnectionStringBuilderConverter))]
  [DefaultProperty("DataSource")]
  public sealed class SqlConnectionStringBuilder : DbConnectionStringBuilder
  {
    private static readonly string[] _validKeywords;
    private static readonly Dictionary<string, SqlConnectionStringBuilder.Keywords> _keywords;
    private string _applicationName;
    private string _attachDBFilename;
    private string _currentLanguage;
    private string _dataSource;
    private string _failoverPartner;
    private string _initialCatalog;
    private string _networkLibrary;
    private string _password;
    private string _transactionBinding;
    private string _typeSystemVersion;
    private string _userID;
    private string _workstationID;
    private int _connectTimeout;
    private int _loadBalanceTimeout;
    private int _maxPoolSize;
    private int _minPoolSize;
    private int _packetSize;
    private bool _asynchronousProcessing;
    private bool _connectionReset;
    private bool _contextConnection;
    private bool _encrypt;
    private bool _trustServerCertificate;
    private bool _enlist;
    private bool _integratedSecurity;
    private bool _multipleActiveResultSets;
    private bool _persistSecurityInfo;
    private bool _pooling;
    private bool _replication;
    private bool _userInstance;

    /// <summary>
    /// Gets or sets the value associated with the specified key. In C#, this property is the indexer.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value associated with the specified key.
    /// 
    /// </returns>
    /// <param name="keyword">The key of the item to get or set.
    ///                 </param><exception cref="T:System.ArgumentNullException"><paramref name="keyword"/> is a null reference (Nothing in Visual Basic).
    ///                 </exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">Tried to add a key that does not exist within the available keys.
    ///                 </exception><exception cref="T:System.FormatException">Invalid value within the connection string (specifically, a Boolean or numeric value was expected but not supplied).
    ///                 </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/></PermissionSet>
    public override object this[string keyword]
    {
      get
      {
        return this.GetAt(this.GetIndex(keyword));
      }
      set
      {
        Bid.Trace("<comm.SqlConnectionStringBuilder.set_Item|API> keyword='%ls'\n", keyword);
        if (value != null)
        {
          switch (this.GetIndex(keyword))
          {
            case SqlConnectionStringBuilder.Keywords.DataSource:
              this.DataSource = SqlConnectionStringBuilder.ConvertToString(value);
              break;
            case SqlConnectionStringBuilder.Keywords.FailoverPartner:
              this.FailoverPartner = SqlConnectionStringBuilder.ConvertToString(value);
              break;
            case SqlConnectionStringBuilder.Keywords.AttachDBFilename:
              this.AttachDBFilename = SqlConnectionStringBuilder.ConvertToString(value);
              break;
            case SqlConnectionStringBuilder.Keywords.InitialCatalog:
              this.InitialCatalog = SqlConnectionStringBuilder.ConvertToString(value);
              break;
            case SqlConnectionStringBuilder.Keywords.IntegratedSecurity:
              this.IntegratedSecurity = SqlConnectionStringBuilder.ConvertToIntegratedSecurity(value);
              break;
            case SqlConnectionStringBuilder.Keywords.PersistSecurityInfo:
              this.PersistSecurityInfo = SqlConnectionStringBuilder.ConvertToBoolean(value);
              break;
            case SqlConnectionStringBuilder.Keywords.UserID:
              this.UserID = SqlConnectionStringBuilder.ConvertToString(value);
              break;
            case SqlConnectionStringBuilder.Keywords.Password:
              this.Password = SqlConnectionStringBuilder.ConvertToString(value);
              break;
            case SqlConnectionStringBuilder.Keywords.Enlist:
              this.Enlist = SqlConnectionStringBuilder.ConvertToBoolean(value);
              break;
            case SqlConnectionStringBuilder.Keywords.Pooling:
              this.Pooling = SqlConnectionStringBuilder.ConvertToBoolean(value);
              break;
            case SqlConnectionStringBuilder.Keywords.MinPoolSize:
              this.MinPoolSize = SqlConnectionStringBuilder.ConvertToInt32(value);
              break;
            case SqlConnectionStringBuilder.Keywords.MaxPoolSize:
              this.MaxPoolSize = SqlConnectionStringBuilder.ConvertToInt32(value);
              break;
            case SqlConnectionStringBuilder.Keywords.AsynchronousProcessing:
              this.AsynchronousProcessing = SqlConnectionStringBuilder.ConvertToBoolean(value);
              break;
            case SqlConnectionStringBuilder.Keywords.ConnectionReset:
              this.ConnectionReset = SqlConnectionStringBuilder.ConvertToBoolean(value);
              break;
            case SqlConnectionStringBuilder.Keywords.MultipleActiveResultSets:
              this.MultipleActiveResultSets = SqlConnectionStringBuilder.ConvertToBoolean(value);
              break;
            case SqlConnectionStringBuilder.Keywords.Replication:
              this.Replication = SqlConnectionStringBuilder.ConvertToBoolean(value);
              break;
            case SqlConnectionStringBuilder.Keywords.ConnectTimeout:
              this.ConnectTimeout = SqlConnectionStringBuilder.ConvertToInt32(value);
              break;
            case SqlConnectionStringBuilder.Keywords.Encrypt:
              this.Encrypt = SqlConnectionStringBuilder.ConvertToBoolean(value);
              break;
            case SqlConnectionStringBuilder.Keywords.TrustServerCertificate:
              this.TrustServerCertificate = SqlConnectionStringBuilder.ConvertToBoolean(value);
              break;
            case SqlConnectionStringBuilder.Keywords.LoadBalanceTimeout:
              this.LoadBalanceTimeout = SqlConnectionStringBuilder.ConvertToInt32(value);
              break;
            case SqlConnectionStringBuilder.Keywords.NetworkLibrary:
              this.NetworkLibrary = SqlConnectionStringBuilder.ConvertToString(value);
              break;
            case SqlConnectionStringBuilder.Keywords.PacketSize:
              this.PacketSize = SqlConnectionStringBuilder.ConvertToInt32(value);
              break;
            case SqlConnectionStringBuilder.Keywords.TypeSystemVersion:
              this.TypeSystemVersion = SqlConnectionStringBuilder.ConvertToString(value);
              break;
            case SqlConnectionStringBuilder.Keywords.ApplicationName:
              this.ApplicationName = SqlConnectionStringBuilder.ConvertToString(value);
              break;
            case SqlConnectionStringBuilder.Keywords.CurrentLanguage:
              this.CurrentLanguage = SqlConnectionStringBuilder.ConvertToString(value);
              break;
            case SqlConnectionStringBuilder.Keywords.WorkstationID:
              this.WorkstationID = SqlConnectionStringBuilder.ConvertToString(value);
              break;
            case SqlConnectionStringBuilder.Keywords.UserInstance:
              this.UserInstance = SqlConnectionStringBuilder.ConvertToBoolean(value);
              break;
            case SqlConnectionStringBuilder.Keywords.ContextConnection:
              this.ContextConnection = SqlConnectionStringBuilder.ConvertToBoolean(value);
              break;
            case SqlConnectionStringBuilder.Keywords.TransactionBinding:
              this.TransactionBinding = SqlConnectionStringBuilder.ConvertToString(value);
              break;
            default:
              throw ADP.KeywordNotSupported(keyword);
          }
        }
        else
          this.Remove(keyword);
      }
    }

    /// <summary>
    /// Gets or sets the name of the application associated with the connection string.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The name of the application, or ".NET SqlClient Data Provider" if no name has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [ResDescription("DbConnectionString_ApplicationName")]
    [DisplayName("Application Name")]
    [RefreshProperties(RefreshProperties.All)]
    [ResCategory("DataCategory_Context")]
    public string ApplicationName
    {
      get
      {
        return this._applicationName;
      }
      set
      {
        this.SetValue("Application Name", value);
        this._applicationName = value;
      }
    }

    /// <summary>
    /// Gets or sets a Boolean value that indicates whether asynchronous processing is allowed by the connection created by using this connection string.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.AsynchronousProcessing"/> property, or false if no value has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [ResDescription("DbConnectionString_AsynchronousProcessing")]
    [DisplayName("Asynchronous Processing")]
    [ResCategory("DataCategory_Initialization")]
    [RefreshProperties(RefreshProperties.All)]
    public bool AsynchronousProcessing
    {
      get
      {
        return this._asynchronousProcessing;
      }
      set
      {
        this.SetValue("Asynchronous Processing", value);
        this._asynchronousProcessing = value;
      }
    }

    /// <summary>
    /// Gets or sets a string that contains the name of the primary data file. This includes the full path name of an attachable database.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the AttachDBFilename property, or String.Empty if no value has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [RefreshProperties(RefreshProperties.All)]
    [Editor("System.Windows.Forms.Design.FileNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [ResCategory("DataCategory_Source")]
    [ResDescription("DbConnectionString_AttachDBFilename")]
    [DisplayName("AttachDbFilename")]
    public string AttachDBFilename
    {
      get
      {
        return this._attachDBFilename;
      }
      set
      {
        this.SetValue("AttachDbFilename", value);
        this._attachDBFilename = value;
      }
    }

    /// <summary>
    /// Obsolete. Gets or sets a Boolean value that indicates whether the connection is reset when drawn from the connection pool.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.ConnectionReset"/> property, or true if no value has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [RefreshProperties(RefreshProperties.All)]
    [Obsolete("ConnectionReset has been deprecated.  SqlConnection will ignore the 'connection reset' keyword and always reset the connection")]
    [ResDescription("DbConnectionString_ConnectionReset")]
    [DisplayName("Connection Reset")]
    [ResCategory("DataCategory_Pooling")]
    [Browsable(false)]
    public bool ConnectionReset
    {
      get
      {
        return this._connectionReset;
      }
      set
      {
        this.SetValue("Connection Reset", value);
        this._connectionReset = value;
      }
    }

    /// <summary>
    /// Gets or sets a value that indicates whether a client/server or in-process connection to SQL Server should be made.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.ContextConnection"/> property, or False if none has been supplied.
    /// 
    /// </returns>
    [RefreshProperties(RefreshProperties.All)]
    [ResDescription("DbConnectionString_ContextConnection")]
    [ResCategory("DataCategory_Source")]
    [DisplayName("Context Connection")]
    public bool ContextConnection
    {
      get
      {
        return this._contextConnection;
      }
      set
      {
        this.SetValue("Context Connection", value);
        this._contextConnection = value;
      }
    }

    /// <summary>
    /// Gets or sets the connection timeout.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.ConnectTimeout"/> property, or 15 if no value has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [RefreshProperties(RefreshProperties.All)]
    [ResDescription("DbConnectionString_ConnectTimeout")]
    [DisplayName("Connect Timeout")]
    [ResCategory("DataCategory_Initialization")]
    public int ConnectTimeout
    {
      get
      {
        return this._connectTimeout;
      }
      set
      {
        if (value < 0)
          throw ADP.InvalidConnectionOptionValue("Connect Timeout");
        this.SetValue("Connect Timeout", value);
        this._connectTimeout = value;
      }
    }

    /// <summary>
    /// Gets or sets the SQL Server Language record name.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.CurrentLanguage"/> property, or String.Empty if no value has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [ResDescription("DbConnectionString_CurrentLanguage")]
    [RefreshProperties(RefreshProperties.All)]
    [ResCategory("DataCategory_Initialization")]
    [DisplayName("Current Language")]
    public string CurrentLanguage
    {
      get
      {
        return this._currentLanguage;
      }
      set
      {
        this.SetValue("Current Language", value);
        this._currentLanguage = value;
      }
    }

    /// <summary>
    /// Gets or sets the name or network address of the instance of SQL Server to connect to.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.DataSource"/> property, or String.Empty if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [TypeConverter(typeof (SqlConnectionStringBuilder.SqlDataSourceConverter))]
    [ResDescription("DbConnectionString_DataSource")]
    [ResCategory("DataCategory_Source")]
    [DisplayName("Data Source")]
    [RefreshProperties(RefreshProperties.All)]
    public string DataSource
    {
      get
      {
        return this._dataSource;
      }
      set
      {
        this.SetValue("Data Source", value);
        this._dataSource = value;
      }
    }

    /// <summary>
    /// Gets or sets a Boolean value that indicates whether SQL Server uses SSL encryption for all data sent between the client and server if the server has a certificate installed.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.Encrypt"/> property, or false if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [ResCategory("DataCategory_Security")]
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("Encrypt")]
    [ResDescription("DbConnectionString_Encrypt")]
    public bool Encrypt
    {
      get
      {
        return this._encrypt;
      }
      set
      {
        this.SetValue("Encrypt", value);
        this._encrypt = value;
      }
    }

    /// <summary>
    /// Gets or sets a value that indicates whether the channel will be encrypted while bypassing walking the certificate chain to validate trust.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// A Boolean. Recognized values are true, false, yes, and no.
    /// 
    /// </returns>
    [ResDescription("DbConnectionString_TrustServerCertificate")]
    [RefreshProperties(RefreshProperties.All)]
    [ResCategory("DataCategory_Security")]
    [DisplayName("TrustServerCertificate")]
    public bool TrustServerCertificate
    {
      get
      {
        return this._trustServerCertificate;
      }
      set
      {
        this.SetValue("TrustServerCertificate", value);
        this._trustServerCertificate = value;
      }
    }

    /// <summary>
    /// Gets or sets a Boolean value that indicates whether the SQL Server connection pooler automatically enlists the connection in the creation thread's current transaction context.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.Enlist"/> property, or true if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [DisplayName("Enlist")]
    [RefreshProperties(RefreshProperties.All)]
    [ResCategory("DataCategory_Pooling")]
    [ResDescription("DbConnectionString_Enlist")]
    public bool Enlist
    {
      get
      {
        return this._enlist;
      }
      set
      {
        this.SetValue("Enlist", value);
        this._enlist = value;
      }
    }

    /// <summary>
    /// Gets or sets the name or address of the partner server to connect to if the primary server is down.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.FailoverPartner"/> property, or String.Empty if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>1</filterpriority>
    [ResCategory("DataCategory_Source")]
    [RefreshProperties(RefreshProperties.All)]
    [TypeConverter(typeof (SqlConnectionStringBuilder.SqlDataSourceConverter))]
    [DisplayName("Failover Partner")]
    [ResDescription("DbConnectionString_FailoverPartner")]
    public string FailoverPartner
    {
      get
      {
        return this._failoverPartner;
      }
      set
      {
        this.SetValue("Failover Partner", value);
        this._failoverPartner = value;
      }
    }

    /// <summary>
    /// Gets or sets the name of the database associated with the connection.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.InitialCatalog"/> property, or String.Empty if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [ResDescription("DbConnectionString_InitialCatalog")]
    [DisplayName("Initial Catalog")]
    [TypeConverter(typeof (SqlConnectionStringBuilder.SqlInitialCatalogConverter))]
    [RefreshProperties(RefreshProperties.All)]
    [ResCategory("DataCategory_Source")]
    public string InitialCatalog
    {
      get
      {
        return this._initialCatalog;
      }
      set
      {
        this.SetValue("Initial Catalog", value);
        this._initialCatalog = value;
      }
    }

    /// <summary>
    /// Gets or sets a Boolean value that indicates whether User ID and Password are specified in the connection (when false) or whether the current Windows account credentials are used for authentication (when true).
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.IntegratedSecurity"/> property, or false if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [ResCategory("DataCategory_Security")]
    [DisplayName("Integrated Security")]
    [ResDescription("DbConnectionString_IntegratedSecurity")]
    [RefreshProperties(RefreshProperties.All)]
    public bool IntegratedSecurity
    {
      get
      {
        return this._integratedSecurity;
      }
      set
      {
        this.SetValue("Integrated Security", value);
        this._integratedSecurity = value;
      }
    }

    /// <summary>
    /// Gets or sets the minimum time, in seconds, for the connection to live in the connection pool before being destroyed.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.LoadBalanceTimeout"/> property, or 0 if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [ResCategory("DataCategory_Pooling")]
    [ResDescription("DbConnectionString_LoadBalanceTimeout")]
    [DisplayName("Load Balance Timeout")]
    [RefreshProperties(RefreshProperties.All)]
    public int LoadBalanceTimeout
    {
      get
      {
        return this._loadBalanceTimeout;
      }
      set
      {
        if (value < 0)
          throw ADP.InvalidConnectionOptionValue("Load Balance Timeout");
        this.SetValue("Load Balance Timeout", value);
        this._loadBalanceTimeout = value;
      }
    }

    /// <summary>
    /// Gets or sets the maximum number of connections allowed in the connection pool for this specific connection string.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.MaxPoolSize"/> property, or 100 if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [ResCategory("DataCategory_Pooling")]
    [DisplayName("Max Pool Size")]
    [RefreshProperties(RefreshProperties.All)]
    [ResDescription("DbConnectionString_MaxPoolSize")]
    public int MaxPoolSize
    {
      get
      {
        return this._maxPoolSize;
      }
      set
      {
        if (value < 1)
          throw ADP.InvalidConnectionOptionValue("Max Pool Size");
        this.SetValue("Max Pool Size", value);
        this._maxPoolSize = value;
      }
    }

    /// <summary>
    /// Gets or sets the minimum number of connections allowed in the connection pool for this specific connection string.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.MinPoolSize"/> property, or 0 if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [ResDescription("DbConnectionString_MinPoolSize")]
    [RefreshProperties(RefreshProperties.All)]
    [ResCategory("DataCategory_Pooling")]
    [DisplayName("Min Pool Size")]
    public int MinPoolSize
    {
      get
      {
        return this._minPoolSize;
      }
      set
      {
        if (value < 0)
          throw ADP.InvalidConnectionOptionValue("Min Pool Size");
        this.SetValue("Min Pool Size", value);
        this._minPoolSize = value;
      }
    }

    /// <summary>
    /// Gets or sets a Boolean value that indicates whether multiple active result sets can be associated with the associated connection.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.MultipleActiveResultSets"/> property, or false if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [RefreshProperties(RefreshProperties.All)]
    [ResCategory("DataCategory_Advanced")]
    [ResDescription("DbConnectionString_MultipleActiveResultSets")]
    [DisplayName("MultipleActiveResultSets")]
    public bool MultipleActiveResultSets
    {
      get
      {
        return this._multipleActiveResultSets;
      }
      set
      {
        this.SetValue("MultipleActiveResultSets", value);
        this._multipleActiveResultSets = value;
      }
    }

    /// <summary>
    /// Gets or sets a string that contains the name of the network library used to establish a connection to the SQL Server.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.NetworkLibrary"/> property, or String.Empty if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [RefreshProperties(RefreshProperties.All)]
    [ResCategory("DataCategory_Advanced")]
    [DisplayName("Network Library")]
    [TypeConverter(typeof (SqlConnectionStringBuilder.NetworkLibraryConverter))]
    [ResDescription("DbConnectionString_NetworkLibrary")]
    public string NetworkLibrary
    {
      get
      {
        return this._networkLibrary;
      }
      set
      {
        if (value != null)
        {
          switch (value.Trim().ToLower(CultureInfo.InvariantCulture))
          {
            case "dbmsadsn":
              value = "dbmsadsn";
              break;
            case "dbmsvinn":
              value = "dbmsvinn";
              break;
            case "dbmsspxn":
              value = "dbmsspxn";
              break;
            case "dbmsrpcn":
              value = "dbmsrpcn";
              break;
            case "dbnmpntw":
              value = "dbnmpntw";
              break;
            case "dbmslpcn":
              value = "dbmslpcn";
              break;
            case "dbmssocn":
              value = "dbmssocn";
              break;
            case "dbmsgnet":
              value = "dbmsgnet";
              break;
            default:
              throw ADP.InvalidConnectionOptionValue("Network Library");
          }
        }
        this.SetValue("Network Library", value);
        this._networkLibrary = value;
      }
    }

    /// <summary>
    /// Gets or sets the size in bytes of the network packets used to communicate with an instance of SQL Server.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.PacketSize"/> property, or 8000 if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [DisplayName("Packet Size")]
    [RefreshProperties(RefreshProperties.All)]
    [ResDescription("DbConnectionString_PacketSize")]
    [ResCategory("DataCategory_Advanced")]
    public int PacketSize
    {
      get
      {
        return this._packetSize;
      }
      set
      {
        if (value < 512 || 32768 < value)
          throw SQL.InvalidPacketSizeValue();
        this.SetValue("Packet Size", value);
        this._packetSize = value;
      }
    }

    /// <summary>
    /// Gets or sets the password for the SQL Server account.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.Password"/> property, or String.Empty if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [ResDescription("DbConnectionString_Password")]
    [ResCategory("DataCategory_Security")]
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("Password")]
    [PasswordPropertyText(true)]
    public string Password
    {
      get
      {
        return this._password;
      }
      set
      {
        this.SetValue("Password", value);
        this._password = value;
      }
    }

    /// <summary>
    /// Gets or sets a Boolean value that indicates if security-sensitive information, such as the password, is not returned as part of the connection if the connection is open or has ever been in an open state.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.PersistSecurityInfo"/> property, or false if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [DisplayName("Persist Security Info")]
    [ResCategory("DataCategory_Security")]
    [RefreshProperties(RefreshProperties.All)]
    [ResDescription("DbConnectionString_PersistSecurityInfo")]
    public bool PersistSecurityInfo
    {
      get
      {
        return this._persistSecurityInfo;
      }
      set
      {
        this.SetValue("Persist Security Info", value);
        this._persistSecurityInfo = value;
      }
    }

    /// <summary>
    /// Gets or sets a Boolean value that indicates whether the connection will be pooled or explicitly opened every time that the connection is requested.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.Pooling"/> property, or true if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [ResDescription("DbConnectionString_Pooling")]
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("Pooling")]
    [ResCategory("DataCategory_Pooling")]
    public bool Pooling
    {
      get
      {
        return this._pooling;
      }
      set
      {
        this.SetValue("Pooling", value);
        this._pooling = value;
      }
    }

    /// <summary>
    /// Gets or sets a Boolean value that indicates whether replication is supported using the connection.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.Replication"/> property, or false if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [ResCategory("DataCategory_Replication")]
    [RefreshProperties(RefreshProperties.All)]
    [DisplayName("Replication")]
    [ResDescription("DbConnectionString_Replication")]
    public bool Replication
    {
      get
      {
        return this._replication;
      }
      set
      {
        this.SetValue("Replication", value);
        this._replication = value;
      }
    }

    /// <summary>
    /// Gets or sets a string value that indicates how the connection maintains its association with an enlisted System.Transactions transaction.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.TransactionBinding"/> property, or String.Empty if none has been supplied.
    /// 
    /// </returns>
    [ResDescription("DbConnectionString_TransactionBinding")]
    [DisplayName("Transaction Binding")]
    [RefreshProperties(RefreshProperties.All)]
    [ResCategory("DataCategory_Advanced")]
    public string TransactionBinding
    {
      get
      {
        return this._transactionBinding;
      }
      set
      {
        this.SetValue("Transaction Binding", value);
        this._transactionBinding = value;
      }
    }

    /// <summary>
    /// Gets or sets a string value that indicates the type system the application expects.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The following table shows the possible values for the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.TypeSystemVersion"/> property:
    /// 
    ///                     Value
    /// 
    ///                     Description
    /// 
    ///                     SQL Server 2000
    /// 
    ///                     Uses the SQL Server 2000 type system. The following comparisons will be performed when connecting to a SQL Server 2005 instance:
    /// 
    ///                     XML to NTEXT
    /// 
    ///                     UDT to VARBINARY
    /// 
    ///                     VARCHAR(MAX), NVARCHAR(MAX) and VARBINARY(MAX) to TEXT, NEXT and IMAGE respectively.
    /// 
    ///                     SQL Server 2005
    /// 
    ///                     Uses the SQL Server 2005 type system. No conversions are made for the current version of ADO.NET.
    /// 
    ///                     SQL Server 2008
    /// 
    ///                     Uses the SQL Server 2008 type system.
    /// 
    ///                     Latest
    /// 
    ///                     Use the latest version than this client-server pair can handle. This will automatically move forward as the client and server components are upgraded.
    /// 
    /// </returns>
    [RefreshProperties(RefreshProperties.All)]
    [ResDescription("DbConnectionString_TypeSystemVersion")]
    [DisplayName("Type System Version")]
    [ResCategory("DataCategory_Advanced")]
    public string TypeSystemVersion
    {
      get
      {
        return this._typeSystemVersion;
      }
      set
      {
        this.SetValue("Type System Version", value);
        this._typeSystemVersion = value;
      }
    }

    /// <summary>
    /// Gets or sets the user ID to be used when connecting to SQL Server.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.UserID"/> property, or String.Empty if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [RefreshProperties(RefreshProperties.All)]
    [ResCategory("DataCategory_Security")]
    [ResDescription("DbConnectionString_UserID")]
    [DisplayName("User ID")]
    public string UserID
    {
      get
      {
        return this._userID;
      }
      set
      {
        this.SetValue("User ID", value);
        this._userID = value;
      }
    }

    /// <summary>
    /// Gets or sets a value that indicates whether to redirect the connection from the default SQL Server Express instance to a runtime-initiated instance running under the account of the caller.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.UserInstance"/> property, or False if none has been supplied.
    /// 
    /// </returns>
    [RefreshProperties(RefreshProperties.All)]
    [ResDescription("DbConnectionString_UserInstance")]
    [ResCategory("DataCategory_Source")]
    [DisplayName("User Instance")]
    public bool UserInstance
    {
      get
      {
        return this._userInstance;
      }
      set
      {
        this.SetValue("User Instance", value);
        this._userInstance = value;
      }
    }

    /// <summary>
    /// Gets or sets the name of the workstation connecting to SQL Server.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.WorkstationID"/> property, or String.Empty if none has been supplied.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [RefreshProperties(RefreshProperties.All)]
    [ResDescription("DbConnectionString_WorkstationID")]
    [DisplayName("Workstation ID")]
    [ResCategory("DataCategory_Context")]
    public string WorkstationID
    {
      get
      {
        return this._workstationID;
      }
      set
      {
        this.SetValue("Workstation ID", value);
        this._workstationID = value;
      }
    }

    /// <summary>
    /// Gets a value that indicates whether the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/> has a fixed size.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// true in every case, because the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/> supplies a fixed-size collection of key/value pairs.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public override bool IsFixedSize
    {
      get
      {
        return true;
      }
    }

    /// <summary>
    /// Gets an <see cref="T:System.Collections.ICollection"/> that contains the keys in the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/>.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:System.Collections.ICollection"/> that contains the keys in the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/>.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public override ICollection Keys
    {
      get
      {
        return (ICollection) new ReadOnlyCollection<string>(SqlConnectionStringBuilder._validKeywords);
      }
    }

    /// <summary>
    /// Gets an <see cref="T:System.Collections.ICollection"/> that contains the values in the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/>.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:System.Collections.ICollection"/> that contains the values in the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/>.
    /// 
    /// </returns>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/></PermissionSet>
    public override ICollection Values
    {
      get
      {
        object[] items = new object[SqlConnectionStringBuilder._validKeywords.Length];
        for (int index = 0; index < items.Length; ++index)
          items[index] = this.GetAt((SqlConnectionStringBuilder.Keywords) index);
        return (ICollection) new ReadOnlyCollection<object>(items);
      }
    }

    static SqlConnectionStringBuilder()
    {
      string[] strArray = new string[29];
      strArray[23] = "Application Name";
      strArray[12] = "Asynchronous Processing";
      strArray[2] = "AttachDbFilename";
      strArray[13] = "Connection Reset";
      strArray[27] = "Context Connection";
      strArray[16] = "Connect Timeout";
      strArray[24] = "Current Language";
      strArray[0] = "Data Source";
      strArray[17] = "Encrypt";
      strArray[8] = "Enlist";
      strArray[1] = "Failover Partner";
      strArray[3] = "Initial Catalog";
      strArray[4] = "Integrated Security";
      strArray[19] = "Load Balance Timeout";
      strArray[11] = "Max Pool Size";
      strArray[10] = "Min Pool Size";
      strArray[14] = "MultipleActiveResultSets";
      strArray[20] = "Network Library";
      strArray[21] = "Packet Size";
      strArray[7] = "Password";
      strArray[5] = "Persist Security Info";
      strArray[9] = "Pooling";
      strArray[15] = "Replication";
      strArray[28] = "Transaction Binding";
      strArray[18] = "TrustServerCertificate";
      strArray[22] = "Type System Version";
      strArray[6] = "User ID";
      strArray[26] = "User Instance";
      strArray[25] = "Workstation ID";
      SqlConnectionStringBuilder._validKeywords = strArray;
      SqlConnectionStringBuilder._keywords = new Dictionary<string, SqlConnectionStringBuilder.Keywords>(50, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
      {
        {
          "Application Name",
          SqlConnectionStringBuilder.Keywords.ApplicationName
        },
        {
          "Asynchronous Processing",
          SqlConnectionStringBuilder.Keywords.AsynchronousProcessing
        },
        {
          "AttachDbFilename",
          SqlConnectionStringBuilder.Keywords.AttachDBFilename
        },
        {
          "Connect Timeout",
          SqlConnectionStringBuilder.Keywords.ConnectTimeout
        },
        {
          "Connection Reset",
          SqlConnectionStringBuilder.Keywords.ConnectionReset
        },
        {
          "Context Connection",
          SqlConnectionStringBuilder.Keywords.ContextConnection
        },
        {
          "Current Language",
          SqlConnectionStringBuilder.Keywords.CurrentLanguage
        },
        {
          "Data Source",
          SqlConnectionStringBuilder.Keywords.DataSource
        },
        {
          "Encrypt",
          SqlConnectionStringBuilder.Keywords.Encrypt
        },
        {
          "Enlist",
          SqlConnectionStringBuilder.Keywords.Enlist
        },
        {
          "Failover Partner",
          SqlConnectionStringBuilder.Keywords.FailoverPartner
        },
        {
          "Initial Catalog",
          SqlConnectionStringBuilder.Keywords.InitialCatalog
        },
        {
          "Integrated Security",
          SqlConnectionStringBuilder.Keywords.IntegratedSecurity
        },
        {
          "Load Balance Timeout",
          SqlConnectionStringBuilder.Keywords.LoadBalanceTimeout
        },
        {
          "MultipleActiveResultSets",
          SqlConnectionStringBuilder.Keywords.MultipleActiveResultSets
        },
        {
          "Max Pool Size",
          SqlConnectionStringBuilder.Keywords.MaxPoolSize
        },
        {
          "Min Pool Size",
          SqlConnectionStringBuilder.Keywords.MinPoolSize
        },
        {
          "Network Library",
          SqlConnectionStringBuilder.Keywords.NetworkLibrary
        },
        {
          "Packet Size",
          SqlConnectionStringBuilder.Keywords.PacketSize
        },
        {
          "Password",
          SqlConnectionStringBuilder.Keywords.Password
        },
        {
          "Persist Security Info",
          SqlConnectionStringBuilder.Keywords.PersistSecurityInfo
        },
        {
          "Pooling",
          SqlConnectionStringBuilder.Keywords.Pooling
        },
        {
          "Replication",
          SqlConnectionStringBuilder.Keywords.Replication
        },
        {
          "Transaction Binding",
          SqlConnectionStringBuilder.Keywords.TransactionBinding
        },
        {
          "TrustServerCertificate",
          SqlConnectionStringBuilder.Keywords.TrustServerCertificate
        },
        {
          "Type System Version",
          SqlConnectionStringBuilder.Keywords.TypeSystemVersion
        },
        {
          "User ID",
          SqlConnectionStringBuilder.Keywords.UserID
        },
        {
          "User Instance",
          SqlConnectionStringBuilder.Keywords.UserInstance
        },
        {
          "Workstation ID",
          SqlConnectionStringBuilder.Keywords.WorkstationID
        },
        {
          "app",
          SqlConnectionStringBuilder.Keywords.ApplicationName
        },
        {
          "async",
          SqlConnectionStringBuilder.Keywords.AsynchronousProcessing
        },
        {
          "extended properties",
          SqlConnectionStringBuilder.Keywords.AttachDBFilename
        },
        {
          "initial file name",
          SqlConnectionStringBuilder.Keywords.AttachDBFilename
        },
        {
          "connection timeout",
          SqlConnectionStringBuilder.Keywords.ConnectTimeout
        },
        {
          "timeout",
          SqlConnectionStringBuilder.Keywords.ConnectTimeout
        },
        {
          "language",
          SqlConnectionStringBuilder.Keywords.CurrentLanguage
        },
        {
          "addr",
          SqlConnectionStringBuilder.Keywords.DataSource
        },
        {
          "address",
          SqlConnectionStringBuilder.Keywords.DataSource
        },
        {
          "network address",
          SqlConnectionStringBuilder.Keywords.DataSource
        },
        {
          "server",
          SqlConnectionStringBuilder.Keywords.DataSource
        },
        {
          "database",
          SqlConnectionStringBuilder.Keywords.InitialCatalog
        },
        {
          "trusted_connection",
          SqlConnectionStringBuilder.Keywords.IntegratedSecurity
        },
        {
          "connection lifetime",
          SqlConnectionStringBuilder.Keywords.LoadBalanceTimeout
        },
        {
          "net",
          SqlConnectionStringBuilder.Keywords.NetworkLibrary
        },
        {
          "network",
          SqlConnectionStringBuilder.Keywords.NetworkLibrary
        },
        {
          "pwd",
          SqlConnectionStringBuilder.Keywords.Password
        },
        {
          "persistsecurityinfo",
          SqlConnectionStringBuilder.Keywords.PersistSecurityInfo
        },
        {
          "uid",
          SqlConnectionStringBuilder.Keywords.UserID
        },
        {
          "user",
          SqlConnectionStringBuilder.Keywords.UserID
        },
        {
          "wsid",
          SqlConnectionStringBuilder.Keywords.WorkstationID
        }
      };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/> class.
    /// 
    /// </summary>
    public SqlConnectionStringBuilder()
      : this((string) null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/> class. The provided connection string provides the data for the instance's internal connection information.
    /// 
    /// </summary>
    /// <param name="connectionString">The basis for the object's internal connection information. Parsed into name/value pairs. Invalid key names raise <see cref="T:System.Collections.Generic.KeyNotFoundException"/>.
    ///                 </param><exception cref="T:System.Collections.Generic.KeyNotFoundException">Invalid key name within the connection string.
    ///                 </exception><exception cref="T:System.FormatException">Invalid value within the connection string (specifically, when a Boolean or numeric value was expected but not supplied).
    ///                 </exception><exception cref="T:System.ArgumentException">The supplied <paramref name="connectionString"/> is not valid.
    ///                 </exception>
    public SqlConnectionStringBuilder(string connectionString)
    {
      if (ADP.IsEmpty(connectionString))
        return;
      this.ConnectionString = connectionString;
    }

    /// <summary>
    /// Clears the contents of the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/> instance.
    /// 
    /// </summary>
    /// <filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/></PermissionSet>
    public override void Clear()
    {
      base.Clear();
      for (int index = 0; index < SqlConnectionStringBuilder._validKeywords.Length; ++index)
        this.Reset((SqlConnectionStringBuilder.Keywords) index);
    }

    /// <summary>
    /// Determines whether the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/> contains a specific key.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// true if the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/> contains an element that has the specified key; otherwise, false.
    /// 
    /// </returns>
    /// <param name="keyword">The key to locate in the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/>.
    ///                 </param><exception cref="T:System.ArgumentNullException"><paramref name="keyword"/> is null (Nothing in Visual Basic)
    ///                 </exception><filterpriority>1</filterpriority>
    public override bool ContainsKey(string keyword)
    {
      ADP.CheckArgumentNull((object) keyword, "keyword");
      return SqlConnectionStringBuilder._keywords.ContainsKey(keyword);
    }

    private static bool ConvertToBoolean(object value)
    {
      return DbConnectionStringBuilderUtil.ConvertToBoolean(value);
    }

    private static int ConvertToInt32(object value)
    {
      return DbConnectionStringBuilderUtil.ConvertToInt32(value);
    }

    private static bool ConvertToIntegratedSecurity(object value)
    {
      return DbConnectionStringBuilderUtil.ConvertToIntegratedSecurity(value);
    }

    private static string ConvertToString(object value)
    {
      return DbConnectionStringBuilderUtil.ConvertToString(value);
    }

    private object GetAt(SqlConnectionStringBuilder.Keywords index)
    {
      switch (index)
      {
        case SqlConnectionStringBuilder.Keywords.DataSource:
          return (object) this.DataSource;
        case SqlConnectionStringBuilder.Keywords.FailoverPartner:
          return (object) this.FailoverPartner;
        case SqlConnectionStringBuilder.Keywords.AttachDBFilename:
          return (object) this.AttachDBFilename;
        case SqlConnectionStringBuilder.Keywords.InitialCatalog:
          return (object) this.InitialCatalog;
        case SqlConnectionStringBuilder.Keywords.IntegratedSecurity:
          return (object) (bool) (this.IntegratedSecurity ? 1 : 0);
        case SqlConnectionStringBuilder.Keywords.PersistSecurityInfo:
          return (object) (bool) (this.PersistSecurityInfo ? 1 : 0);
        case SqlConnectionStringBuilder.Keywords.UserID:
          return (object) this.UserID;
        case SqlConnectionStringBuilder.Keywords.Password:
          return (object) this.Password;
        case SqlConnectionStringBuilder.Keywords.Enlist:
          return (object) (bool) (this.Enlist ? 1 : 0);
        case SqlConnectionStringBuilder.Keywords.Pooling:
          return (object) (bool) (this.Pooling ? 1 : 0);
        case SqlConnectionStringBuilder.Keywords.MinPoolSize:
          return (object) this.MinPoolSize;
        case SqlConnectionStringBuilder.Keywords.MaxPoolSize:
          return (object) this.MaxPoolSize;
        case SqlConnectionStringBuilder.Keywords.AsynchronousProcessing:
          return (object) (bool) (this.AsynchronousProcessing ? 1 : 0);
        case SqlConnectionStringBuilder.Keywords.ConnectionReset:
          return (object) (bool) (this.ConnectionReset ? 1 : 0);
        case SqlConnectionStringBuilder.Keywords.MultipleActiveResultSets:
          return (object) (bool) (this.MultipleActiveResultSets ? 1 : 0);
        case SqlConnectionStringBuilder.Keywords.Replication:
          return (object) (bool) (this.Replication ? 1 : 0);
        case SqlConnectionStringBuilder.Keywords.ConnectTimeout:
          return (object) this.ConnectTimeout;
        case SqlConnectionStringBuilder.Keywords.Encrypt:
          return (object) (bool) (this.Encrypt ? 1 : 0);
        case SqlConnectionStringBuilder.Keywords.TrustServerCertificate:
          return (object) (bool) (this.TrustServerCertificate ? 1 : 0);
        case SqlConnectionStringBuilder.Keywords.LoadBalanceTimeout:
          return (object) this.LoadBalanceTimeout;
        case SqlConnectionStringBuilder.Keywords.NetworkLibrary:
          return (object) this.NetworkLibrary;
        case SqlConnectionStringBuilder.Keywords.PacketSize:
          return (object) this.PacketSize;
        case SqlConnectionStringBuilder.Keywords.TypeSystemVersion:
          return (object) this.TypeSystemVersion;
        case SqlConnectionStringBuilder.Keywords.ApplicationName:
          return (object) this.ApplicationName;
        case SqlConnectionStringBuilder.Keywords.CurrentLanguage:
          return (object) this.CurrentLanguage;
        case SqlConnectionStringBuilder.Keywords.WorkstationID:
          return (object) this.WorkstationID;
        case SqlConnectionStringBuilder.Keywords.UserInstance:
          return (object) (bool) (this.UserInstance ? 1 : 0);
        case SqlConnectionStringBuilder.Keywords.ContextConnection:
          return (object) (bool) (this.ContextConnection ? 1 : 0);
        case SqlConnectionStringBuilder.Keywords.TransactionBinding:
          return (object) this.TransactionBinding;
        default:
          throw ADP.KeywordNotSupported(SqlConnectionStringBuilder._validKeywords[(int) index]);
      }
    }

    private SqlConnectionStringBuilder.Keywords GetIndex(string keyword)
    {
      ADP.CheckArgumentNull((object) keyword, "keyword");
      SqlConnectionStringBuilder.Keywords keywords;
      if (SqlConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
        return keywords;
      else
        throw ADP.KeywordNotSupported(keyword);
    }

    protected override void GetProperties(Hashtable propertyDescriptors)
    {
      foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties((object) this, true))
      {
        bool flag = false;
        string displayName = propertyDescriptor.DisplayName;
        bool isReadOnly;
        if ("Integrated Security" == displayName)
        {
          flag = true;
          isReadOnly = propertyDescriptor.IsReadOnly;
        }
        else if ("Password" == displayName || "User ID" == displayName)
          isReadOnly = this.IntegratedSecurity;
        else
          continue;
        Attribute[] attributesFromCollection = this.GetAttributesFromCollection(propertyDescriptor.Attributes);
        propertyDescriptors[(object) displayName] = (object) new DbConnectionStringBuilderDescriptor(propertyDescriptor.Name, propertyDescriptor.ComponentType, propertyDescriptor.PropertyType, isReadOnly, attributesFromCollection)
        {
          RefreshOnChange = flag
        };
      }
      base.GetProperties(propertyDescriptors);
    }

    /// <summary>
    /// Removes the entry with the specified key from the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/> instance.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// true if the key existed within the connection string and was removed; false if the key did not exist.
    /// 
    /// </returns>
    /// <param name="keyword">The key of the key/value pair to be removed from the connection string in this <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/>.
    ///                 </param><exception cref="T:System.ArgumentNullException"><paramref name="keyword"/> is null (Nothing in Visual Basic)
    ///                 </exception><filterpriority>1</filterpriority>
    public override bool Remove(string keyword)
    {
      ADP.CheckArgumentNull((object) keyword, "keyword");
      SqlConnectionStringBuilder.Keywords index;
      if (!SqlConnectionStringBuilder._keywords.TryGetValue(keyword, out index) || !base.Remove(SqlConnectionStringBuilder._validKeywords[(int) index]))
        return false;
      this.Reset(index);
      return true;
    }

    private void Reset(SqlConnectionStringBuilder.Keywords index)
    {
      switch (index)
      {
        case SqlConnectionStringBuilder.Keywords.DataSource:
          this._dataSource = "";
          break;
        case SqlConnectionStringBuilder.Keywords.FailoverPartner:
          this._failoverPartner = "";
          break;
        case SqlConnectionStringBuilder.Keywords.AttachDBFilename:
          this._attachDBFilename = "";
          break;
        case SqlConnectionStringBuilder.Keywords.InitialCatalog:
          this._initialCatalog = "";
          break;
        case SqlConnectionStringBuilder.Keywords.IntegratedSecurity:
          this._integratedSecurity = false;
          break;
        case SqlConnectionStringBuilder.Keywords.PersistSecurityInfo:
          this._persistSecurityInfo = false;
          break;
        case SqlConnectionStringBuilder.Keywords.UserID:
          this._userID = "";
          break;
        case SqlConnectionStringBuilder.Keywords.Password:
          this._password = "";
          break;
        case SqlConnectionStringBuilder.Keywords.Enlist:
          this._enlist = true;
          break;
        case SqlConnectionStringBuilder.Keywords.Pooling:
          this._pooling = true;
          break;
        case SqlConnectionStringBuilder.Keywords.MinPoolSize:
          this._minPoolSize = 0;
          break;
        case SqlConnectionStringBuilder.Keywords.MaxPoolSize:
          this._maxPoolSize = 100;
          break;
        case SqlConnectionStringBuilder.Keywords.AsynchronousProcessing:
          this._asynchronousProcessing = false;
          break;
        case SqlConnectionStringBuilder.Keywords.ConnectionReset:
          this._connectionReset = true;
          break;
        case SqlConnectionStringBuilder.Keywords.MultipleActiveResultSets:
          this._multipleActiveResultSets = false;
          break;
        case SqlConnectionStringBuilder.Keywords.Replication:
          this._replication = false;
          break;
        case SqlConnectionStringBuilder.Keywords.ConnectTimeout:
          this._connectTimeout = 15;
          break;
        case SqlConnectionStringBuilder.Keywords.Encrypt:
          this._encrypt = false;
          break;
        case SqlConnectionStringBuilder.Keywords.TrustServerCertificate:
          this._trustServerCertificate = false;
          break;
        case SqlConnectionStringBuilder.Keywords.LoadBalanceTimeout:
          this._loadBalanceTimeout = 0;
          break;
        case SqlConnectionStringBuilder.Keywords.NetworkLibrary:
          this._networkLibrary = "";
          break;
        case SqlConnectionStringBuilder.Keywords.PacketSize:
          this._packetSize = 8000;
          break;
        case SqlConnectionStringBuilder.Keywords.TypeSystemVersion:
          this._typeSystemVersion = "Latest";
          break;
        case SqlConnectionStringBuilder.Keywords.ApplicationName:
          this._applicationName = ".Net SqlClient Data Provider";
          break;
        case SqlConnectionStringBuilder.Keywords.CurrentLanguage:
          this._currentLanguage = "";
          break;
        case SqlConnectionStringBuilder.Keywords.WorkstationID:
          this._workstationID = "";
          break;
        case SqlConnectionStringBuilder.Keywords.UserInstance:
          this._userInstance = false;
          break;
        case SqlConnectionStringBuilder.Keywords.ContextConnection:
          this._contextConnection = false;
          break;
        case SqlConnectionStringBuilder.Keywords.TransactionBinding:
          this._transactionBinding = "Implicit Unbind";
          break;
        default:
          throw ADP.KeywordNotSupported(SqlConnectionStringBuilder._validKeywords[(int) index]);
      }
    }

    private void SetValue(string keyword, bool value)
    {
      base[keyword] = (object) value.ToString((IFormatProvider) null);
    }

    private void SetValue(string keyword, int value)
    {
      base[keyword] = (object) value.ToString((IFormatProvider) null);
    }

    private void SetValue(string keyword, string value)
    {
      ADP.CheckArgumentNull((object) value, keyword);
      base[keyword] = (object) value;
    }

    /// <summary>
    /// Indicates whether the specified key exists in this <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/> instance.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// true if the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/> contains an entry with the specified key; otherwise, false.
    /// 
    /// </returns>
    /// <param name="keyword">The key to locate in the <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/>.
    ///                 </param><filterpriority>1</filterpriority>
    public override bool ShouldSerialize(string keyword)
    {
      ADP.CheckArgumentNull((object) keyword, "keyword");
      SqlConnectionStringBuilder.Keywords keywords;
      if (SqlConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
        return base.ShouldSerialize(SqlConnectionStringBuilder._validKeywords[(int) keywords]);
      else
        return false;
    }

    /// <summary>
    /// Retrieves a value corresponding to the supplied key from this <see cref="T:System.Data.SqlClient.SqlConnectionStringBuilder"/>.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// true if <paramref name="keyword"/> was found within the connection string; otherwise, false.
    /// 
    /// </returns>
    /// <param name="keyword">The key of the item to retrieve.
    ///                 </param><param name="value">The value corresponding to <paramref name="keyword."/></param><exception cref="T:System.ArgumentNullException"><paramref name="keyword"/> contains a null value (Nothing in Visual Basic).
    ///                 </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/></PermissionSet>
    public override bool TryGetValue(string keyword, out object value)
    {
      SqlConnectionStringBuilder.Keywords index;
      if (SqlConnectionStringBuilder._keywords.TryGetValue(keyword, out index))
      {
        value = this.GetAt(index);
        return true;
      }
      else
      {
        value = (object) null;
        return false;
      }
    }

    private enum Keywords
    {
      DataSource,
      FailoverPartner,
      AttachDBFilename,
      InitialCatalog,
      IntegratedSecurity,
      PersistSecurityInfo,
      UserID,
      Password,
      Enlist,
      Pooling,
      MinPoolSize,
      MaxPoolSize,
      AsynchronousProcessing,
      ConnectionReset,
      MultipleActiveResultSets,
      Replication,
      ConnectTimeout,
      Encrypt,
      TrustServerCertificate,
      LoadBalanceTimeout,
      NetworkLibrary,
      PacketSize,
      TypeSystemVersion,
      ApplicationName,
      CurrentLanguage,
      WorkstationID,
      UserInstance,
      ContextConnection,
      TransactionBinding,
    }

    private sealed class NetworkLibraryConverter : TypeConverter
    {
      private const string NamedPipes = "Named Pipes (DBNMPNTW)";
      private const string SharedMemory = "Shared Memory (DBMSSOCN)";
      private const string TCPIP = "TCP/IP (DBMSGNET)";
      private const string VIA = "VIA (DBMSGNET)";
      private TypeConverter.StandardValuesCollection _standardValues;

      public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
      {
        if (typeof (string) != sourceType)
          return base.CanConvertFrom(context, sourceType);
        else
          return true;
      }

      public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
      {
        string str = value as string;
        if (str == null)
          return base.ConvertFrom(context, culture, value);
        string x = str.Trim();
        if (StringComparer.OrdinalIgnoreCase.Equals(x, "Named Pipes (DBNMPNTW)"))
          return (object) "dbnmpntw";
        if (StringComparer.OrdinalIgnoreCase.Equals(x, "Shared Memory (DBMSSOCN)"))
          return (object) "dbmslpcn";
        if (StringComparer.OrdinalIgnoreCase.Equals(x, "TCP/IP (DBMSGNET)"))
          return (object) "dbmssocn";
        if (StringComparer.OrdinalIgnoreCase.Equals(x, "VIA (DBMSGNET)"))
          return (object) "dbmsgnet";
        else
          return (object) x;
      }

      public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
      {
        if (typeof (string) != destinationType)
          return base.CanConvertTo(context, destinationType);
        else
          return true;
      }

      public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
      {
        string str = value as string;
        if (str == null || destinationType != typeof (string))
          return base.ConvertTo(context, culture, value, destinationType);
        switch (str.Trim().ToLower(CultureInfo.InvariantCulture))
        {
          case "dbnmpntw":
            return (object) "Named Pipes (DBNMPNTW)";
          case "dbmslpcn":
            return (object) "Shared Memory (DBMSSOCN)";
          case "dbmssocn":
            return (object) "TCP/IP (DBMSGNET)";
          case "dbmsgnet":
            return (object) "VIA (DBMSGNET)";
          default:
            return (object) str;
        }
      }

      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
      {
        return true;
      }

      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
      {
        return false;
      }

      public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
      {
        if (context != null)
        {
          object instance = context.Instance;
        }
        TypeConverter.StandardValuesCollection valuesCollection = this._standardValues;
        if (valuesCollection == null)
        {
          valuesCollection = new TypeConverter.StandardValuesCollection((ICollection) new string[4]
          {
            "Named Pipes (DBNMPNTW)",
            "Shared Memory (DBMSSOCN)",
            "TCP/IP (DBMSGNET)",
            "VIA (DBMSGNET)"
          });
          this._standardValues = valuesCollection;
        }
        return valuesCollection;
      }
    }

    private sealed class SqlDataSourceConverter : StringConverter
    {
      private TypeConverter.StandardValuesCollection _standardValues;

      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
      {
        return true;
      }

      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
      {
        return false;
      }

      public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
      {
        TypeConverter.StandardValuesCollection valuesCollection = this._standardValues;
        if (this._standardValues == null)
        {
          DataTable dataSources = SqlClientFactory.Instance.CreateDataSourceEnumerator().GetDataSources();
          DataColumn index1 = dataSources.Columns["ServerName"];
          DataColumn index2 = dataSources.Columns["InstanceName"];
          DataRowCollection rows = dataSources.Rows;
          string[] array = new string[rows.Count];
          for (int index3 = 0; index3 < array.Length; ++index3)
          {
            string str1 = rows[index3][index1] as string;
            string str2 = rows[index3][index2] as string;
            array[index3] = str2 == null || str2.Length == 0 || "MSSQLSERVER" == str2 ? str1 : str1 + "\\" + str2;
          }
          Array.Sort<string>(array);
          valuesCollection = new TypeConverter.StandardValuesCollection((ICollection) array);
          this._standardValues = valuesCollection;
        }
        return valuesCollection;
      }
    }

    private sealed class SqlInitialCatalogConverter : StringConverter
    {
      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
      {
        return this.GetStandardValuesSupportedInternal(context);
      }

      private bool GetStandardValuesSupportedInternal(ITypeDescriptorContext context)
      {
        bool flag = false;
        if (context != null)
        {
          SqlConnectionStringBuilder connectionStringBuilder = context.Instance as SqlConnectionStringBuilder;
          if (connectionStringBuilder != null && 0 < connectionStringBuilder.DataSource.Length && (connectionStringBuilder.IntegratedSecurity || 0 < connectionStringBuilder.UserID.Length))
            flag = true;
        }
        return flag;
      }

      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
      {
        return false;
      }

      public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
      {
        if (!this.GetStandardValuesSupportedInternal(context))
          return (TypeConverter.StandardValuesCollection) null;
        List<string> list = new List<string>();
        try
        {
          SqlConnectionStringBuilder connectionStringBuilder = (SqlConnectionStringBuilder) context.Instance;
          using (SqlConnection connection = new SqlConnection())
          {
            connection.ConnectionString = connectionStringBuilder.ConnectionString;
            using (SqlCommand sqlCommand = new SqlCommand("SELECT name FROM master.dbo.sysdatabases ORDER BY name", connection))
            {
              connection.Open();
              using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
              {
                while (sqlDataReader.Read())
                  list.Add(sqlDataReader.GetString(0));
              }
            }
          }
        }
        catch (SqlException ex)
        {
          ADP.TraceExceptionWithoutRethrow((Exception) ex);
        }
        return new TypeConverter.StandardValuesCollection((ICollection) list);
      }
    }

    internal sealed class SqlConnectionStringBuilderConverter : ExpandableObjectConverter
    {
      public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
      {
        if (typeof (InstanceDescriptor) == destinationType)
          return true;
        else
          return base.CanConvertTo(context, destinationType);
      }

      public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
      {
        if (destinationType == null)
          throw ADP.ArgumentNull("destinationType");
        if (typeof (InstanceDescriptor) == destinationType)
        {
          SqlConnectionStringBuilder options = value as SqlConnectionStringBuilder;
          if (options != null)
            return (object) this.ConvertToInstanceDescriptor(options);
        }
        return base.ConvertTo(context, culture, value, destinationType);
      }

      private InstanceDescriptor ConvertToInstanceDescriptor(SqlConnectionStringBuilder options)
      {
        return new InstanceDescriptor((MemberInfo) typeof (SqlConnectionStringBuilder).GetConstructor(new Type[1]
        {
          typeof (string)
        }), (ICollection) new object[1]
        {
          (object) options.ConnectionString
        });
      }
    }
  }
}
