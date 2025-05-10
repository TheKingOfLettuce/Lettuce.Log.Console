using LettuceLogger.Core;

namespace LettuceLogger.Console;

/// <summary>
/// Takes log messages and writes them to <see cref="System.Console"/>
/// Colors the logs based on the <see cref="LogEventLevel"/>
/// </summary>
public class ConsoleDestination : ILogDestination
{
    /// <summary>
    /// Logs the message to <see cref="System.Console"/>
    /// </summary>
    /// <param name="message">the message to log</param>
    /// <param name="level">the logging level to color with</param>
    public void LogMessage(string message, LogEventLevel level)
    {
        ConsoleColor oldColor = System.Console.ForegroundColor;

        ConsoleColor logColor = oldColor;
        switch (level) {
            case LogEventLevel.VERBOSE:
                logColor = ConsoleColor.DarkGray;
                break;
            case LogEventLevel.DEBUG:
                logColor = ConsoleColor.Gray;
                break;
            case LogEventLevel.INFORMATION:
                logColor = ConsoleColor.White;
                break;
            case LogEventLevel.WARNING:
                logColor = ConsoleColor.Yellow;
                break;
            case LogEventLevel.ERROR:
                logColor = ConsoleColor.Red;
                break;
            case LogEventLevel.FATAL:
                logColor = ConsoleColor.DarkRed;
                break;
        }

        System.Console.ForegroundColor = logColor;
        System.Console.WriteLine(message);
        System.Console.ForegroundColor = oldColor;
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