using Lettuce.Log.Core;
using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lettuce.Log.Console {
    internal struct ConsoleLogEntry {
        public string Message {get; set;}
        public LogEventLevel LogLevel {get; set;}
    }


    /// <summary>
    /// Takes log messages and writes them to <see cref="System.Console"/>
    /// Colors the logs based on the <see cref="LogEventLevel"/>
    /// </summary>
    public sealed class ConsoleDestination : ILogDestination, IDisposable {
        private readonly Channel<ConsoleLogEntry> _logChannel;
        private readonly Task _consumeLoop;
        private bool _isDisposed;

        /// <summary>
        /// Constructs our console destination
        /// </summary>
        public ConsoleDestination() {
            _logChannel = Channel.CreateUnbounded<ConsoleLogEntry>();
            _consumeLoop = Task.Run(ConsumerLoop);
        }

        /// <summary>
        /// Logs the message to <see cref="System.Console"/>
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="level">the logging level to color with</param>
        public void LogMessage(string message, LogEventLevel level) {
            _ = _logChannel.Writer.TryWrite(new ConsoleLogEntry{LogLevel=level, Message=message});
        }

        private async Task ConsumerLoop() {
            await foreach(ConsoleLogEntry logEntry in _logChannel.Reader.ReadAllAsync()) {
                ConsoleColor oldColor = System.Console.ForegroundColor;
                System.Console.ForegroundColor = GetConsoleColor(logEntry.LogLevel);
                System.Console.WriteLine(logEntry.Message);
                System.Console.ForegroundColor = oldColor;
            }
        }

        private static ConsoleColor GetConsoleColor(LogEventLevel level) {
            switch (level) {
                case LogEventLevel.VERBOSE:
                    return ConsoleColor.DarkGray;
                case LogEventLevel.DEBUG:
                    return ConsoleColor.Gray;
                case LogEventLevel.INFORMATION:
                    return ConsoleColor.White;
                case LogEventLevel.WARNING:
                    return ConsoleColor.Yellow;
                case LogEventLevel.ERROR:
                    return ConsoleColor.Red;
                case LogEventLevel.FATAL:
                    return ConsoleColor.DarkRed;
                default:
                    return ConsoleColor.White;
            }
        }

        /// <summary>
        /// Disposes of our console destination ensuring all logs are written to the console
        /// </summary>
        public void Dispose() {
            if (_isDisposed) {
                return;
            }

            _isDisposed = true;
            _logChannel.Writer.Complete();
            _consumeLoop.Wait();
        }
    }

    /// <summary>
    /// Extension class to fluently add a <see cref="ConsoleDestination"/>
    /// </summary>
    public static class ConsoleDestinationExtension {

        /// <summary>
        /// Extension method to take <see cref="Logger"/> and add a <see cref="ConsoleDestination"/>
        /// </summary>
        /// <param name="logger">the logger to add to</param>
        /// <returns>the <see cref="Logger"/> with a <see cref="ConsoleDestination"/></returns>
        public static Logger AddConsoleDestination(this Logger logger) {
            logger.AddDestination(new ConsoleDestination());
            return logger;
        }
    }
}