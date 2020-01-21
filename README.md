# Extensions.Configuration

[![](https://github.com/JanDonnermayer/Extensions.Configuration/workflows/UnitTests/badge.svg)](
https://github.com/JanDonnermayer/Extensions.Configuration/actions)

[![](https://img.shields.io/badge/nuget-v0.0.1-blue.svg)](
https://www.nuget.org/packages/Extensions.Configuration/)

## Description

Within system configuration, sometimes references to environment variables or other entries are used.
Hereby, even multiple formats can occur.

```json
{
    "AppName" : "MyApp",
    "UserSettings" : "{$env:HOMEPATH}/.$(AppName)/settings.json"
}
```

This package provides an extension method for **Microsoft.Extensions.Configuration.IConfiguration**,
which can resolve such references.

## Installation

```powershell
dotnet add package Extensions.Configuration
```

## Usage

```csharp
using Microsoft.Extensions.Configuration

var userSettingsResolved = configuration.ResolveValue("UserSettings");
// -> C:\Users\UserXY\.MyApp\preferences.json
```
