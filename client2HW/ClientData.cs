using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client2HW
{
    class ClientData
    {
        private string _username;
        private string _ip;
        private int _port;
        private string _message;
        private int _connect;

        public ClientData(string ip, int port, string username, string message, int connect)
        {
            _ip = ip;
            _port = port;
            _username = username;
            _message = message;
            _connect = connect;
        }
        public string clientIp { get => _ip; set => _ip = value; }
        public int clientPort { get => _port; set => _port = value; }
        public string clientName { get => _username; set => _username = value; }
        public string clientMsg { get => _message; set => _message = value; }
        public int clientConn { get => _connect; set => _connect = value; }
    }
}
