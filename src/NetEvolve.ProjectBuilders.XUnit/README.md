# NetEvolve.ProjectBuilders.XUnit

[![NuGet Version](https://img.shields.io/nuget/v/NetEvolve.ProjectBuilders.XUnit.svg)](https://www.nuget.org/packages/NetEvolve.ProjectBuilders.XUnit/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/NetEvolve.ProjectBuilders.XUnit.svg)](https://www.nuget.org/packages/NetEvolve.ProjectBuilders.XUnit/)
[![License](https://img.shields.io/github/license/dailydevops/projectbuilders.svg)](https://github.com/dailydevops/projectbuilders/blob/main/LICENSE)

Test utilities for xUnit-based .NET projects, providing helpers for temporary directory management and test environment setup.

## Features

- Temporary directory management for integration tests
- Utilities for test environment setup and teardown
- Seamless integration with xUnit test framework

## Installation

### NuGet Package Manager

```powershell
Install-Package NetEvolve.ProjectBuilders.XUnit
```

### .NET CLI

```bash
dotnet add package NetEvolve.ProjectBuilders.XUnit
```

### PackageReference

```xml
<PackageReference Include="NetEvolve.ProjectBuilders.XUnit" />
```

## Quick Start

```csharp
using NetEvolve.ProjectBuilders.XUnit;

// Use TemporaryDirectoryFixture in your xUnit tests
public class MyTests : IClassFixture<TemporaryDirectoryFixture>
{
    private readonly TemporaryDirectoryFixture _fixture;
    public MyTests(TemporaryDirectoryFixture fixture)
    {
        _fixture = fixture;
    }
    // Use _fixture.Directory for test files
}
```

## Usage

### Basic Example

```csharp
// Use TemporaryDirectoryFixture in xUnit tests
public class ExampleTests : IClassFixture<TemporaryDirectoryFixture>
{
    private readonly TemporaryDirectoryFixture _fixture;
    public ExampleTests(TemporaryDirectoryFixture fixture)
    {
        _fixture = fixture;
    }
    // Use _fixture.Directory for test files
}
```

## Requirements

- .NET 8.0 or higher
- [xUnit](https://www.nuget.org/packages/xunit/)

## Related Packages

- [**NetEvolve.ProjectBuilders**](https://www.nuget.org/packages/NetEvolve.ProjectBuilders/) - Core project builder utilities
- [**NetEvolve.ProjectBuilders.TUnit**](https://www.nuget.org/packages/NetEvolve.ProjectBuilders.TUnit/) - Test utilities for TUnit

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
