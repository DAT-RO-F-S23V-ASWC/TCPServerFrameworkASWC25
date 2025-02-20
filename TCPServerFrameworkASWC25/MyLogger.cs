using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace TCPServerFrameworkASWC25
{
    /// <summary>
    /// A singleton class for logging
    /// </summary>
    public class MyLogger
    {

        private static MyLogger instance = null;
        private static object _lock = new object();
        
        /// <summary>
        /// Gets an instance of the MyLogger
        /// </summary>
        /// <returns>The instance</returns>
        public static MyLogger GetInstance() 
        {
            lock (_lock)
            {
                if(instance == null)
                {
                    instance = new MyLogger();
                }
            }
            return instance;
        }

        private MyLogger() {
            tc = new TraceSource(cfg.ServerName);
            tc.Switch = new SourceSwitch(cfg.DebugLevel);

#if DEBUG
            tc.Listeners.Add(new ConsoleTraceListener());
#endif

        }

        private readonly Configuration cfg = Configuration.Instance;
        private readonly TraceSource? tc = null;


        /*
         * Maintaine Listeners
         */
        /// <summary>
        /// Add a tracelistener to the MyLogger
        /// </summary>
        /// <param name="listener">The Listener to be added</param>
        public void AddListener(TraceListener listener)
        {
            if (listener != null)
            {
                tc.Listeners.Add(listener);
            }
        }

        /// <summary>
        /// Removes a tracelistener from the MyLogger
        /// </summary>
        /// <param name="listener">The Listener to be removed</param>
        public void RemoveListener(TraceListener listener)
        {
            if (listener != null)
            {
                tc.Listeners.Remove(listener);
            }
        }


        /*
         * Loggging diff messages
         */
        /// <summary>
        /// Log the message of information level
        /// </summary>
        /// <param name="message">The messages to be logged</param>
        public void LogInfo(string message)
        {
            tc.TraceEvent(TraceEventType.Information, cfg.ServerPort, message);
        }
        /// <summary>
        /// Log the message of warning level
        /// </summary>
        /// <param name="message">The messages to be logged</param>
        public void LogWarning(string message)
        {
            tc.TraceEvent(TraceEventType.Warning, cfg.ServerPort, message);
        }
        /// <summary>
        /// Log the message of error level
        /// </summary>
        /// <param name="message">The messages to be logged</param>
        public void LogError(string message)
        {
            tc.TraceEvent(TraceEventType.Error, cfg.ServerPort, message);
        }
        /// <summary>
        /// Log the message of critical level
        /// </summary>
        /// <param name="message">The messages to be logged</param>
        public void LogCritical(string message)
        {
            tc.TraceEvent(TraceEventType.Critical, cfg.ServerPort, message);
        }

    }
}
