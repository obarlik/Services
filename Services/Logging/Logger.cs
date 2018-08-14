using System;
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
                Log(LogType.Trace, "Action", string.Format("Running action '{0}'", actionName));
                action();
                Log(LogType.Trace, "Action", string.Format("Returning from action '{0}'", actionName));
            }
            catch (Exception ex)
            {
                LogError(
                    "Action",
                    string.Format("Error while running action '{0}'.", actionName), 
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
                    string.Format("Running function '{0}'...", 
                    functionName));

                var result = function();

                Log(LogType.Trace,
                    "Function",
                    string.Format(
                        "Returned from function '{0}' with result {1}.",
                        functionName,
                        result == null ?
                            "NULL" :
                            string.Format(
                                "'{0}'", 
                                result.ToString())));

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

            var time = DateTime.Now;

            await Task.Run(() =>
            {

                var logData = new LogData(time, logType, category, message);
                LogOutput(logData);
            });
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
