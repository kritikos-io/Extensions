# Kritikos.Extensions

An opinionated set of extensions to be used as a starting point for new projects.

Notable projects:

## Kritikos.Extensions.Version

A library that provides a simple way to get the semantic version of the current assembly.
```csharp
// Simple usage
SemanticVersion.FromAsembly(typeof(Startup).Assembly);

// DI Injection
var services = new ServiceCollection();
services.AddSemanticVersionExposing(typeof(Startup).Assembly);
```

Versioning is expected to be exposed in the InformationalVersion attribute, and can be of any of the following formats:

* Simple version: `1.0.0`
*
