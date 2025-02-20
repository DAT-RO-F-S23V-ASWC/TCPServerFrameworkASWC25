using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPServerFrameworkASWC25;

namespace TestAfTCPServer
{
    public class EasyEchoServer : AbstractTCPServer
    {
        public EasyEchoServer(int port, string name = "") : base(port, name)
        {
        }

        public EasyEchoServer(string configFile) : base(configFile)
        {
        }

        protected override void HandleClient(StreamReader sr, StreamWriter sw)
        {
            sw.WriteLine(sr.ReadLine());
        }
    }
}
