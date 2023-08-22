using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game
{
    public enum LogLevel : byte
    {
        Debug = 0,
        Advice = 1,
        Warning = 2,
        Error = 3,
        Fatal = 4
    }
    public static class Logger
    {
        private static readonly ConcurrentQueue<BaseLogData> _messages = new();
        private static readonly TimeSpan TotalTimeFinishWait = TimeSpan.FromSeconds(40);
        private static bool _creatingLogInProcess = false;
        private static bool _finish = true;
        private static string _logPath = string.Empty;
        private const int LOG_BUFFER_CAPACITY = 1024;
        public static EventHandler<Exception> InternException;

#if DEBUG
        private static LogLevel _minLogLevel = LogLevel.Debug;
#elif RELEASE
        private static LogLevel _minLogLevel = LogLevel.Advice;
#endif

        public static void Start()
        {
            if (!_finish) return;
            _messages.Clear();

            //Check Directory
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");

            //Check File
            DateTime now = DateTime.Now;
            string fileName = "Log " + now.ToString("MM_dd_yy") + ".log";
            _logPath = Path.Combine("Logs", fileName);
            if (!File.Exists(_logPath))
                File.Create(_logPath);

            //Start Log Task
            _finish = false;
            Task.Factory.StartNew(LoggerTask);

            //First log
            Advice("===================LOG START===================");
        }
        public static void Finish()
        {
            Advice("===================LOG FINISH==================");
            DateTime startWait = DateTime.Now;

            while (!((_messages.Count == 0 && !_creatingLogInProcess) || DateTime.Now - startWait > TotalTimeFinishWait || _finish))
                Task.Delay(25).Wait();

            _finish = true;
        }


        public static void Debug(string message) => Log(message, LogLevel.Debug);
        public static void Debug(string[] messages) => Log(messages, LogLevel.Debug);

        public static void Advice(string message) => Log(message, LogLevel.Advice);
        public static void Advice(string[] messages) => Log(messages, LogLevel.Advice);

        public static void Warning(string message) => Log(message, LogLevel.Warning);
        public static void Warning(string[] messages) => Log(messages, LogLevel.Warning);
        public static void Warning(Exception ex) => Log(ex, LogLevel.Warning);

        public static void Error(string message) => Log(message, LogLevel.Error);
        public static void Error(string[] messages) => Log(messages, LogLevel.Error);
        public static void Error(Exception ex) => Log(ex, LogLevel.Error);

        public static void Fatal(Exception ex) => Log(ex, LogLevel.Fatal);


        public static void Log(Exception ex, LogLevel level = LogLevel.Error)
        {
            if (level < _minLogLevel) return;

            ExceptionLogData log = new(level, ex);
            _messages.Enqueue(log);
        }
        public static void Log(string message, LogLevel level = LogLevel.Advice)
        {
            if (level < _minLogLevel) return;

            MessageLogData log = new(level, message);
            _messages.Enqueue(log);
        }
        public static void Log(string[] messages, LogLevel level = LogLevel.Advice)
        {
            if (level < _minLogLevel) return;
            if (messages.Length == 0) return;

            string message = messages[0];
            for (int i = 1; i < messages.Length; i++)
                message += Environment.NewLine + messages[i];

            MessageLogData log = new(level, message);
            _messages.Enqueue(log);
        }


        private static void LoggerTask()
        {
            DateTime lastError = new();
            int errorCount = 0;

            while (!_finish)
            {
                if (_messages.Count > LOG_BUFFER_CAPACITY)
                {
                    //Too many exceptions
                    StackOverflowException ex = new("Too many logs to write!!!");

                    try
                    {
                        ExceptionLogData log = new(LogLevel.Fatal, ex);
                        DoLog(log);
                    }
                    finally
                    {
                        _finish = true;
                        InternException?.Invoke(null, ex);
                        throw ex;
                    }
                }

                try
                {
                    if (_messages.Count > 0)
                    {
                        if (_messages.TryDequeue(out BaseLogData logData))
                            DoLog(logData);
                    }
                }
                catch
                {
                    DateTime now = DateTime.Now;
                    if (now - lastError < TimeSpan.FromSeconds(10))
                        errorCount = 0;

                    lastError = now;
                    errorCount++;
                    if (errorCount >= 3)
                    {
                        //Recursive multi exceptions
                        StackOverflowException ex = new("Too many error trying logging!!!");

                        try
                        {
                            ExceptionLogData log = new(LogLevel.Fatal, ex);
                            DoLog(log);
                        }
                        finally
                        {
                            _finish = true;
                            InternException?.Invoke(null, ex);
                            throw ex;
                        }
                    }
                }
                //Task.Delay(2).Wait();
            }
        }
        private static void DoLog(BaseLogData logData)
        {
            _creatingLogInProcess = true;
            string logMessage = CreateLogString(logData);
            File.AppendAllText(_logPath, logMessage + Environment.NewLine);
            System.Diagnostics.Debug.WriteLine(logMessage);
            _creatingLogInProcess = false;
        }
        private static string CreateLogString(BaseLogData logData)
        {
            StringBuilder logBuilder = new();
            logBuilder.Append(logData.Date.ToString("MM/dd/yy HH:mm:ss:ffff"));
            logBuilder.Append($" ({(byte)logData.LogLevel}) =>");
            logBuilder.Append('\t');

            Type logType = logData.GetType();
            if (logType == typeof(ExceptionLogData))
            {
                //Process exception message
                Exception ex = ((ExceptionLogData)logData).Exception;
                logBuilder.Append("-----------------EXCEPTION-----------------");
                logBuilder.AppendLine();
                logBuilder.Append($"Type: {ex.GetType()}");
                logBuilder.AppendLine();
                logBuilder.Append($"Message: {ex.Message}");
                logBuilder.AppendLine();
                logBuilder.Append($"Trace: {ex.StackTrace}");
                logBuilder.AppendLine();
                logBuilder.Append("-------------------------------------------");
            }
            else if (logType == typeof(MessageLogData))
            {
                //Process regular message
                logBuilder.Append(((MessageLogData)logData).Message);
            }
            else throw new NotImplementedException("Not implemented log type");

            string logString = logBuilder.ToString();

            //Padding
            string[] lines = logString.Split(Environment.NewLine);
            for (int i = 1; i < lines.Length; i++)
                lines[i] = string.Concat(Environment.NewLine, new string(' ', 32), lines[i]);

            logString = string.Concat(lines);
            return logString;
        }


        private abstract class BaseLogData
        {
            public readonly LogLevel LogLevel;
            public readonly DateTime Date;

            protected BaseLogData(LogLevel logLevel)
            {
                LogLevel = logLevel;
                Date = DateTime.Now;
            }
        }
        private class ExceptionLogData : BaseLogData
        {
            public readonly Exception Exception;

            public ExceptionLogData(LogLevel logLevel, Exception exception) : base(logLevel)
            {
                this.Exception = exception;
            }
        }
        private class MessageLogData : BaseLogData
        {
            public readonly string Message;

            public MessageLogData(LogLevel logLevel, string message) : base(logLevel)
            {
                this.Message = message;
            }
        }
    }
}
