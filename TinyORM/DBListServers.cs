using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TinyORM
{
    public class DbListServers
    {
        /*[DllImport("odbc32.dll")]
        private static extern short SQLAllocHandle(short hType, IntPtr inputHandle, out IntPtr outputHandle);
        [DllImport("odbc32.dll")]
        private static extern short SQLSetEnvAttr(IntPtr henv, int attribute, IntPtr valuePtr, int strLength);
        [DllImport("odbc32.dll")]
        private static extern short SQLFreeHandle(short hType, IntPtr handle); 
        [DllImport("odbc32.dll",CharSet=CharSet.Ansi)]
        private static extern short SQLBrowseConnect(IntPtr hconn, StringBuilder inString, 
            short inStringLength, StringBuilder outString, short outStringLength,
            out short outLengthNeeded);

        private const short SQL_HANDLE_ENV = 1;
        private const short SQL_HANDLE_DBC = 2;
        private const int SQL_ATTR_ODBC_VERSION = 200;
        private const int SQL_OV_ODBC3 = 3;
        private const short SQL_SUCCESS = 0;
		
        private const short SQL_NEED_DATA = 99;
        private const short DEFAULT_RESULT_SIZE = 1024;
        private const string SQL_DRIVER_STR = "DRIVER=SQL SERVER";
	
        public DBListServers(){}

        public DBResultInfoRtn<string[]> GetServers()
        {
            string[] retval = null;
            string endParse = string.Empty;
            string errorMsg = string.Empty;
            IntPtr henv = IntPtr.Zero;
            IntPtr hconn = IntPtr.Zero;
            StringBuilder inString = new StringBuilder(SQL_DRIVER_STR);
            StringBuilder outString = new StringBuilder(DEFAULT_RESULT_SIZE);
            short inStringLength = (short) inString.Length;
            short lenNeeded = 0;

            try
            {
                if (SQL_SUCCESS == SQLAllocHandle(SQL_HANDLE_ENV, henv, out henv))
                {
                    if (SQL_SUCCESS == SQLSetEnvAttr(henv,SQL_ATTR_ODBC_VERSION,(IntPtr)SQL_OV_ODBC3,0))
                    {
                        if (SQL_SUCCESS == SQLAllocHandle(SQL_HANDLE_DBC, henv, out hconn))
                        {
                            if (SQL_NEED_DATA ==  SQLBrowseConnect(hconn, inString, inStringLength, outString, 
                                DEFAULT_RESULT_SIZE, out lenNeeded))
                            {
                                if (DEFAULT_RESULT_SIZE < lenNeeded)
                                {
                                    outString.Capacity = lenNeeded;
                                    if (SQL_NEED_DATA != SQLBrowseConnect(hconn, inString, inStringLength, outString, 
                                        lenNeeded,out lenNeeded))
                                    {
                                        throw new ApplicationException("Unabled to aquire SQL Servers from ODBC driver.");
                                    }	
                                }
                                endParse = outString.ToString();
                                int start = endParse.IndexOf("{") + 1;
                                int len = endParse.IndexOf("}") - start;
                                if ((start > 0) && (len > 0))
                                {
                                    endParse = endParse.Substring(start,len);
                                }
                                else
                                {
                                    endParse = string.Empty;
                                }
                            }						
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                endParse = string.Empty;
                errorMsg = ex.Message;
            }
            finally
            {
                if (hconn != IntPtr.Zero)
                {
                    SQLFreeHandle(SQL_HANDLE_DBC,hconn);
                }
                if (henv != IntPtr.Zero)
                {
                    SQLFreeHandle(SQL_HANDLE_ENV,hconn);
                }
            }
	
            if (endParse.Length > 0)
            {
                retval = endParse.Split(",".ToCharArray());
            }

            return new DBResultInfoRtn<string[]>(errorMsg, retval);
        }*/


        public DbResultInfoRtn<SqlServerInfo[]> GetServers()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);

            //  For .Net v 2.0 it's a bit simpler
            //  socket.EnableBroadcast = true;	// for .Net v2.0
            //  socket.ReceiveTimeout = 3000;	// for .Net v2.0

            var servers = new ArrayList();
            try
            {
                var msg = new byte[] {0x02};
                var ep = new IPEndPoint(IPAddress.Broadcast, 1434);
                socket.SendTo(msg, ep);

                int cnt;
                var bytBuffer = new byte[1024];
                do
                {
                    cnt = socket.Receive(bytBuffer);
                    servers.Add(new SqlServerInfo(null, bytBuffer));
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 300);
                } while (cnt != 0);
            }
            catch (SocketException socex)
            {
                const int wsaeTimedOut = 10060; // Connection timed out. 
                const int wsaeHostUnreach = 10065; // No route to host. 

                // Re-throw if it's not a timeout.
                if (socex.ErrorCode == wsaeTimedOut || socex.ErrorCode == wsaeHostUnreach)
                {
                }
                else
                    throw;
            }
            finally
            {
                socket.Close();
            }

            // Copy from the untyped but expandable ArrayList, to a
            // type-safe but fixed array of SqlServerInfos.
            var aServers = new SqlServerInfo[servers.Count];
            servers.CopyTo(aServers);

            return new DbResultInfoRtn<SqlServerInfo[]>(null, aServers);
        }

        public DbResultInfoRtn<List<string>> GetServersString()
        {
            var tmpSvrs = GetServers();

            if (tmpSvrs.Success)
            {
                var tmpRtn = new List<string>();

                for (int i = 0; i < tmpSvrs.Value.Length; i++)
                {
                    string nameOfServer = tmpSvrs.Value[i].ServerName;

                    if (HasInstanceName(tmpSvrs.Value[i].InstanceName))
                        nameOfServer += "\\" + tmpSvrs.Value[i].InstanceName;

                    tmpRtn.Add(nameOfServer);
                }

                tmpRtn.Sort();
                return new DbResultInfoRtn<List<string>>(tmpSvrs.ErrorMessage, tmpRtn);
            }

            return new DbResultInfoRtn<List<string>>(tmpSvrs.ErrorMessage, null);
        }

        private static bool HasInstanceName(string instanceName)
        {
            return !string.IsNullOrEmpty(instanceName) && instanceName.ToUpper() != "MSSQLSERVER".ToUpper();
        }

        #region Nested type: SqlServerInfo

        public class SqlServerInfo
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="SqlServerInfo"/> class.
            /// </summary>
            private SqlServerInfo()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SqlServerInfo"/> class.
            /// </summary>
            /// <param name="ip">The ip.</param>
            /// <param name="info">The info.</param>
            public SqlServerInfo(IPAddress ip, byte[] info)
                : this(ip, Encoding.ASCII.GetString(info, 3, BitConverter.ToInt16(info, 1)))
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SqlServerInfo"/> class.
            /// </summary>
            /// <param name="ip">The ip address.</param>
            /// <param name="info">The info.</param>
            public SqlServerInfo(IPAddress ip, string info)
            {
                Ip = ip;
                string[] nvs = info.Split(';');
                for (int i = 0; i < nvs.Length; i += 2)
                {
                    switch (nvs[i].ToLower())
                    {
                        case "servername":
                            ServerName = nvs[i + 1];
                            break;

                        case "instancename":
                            InstanceName = nvs[i + 1];
                            break;

                        case "isclustered":
                            IsClustered = (nvs[i + 1].ToLower() == "yes"); //bool.Parse(nvs[i+1]);
                            break;

                        case "version":
                            Version = nvs[i + 1];
                            break;

                        case "tcp":
                            TcpPort = int.Parse(nvs[i + 1]);
                            break;

                        case "np":
                            Np = nvs[i + 1];
                            break;

                        case "rpc":
                            Rpc = nvs[i + 1];
                            break;
                    }
                }
            }

            #endregion

            #region Properties

            public string ServerName { get; private set; }
            public string InstanceName { get; private set; }
            public bool IsClustered { get; private set; }
            public string Version { get; private set; }
            public int TcpPort { get; private set; }
            public string Np { get; private set; }
            public string Rpc { get; private set; }
            public IPAddress Ip { get; private set; }

            #endregion
        }

        #endregion
    }
}