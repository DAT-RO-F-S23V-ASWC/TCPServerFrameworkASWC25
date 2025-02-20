using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TCPServerFrameworkASWC25
{
    /// <summary>
    /// Reads the configuration to the TCP server from a configuration-file
    /// </summary>
    public class ReadConfiguration
    {
        /// <summary>
        /// Reads the configuration
        /// </summary>
        /// <param name="configFilePath">The filename of the configuration file</param>
        /// <returns>A Configuration object holding configurations information such as port, stopport, name etc</returns>
        /// <exception cref="ArgumentException">If the file do not exist or the configuration file are malstructured</exception>
        public Configuration ReadConfig(string configFilePath)
        {
            Configuration cnf = Configuration.Instance;

            try
            {
                XmlDocument configXML = new XmlDocument();
                configXML.Load(configFilePath);

                /*
                 * Read server port
                 */
                XmlNode? PortXML = configXML.DocumentElement.SelectSingleNode("ServerPort");
                if (PortXML != null)
                {
                    cnf.ServerPort = Convert.ToInt32(PortXML.InnerXml.Trim());
                }

                /*
                 * Read stop port (default server port + 1
                 */
                XmlNode? StopPortXml = configXML.DocumentElement.SelectSingleNode("StopServerPort");
                if (StopPortXml != null)
                {
                    cnf.StopPort = Convert.ToInt32(StopPortXml.InnerXml.Trim());
                }
                else
                {
                    cnf.StopPort = cnf.ServerPort + 1;
                }

                /*
                 * Read server name
                 */
                XmlNode? NameXml = configXML.DocumentElement.SelectSingleNode("ServerName");
                if (NameXml != null)
                {
                    cnf.ServerName = NameXml.InnerXml.Trim();
                }

                /*
                 * Read server debug level
                 */
                XmlNode? DebugXml = configXML.DocumentElement.SelectSingleNode("ServerDebug");
                if (DebugXml != null)
                {
                    cnf.DebugLevel = DebugXml.InnerXml.Trim();
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("not possible to read configuration");
            }

            return cnf;
        }
    }
}
