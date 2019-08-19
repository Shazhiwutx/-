using System;

namespace SZWNet
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class Log
    {
        /// <summary>
        /// 这是用于打印日志信息的方法
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <param name="logType">日志消息等级</param>
        public static void WriteLine(string log, LogType logType = LogType.Normal)
        {
            switch (logType)
            {
                case LogType.Normal:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            Console.OutputEncoding = System.Text.Encoding.Default;
            Console.WriteLine("{0}>>{1}", DateTime.Now, log);
        }

        /// <summary>
        /// 日志消息等级枚举
        /// </summary>
        public enum LogType
        {
            Normal,
            Warning,
            Error
        }
    }
}
