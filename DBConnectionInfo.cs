using System;
using System.Data.SqlClient;
using System.Text;

namespace TinyORM
{
    public class DbConnectionInfo
    {
        #region Properties

        private const string DefaultDb = "MASTER";
        private const string ProviderIgnore = "Provider=SQLOLEDB.1;";
        public TimeSpan ConnectionTimemout = new TimeSpan(0, 2, 0);

        public DbConnectionInfo()
        {
        }

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

        public DbConnectionInfo(string domain, string database, string username, string password,
                                bool useWindowsAuthentication)
        {
            Domain = domain;
            Database = database;
            Username = username;
            Password = password;
            UseWindowsAuthentication = useWindowsAuthentication;
        }

        public DbConnectionInfo(string connectionString)
        {
            ParseConnectionString(connectionString);
        }

        #endregion

        #region Methods

        public SqlConnection GetConnection()
        {
            return new SqlConnection(GenerateConnectionString());
        }

        public bool HasEnoughInformation()
        {
            if (string.IsNullOrEmpty(Domain))
                return false;
            if (string.IsNullOrEmpty(Database))
                return false;
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
            var innerErrorMsg = string.Empty;

            try
            {
                using (SqlConnection connection = GetConnection())
                    connection.Open();
            }
            catch (Exception e)
            {
                errorMsg = e.Message;

                if (e.InnerException != null)
                    innerErrorMsg = e.InnerException.Message;
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