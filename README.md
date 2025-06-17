# Lettuce.Log.Console

A logging destination that logs to the `System.Console`, using colors based on the logged event level.

[![Build](https://github.com/TheKingOfLettuce/Lettuce.Log.Console/actions/workflows/Build.yml/badge.svg)](https://github.com/TheKingOfLettuce/Lettuce.Log.Console/actions/workflows/Build.yml)

## Quick Start

`Lettuce.Log.Console` is available on [nuget](https://www.nuget.org/packages/Lettuce.Log.Console/)

```csharp
Log.Logger.AddConsoleDestination();

Log.Info("This logs an information message");
Log.Error("This logs an error, with color!");
```

See [Lettuce.Log.Core](https://github.com/TheKingOfLettuce/Lettuce.Log.Core) for how to use the logger.

### Console Destination
As stated in the descriptions, this `ILogDestination` simply logs to `System.Console` and changes the color based on the `LogEventLevel`

| LogEventLevel | Color |
| :------------ | -----: |
| Verbose       | DarkGray |
| Debug         | Gray |
| Information   | White |
| Warning       | Yellow |
| Error         | Red |
| Fatal         | DarkRed |

Each log tries to set the previous color back to what it was before, however its important to note that changing the color is not thread safe, so beware of off-coloring if multiple threads are changing `System.Console.ForegroundColor`