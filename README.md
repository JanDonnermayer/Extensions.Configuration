# Extensions.Configuration

[![](https://github.com/JanDonnermayer/Extensions.Configuration/workflows/UnitTests/badge.svg)](https://github.com/JanDonnermayer/Extensions.Configuration/actions)

[![](https://img.shields.io/badge/nuget-v0.0.3-blue.svg)](https://www.nuget.org/packages/Extensions.Configuration/)

## Description

This is a umbrella package for two seperate packages

- Extensions.Configuration.Sources.Object
- Extensions.Configuration.Resolver

## Installation

```powershell
dotnet add package Extensions.Configuration
```

# Extensions.Configuration.Sources.Objects

[![](https://img.shields.io/badge/nuget-v0.0.1-blue.svg)](https://www.nuget.org/packages/Extensions.Configuration.Sources.Object/)

## Description

This package adds functionality to **Microsoft.Extensions.Configuration**,
to use objects as configuration source.

## Installation

```powershell
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Extensions.Configuration.Sources.Objects
```

## Usage

```csharp
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder
    .AddObject(new { K1 = "V1" })
    .Build();

var val = configuration["K1"];
// Returns "V1"

```

Nesting is supported

```csharp
var configuration = new ConfigurationBuilder
    .AddObject(new { K1 = new { K2 = "V1" } } })
    .Build();

var val = configuration["K1:K2"];
// Returns "V1"
```

# Extensions.Configuration.Resolver

[![](https://img.shields.io/badge/nuget-v0.0.3-blue.svg)](https://www.nuget.org/packages/Extensions.Configuration.Resolver/)

## Description

This package adds functionality to **Microsoft.Extensions.Configuration**,
to resolve substitutions within configuration entries.

## Installation

```powershell
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Extensions.Configuration.Resolver
```

## Usage

Examplary json configuration:

```json
{
  "AppName": "MyApp",
  "UserSettings": "${HOMEPATH}/.{$env:AppName}/settings.json"
}
```

```csharp
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder
    .AddJsonFile(...)
    .AddEnvironmentVariables()
    .Build();

var userSettingsResolved = configuration.ResolveValue("UserSettings");
// Returns e.g.: "C:\Users\MyUser\.MyApp\settings.json"
```

## Remarks

Supported placeholders:

```
{$env:KEY}, ${KEY}, $(KEY), %KEY%
```
