# Extensions.Configuration

Provides extension methods for Microsoft.Extensions.Configuration.IConfiguration

## Dotnet CLI

```powershell
dotnet add package Extensions.Configuration
```

## Motivation

Within system configuration, sometimes references to other configuration entries or environment variables are used.

```json
{
    "AppName" : "MyApp",
    "UserSettings" : "{$env:HOMEPATH}/.{$env:AppName}/preferences.json"
}
```

This package provides an extension method for **Microsoft.Extensions.Configuration.IConfiguration**,
which can resolve such references.

```csharp
using Microsoft.Extensions.Configuration

var userSettingsResolved = configuration.ResolveValue("UserSettings");
// -> C:\Users\UserXY\.MyApp\preferences.json
```
