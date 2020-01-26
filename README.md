
# Extensions.Configuration
[![](https://github.com/JanDonnermayer/Extensions.Configuration/workflows/UnitTests/badge.svg)](
https://github.com/JanDonnermayer/Extensions.Configuration/actions)

[![](https://img.shields.io/badge/nuget-v0.0.3-blue.svg)](
https://www.nuget.org/packages/Extensions.Configuration/)

# Extensions.Configuration.Resolver

[![](https://img.shields.io/badge/nuget-v0.0.1-blue.svg)](
https://www.nuget.org/packages/Extensions.Configuration.Resolver/)

## Description

Within system configuration, sometimes references to other configuration entries are used.

```json
{
    "AppName" : "MyApp",
    "UserSettings" : "${HOMEPATH}/.{$env:AppName}/settings.json"
}
```

This package adds functionality to **Microsoft.Extensions.Configuration**,
which can resolve such references.

## Installation

```powershell
dotnet add package Extensions.Configuration.Resolver
```

## Usage

### In Program.cs

```csharp
using Microsoft.Extensions.Configuration

hostbuilder.ConfigureAppConfiguration(config =>
{
    config  
        .AddJsonFile(...)
        .AddEnvironmentVariables()
        .AddCommandLine(args);
})
```

### In Startup.cs

```csharp
var userSettingsResolved = configuration.ResolveValue("UserSettings");
// Returns e.g.: C:\Users\MyUser\.MyApp\settings.json
```

## Remarks

Supported placholders:

```
{$env:KEY}, ${KEY}, $(KEY), %KEY%
```
