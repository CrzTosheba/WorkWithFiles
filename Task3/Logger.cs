using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    public enum LogLevel // уровни логирования
    {
        Debug,
        Info,
        Warning,
        Error,
    }

    internal class Logger
    {
        public static LogLevel MinLevel = LogLevel.Debug;
        public static void Log(LogLevel lvl, string msg)
        {
            if (lvl < MinLevel)
                return;
            Console.WriteLine("{0} | {1} | {2}",
                DateTime.Now,
                lvl,
                msg
            );
        }
    }
}
