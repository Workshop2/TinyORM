using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TinyORM.Maintenance
{
    /// <summary>
    /// This class should be able to return a list of sql servers on the local network. Works 90% of the time - maybe work on finding a different way (without using SMO objects)   
    /// Found at http://www.codeproject.com/Articles/12336/Locate-SQL-Server-instances-on-the-local-network
    /// Work by James Curran
    /// </summary>
    public class DbListServers
    {
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
                var msg = new byte[] { 0x02 };
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

                foreach (var t in tmpSvrs.Value)
                {
                    var nameOfServer = t.ServerName;

                    if (HasInstanceName(t.InstanceName))
                        nameOfServer += "\\" + t.InstanceName;

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