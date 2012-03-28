using System;
using System.Data.SqlClient;
using System.Text;
using TinyORM.Connection;

namespace TinyORM.ConnectionSetup
{
    public class SqlDbConnectionSetup : IConnectionSetup
    {
        #region Properties

        private const string DefaultDb = "MASTER";
        private const string ProviderIgnore = "Provider=SQLOLEDB.1;";
        public TimeSpan ConnectionTimemout { get; set; }

        public string Domain { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Should the connection string use Windows Authority instead of SQL Authority
        /// </summary>
        public bool UseWindowsAuthentication { get; set; }

        #endregion

        #region Constructor

        public SqlDbConnectionSetup()
        {
            ConnectionTimemout = new TimeSpan(0, 0, 2, 0);
        }

        public SqlDbConnectionSetup(string domain, string database, string username, string password, bool useWindowsAuthentication)
            : this()
        {
            Domain = domain;
            Database = database;
            Username = username;
            Password = password;
            UseWindowsAuthentication = useWindowsAuthentication;
        }

        public SqlDbConnectionSetup(string connectionString)
            : this()
        {
            ParseConnectionString(connectionString);
        }

        #endregion

        #region Methods

        public ITinyConnection GetConnection()
        {
            var newConnection = new TinySqlConnection();
            newConnection.Initialize(new SqlConnection(GenerateConnectionString()));

            return newConnection;
        }

        public bool HasEnoughInformation()
        {
            if (string.IsNullOrEmpty(Domain))
                return false;
            //if (string.IsNullOrEmpty(Database)) //TODO: DO WE NEED THIS?
            //    return false;
            if (!UseWindowsAuthentication)
            {
                if (string.IsNullOrEmpty(Username))
                    return false;
                if (string.IsNullOrEmpty(Password))
                    return false;
            }

            return true;
        }

        public DbResultInfo TestConnection()
        {
            var errorMsg = string.Empty;
            Exception innerErrorMsg = null;

            try
            {
                using (var connection = GetConnection())
                    connection.Open();
            }
            catch (Exception e)
            {
                errorMsg = e.Message;

                if (e.InnerException != null)
                    innerErrorMsg = e;
            }

            return new DbResultInfo(errorMsg, innerErrorMsg);
        }

        private string GenerateConnectionString()
        {
            //Build up the connection string details.
            var connDetails = new StringBuilder();

            connDetails.AppendFormat("Data Source={0};", Domain);

            //If no DB is selected, select MASTER/default
            if (string.IsNullOrEmpty(Database))
                Database = DefaultDb;

            connDetails.AppendFormat("Initial Catalog={0};", Database);

            if (UseWindowsAuthentication)
                connDetails.Append("Integrated Security=SSPI;");
            else
            {
                connDetails.AppendFormat("User ID={0};", Username);
                connDetails.AppendFormat("Password={0};", Password);
            }

            connDetails.AppendFormat("Connect Timeout={0};", ConnectionTimemout.TotalSeconds);

            return connDetails.ToString();
        }

        private void ParseConnectionString(string connectionString)
        {
            connectionString = connectionString.Replace(ProviderIgnore, "");

            var tmpConnection = new SqlConnectionStringBuilder(connectionString);

            Domain = tmpConnection.DataSource;
            Database = tmpConnection.InitialCatalog;
            Username = tmpConnection.UserID;
            Password = tmpConnection.Password;
            ConnectionTimemout = new TimeSpan(0, 0, tmpConnection.ConnectTimeout);
            UseWindowsAuthentication = tmpConnection.IntegratedSecurity;
        }

        #endregion
    }
}