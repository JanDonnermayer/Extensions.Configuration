# Extensions.Configuration

[![](https://github.com/JanDonnermayer/Extensions.Configuration/workflows/UnitTests/badge.svg)](
https://github.com/JanDonnermayer/Extensions.Configuration/actions)

## Description

Within system configuration, sometimes references to environment variables or other entries are used.

```json
{
    "AppName" : "MyApp",
    "UserSettings" : "{$env:HOMEPATH}/.{$env:AppName}/settings.json"
}
```

Popular formats for placholders include:

```
{$env:KEY}, ${KEY}, $(KEY), %KEY%
```

This package provides an extension method for **Microsoft.Extensions.Configuration.IConfiguration**,
which can resolve such references.

## Installation

```powershell
dotnet add package Extensions.Configuration
```

## Usage

### In Program.cs

```csharp
using Microsoft.Extensions.Configuration

hostbuilder.ConfigureAppConfiguration(config =>
{
    config  // files < environment < cmd-line
        .AddJsonFile(...)
        .AddEnvironmentVariables()
        .AddCommandLine(args);
})
```

### In Startup.cs

```csharp
var userSettingsResolved = configuration.ResolveValue("UserSettings");
// -> C:\Users\UserXY\.MyApp\preferences.json
```
