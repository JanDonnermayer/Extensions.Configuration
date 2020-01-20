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

This package provides an extension method on **Microsoft.Extensions.Configuration.IConfiguration**, that provides the desired behaviour.

```csharp
using Microsoft.Extensions.DependencyInjection

// Returns the configuration entry for,
// with 'HOMEPATH' and 'AppName' resolved.
var userSettingsResolved = configuration.ResolveValue("UserSettings");
```
