# NetEvolve.ProjectBuilders.TUnit

[![NuGet Version](https://img.shields.io/nuget/v/NetEvolve.ProjectBuilders.TUnit.svg)](https://www.nuget.org/packages/NetEvolve.ProjectBuilders.TUnit/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/NetEvolve.ProjectBuilders.TUnit.svg)](https://www.nuget.org/packages/NetEvolve.ProjectBuilders.TUnit/)
[![License](https://img.shields.io/github/license/dailydevops/projectbuilders.svg)](https://github.com/dailydevops/projectbuilders/blob/main/LICENSE)

Test utilities for TUnit-based .NET projects, providing helpers for temporary directory management and test environment setup.

## Features

- Temporary directory management for integration tests
- Utilities for test environment setup and teardown
- Seamless integration with TUnit test framework

## Installation

### NuGet Package Manager

```powershell
Install-Package NetEvolve.ProjectBuilders.TUnit
```

### .NET CLI

```bash
dotnet add package NetEvolve.ProjectBuilders.TUnit
```

### PackageReference

```xml
<PackageReference Include="NetEvolve.ProjectBuilders.TUnit" />
```

## Quick Start

```csharp
using NetEvolve.ProjectBuilders.TUnit;

// Use TemporaryDirectory in your TUnit tests
using (var tempDir = new TemporaryDirectory())
{
    // Arrange test files in tempDir.Path
}
```

## Usage

### Basic Example

```csharp
// Create a temporary directory for a test
using (var tempDir = new TemporaryDirectory())
{
    // Use tempDir.Path for test files
}
```

## Requirements

- .NET 8.0 or higher
- [TUnit](https://www.nuget.org/packages/TUnit/)

## Related Packages

- [**NetEvolve.ProjectBuilders**](https://www.nuget.org/packages/NetEvolve.ProjectBuilders/) - Core project builder utilities
- [**NetEvolve.ProjectBuilders.XUnit**](https://www.nuget.org/packages/NetEvolve.ProjectBuilders.XUnit/) - Test utilities for xUnit

## Documentation

For complete documentation, please visit the [official documentation](https://github.com/dailydevops/projectbuilders/blob/main/README.md).

## Contributing

Contributions are welcome! Please read the [Contributing Guidelines](https://github.com/dailydevops/projectbuilders/blob/main/CONTRIBUTING.md) before submitting a pull request.

## Support

- **Issues**: Report bugs or request features on [GitHub Issues](https://github.com/dailydevops/projectbuilders/issues)
- **Documentation**: Read the full documentation at [https://github.com/dailydevops/projectbuilders](https://github.com/dailydevops/projectbuilders)

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/dailydevops/projectbuilders/blob/main/LICENSE) file for details.

---

> [!NOTE]
> **Made with ❤️ by the NetEvolve Team**
> Visit us at [https://www.daily-devops.net](https://www.daily-devops.net) for more information about our services and solutions.
