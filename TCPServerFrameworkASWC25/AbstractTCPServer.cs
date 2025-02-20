using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TCPServerFrameworkASWC25
{
    /// <summary>
    /// An abstract TCP server including setup logging and configuration and a stop server, for soft shutdown
    /// </summary>
    public abstract class AbstractTCPServer
    {
        private readonly Configuration cfg = Configuration.Instance;
        private readonly MyLogger logger = null;

        private bool running = true;

        /// <summary>
        /// Constructor to setup the tcp server without a configuration file
        /// </summary>
        /// <param name="port">The port where the server should start this implicit set the stop server port</param>
        /// <param name="name">The name of the server default is empty string</param>
        public AbstractTCPServer(int port, string name = "")
        {
            cfg.ServerPort = port;
            cfg.StopPort = port + 1;
            cfg.ServerName = name;
            cfg.DebugLevel = "Information";
            logger = MyLogger.GetInstance();
        }

        /// <summary>
        /// Constructor to setup the tcp server from a configuration file
        /// </summary>
        /// <param name="configfile">the filename of the configuration file</param>
        public AbstractTCPServer(string configfile)
        {
            try
            {
                ReadConfiguration rc = new ReadConfiguration();
                cfg = rc.ReadConfig(configfile);
            }
            catch(ArgumentException ae)
            {
                logger.LogError(ae.Message);
            }
        }


        /// <summary>
        /// Starts the TCP server and the stop server as well
        /// </summary>
        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, cfg.ServerPort);
            listener.Start();
            logger.LogInfo("Server started at port " + cfg.ServerPort);

            Task.Run(StopServer);


            List<Task> tasks = new List<Task>();
            while (running)
            {
                if (listener.Pending())
                {
                    TcpClient client = listener.AcceptTcpClient();
                    logger.LogInfo("Client incoming");
                    logger.LogInfo($"remote (ip,port) = ({client.Client.RemoteEndPoint})");

                    tasks.Add(
                        Task.Run(() =>
                        {
                            TcpClient tmpClient = client;
                            DoOneClient(client);
                        })
                    );
                }
                else
                {
                    Thread.Sleep(100);
                }

                Task.WaitAll(tasks.ToArray());
            }
        }

        private void DoOneClient(TcpClient sock)
        {
            using (StreamReader sr = new StreamReader(sock.GetStream()))
            using (StreamWriter sw = new StreamWriter(sock.GetStream()))
            {
                sw.AutoFlush = true;

                HandleClient(sr, sw);
            }

            sock?.Close();
        }

        /// <summary>
        /// A template method to be overidden in the concrete server
        /// </summary>
        /// <param name="sr">The stream reader associated to the socket</param>
        /// <param name="sw">The stream writer associated to the socket</param>
        protected abstract void HandleClient(StreamReader sr, StreamWriter sw);


        /// <summary>
        /// The default stop server will stop at the text line 'stop'
        /// Can be overidden
        /// </summary>
        private protected virtual void StopServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, cfg.StopPort);
            listener.Start();
            logger.LogInfo($"Stop Server started at port {cfg.StopPort}");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                StreamReader sr = new StreamReader(client.GetStream());
                string? line = sr.ReadLine();
                if (line != null && line == "stop")
                {
                    running = false;
                    return;
                }
            }
        }
        
    }
}
