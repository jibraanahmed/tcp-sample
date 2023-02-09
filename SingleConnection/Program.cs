using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPServer;

namespace SingleConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            Storage.init();
            Server server = new Server();
            server.start();
        }
    }
}
