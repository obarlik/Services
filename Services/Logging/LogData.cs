using System;
using System.Collections.Generic;
using System.Text;

namespace Logging
{
    public class LogData
    {
        public LogData()
        {
        }


        public LogData(DateTime time, LogType logType, string category, string message)
        {
            Time = time;
            LogType = logType;
            Category = category;
            Message = message;
        }


        public DateTime Time { get; set; }

        public LogType LogType { get; set; }

        public string Category { get; set; }

        public string Message { get; set; }
    }
}
