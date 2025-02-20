using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServerFrameworkASWC25
{
    /// <summary>
    /// The configuration properties for the TCP server
    /// </summary>
    public class Configuration
    {

        private static Configuration instance = new Configuration();
        
        /// <summary>
        /// Get an instance of the configuration
        /// </summary>
        public static Configuration Instance => instance;
        private Configuration() { }


        /// <summary>
        /// The server port which the server bind to
        /// </summary>
        public int ServerPort { get; set; }
        /// <summary>
        /// The stop server port which the stop server bind to
        /// </summary>
        public int StopPort { get; set; }
        /// <summary>
        /// The name of the server
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// The general debug level of the server (information, warning, error or critical)
        /// </summary>
        public string DebugLevel { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Instance)}={Instance}, {nameof(ServerPort)}={ServerPort.ToString()}, {nameof(StopPort)}={StopPort.ToString()}, {nameof(ServerName)}={ServerName}, {nameof(DebugLevel)}={DebugLevel}}}";
        }
    }
}
