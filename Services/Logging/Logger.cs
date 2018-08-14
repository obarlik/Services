using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Logging
{
    public class Logger
    {
        public Logger()
        {
        }


        public void Log(
            string actionName,
            Action action)
        {
            try
            {
                Log(LogType.Trace, 
                    "Action", 
                    string.Format(
                        "Entering action '{0}'...", 
                        actionName));

                action();

                Log(LogType.Trace, 
                    "Action", 
                    string.Format(
                        "Returned from action '{0}'.", 
                        actionName));
            }
            catch (Exception ex)
            {
                LogError(
                    "Action",
                    string.Format(
                        "Error while running action '{0}'!", 
                        actionName), 
                    ex);

                throw;
            }
        }


        public T Log<T>(
            string functionName,
            Func<T> function)
        {
            try
            {
                Log(LogType.Trace, 
                    "Function", 
                    string.Format(
                        "Entering function '{0}'...", 
                        functionName));

                var result = function();

                Log(LogType.Trace,
                    "Function",
                    string.Format(
                        "Returned from function '{0}'.",
                        functionName));

                return result;
            }
            catch (Exception ex)
            {
                LogError(
                    "Function",
                    string.Format(
                        "Error while running function '{0}'!", 
                        functionName),
                    ex);

                throw;
            }
        }


        public void LogError(string category, string msg, Exception ex)
        {
            Log(LogType.Error,
                category,
                string.Format(
                    "{0}  Error Message: {1}",
                    msg,
                    ex.Message));
        }


        public LogType LogLevel = LogType.Error;


        public async void Log(LogType logType, string category, string message)
        {
            if (logType > LogLevel)
                return;

            await Task.Run(() =>
                LogOutput(
                    new LogData(
                        DateTime.UtcNow,
                        logType,
                        category,
                        message)));            
        }

        
        protected virtual void LogOutput(LogData logData)
        {
            Debug.WriteLine(
                string.Format(
                    "{0:dd.MM.yyyy HH:mm:ss.fff}  {1:8}  {2}  {3}",
                    logData.Time,
                    logData.LogType,
                    logData.Category,
                    logData.Message));
        }
    }
}
